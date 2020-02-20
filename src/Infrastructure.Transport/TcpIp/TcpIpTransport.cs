using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Background.Abstarct;
using Infrastructure.Transport.Base.Abstract;
using Infrastructure.Transport.Base.DataProvidert;
using Serilog;
using Shared.Enums;
using Shared.Types;

namespace Infrastructure.Transport.TcpIp
{
    public class TcpIpTransport : BaseTransport, ITcpIp
    {
        #region fields

        private TcpClient _client;
        private NetworkStream _netStream;

        #endregion



        #region prop

        public TcpIpOption Option { get; }

        #endregion



        #region ctor

        public TcpIpTransport(ITransportBackground transportBg, TcpIpOption option, KeyTransport keyTransport, ILogger logger) 
            : base(transportBg, keyTransport, logger)
        {
            Option = option;
        }

        #endregion



        #region OverrideMembers

        protected override async Task<bool> ReOpen()
        {
            DisposeTransport();
            try
            {
                _client = new TcpClient { NoDelay = false };  //true - пакет будет отправлен мгновенно (при NetworkStream.Write). false - пока не собранно значительное кол-во данных отправки не будет.
                var ipAddress = IPAddress.Parse(Option.IpAddress);
                StatusString = $"Conect to {ipAddress} : {Option.IpPort} ...";
                await _client.ConnectAsync(ipAddress, Option.IpPort);
                _netStream = _client.GetStream();
                return true;
            }
            catch (Exception ex)
            {
                //IsOpen = false;
                StatusString = $"Ошибка инициализации соединения: \"{ex.Message}\"";
                Logger.Debug(ex, StatusString); //TODO:??
                DisposeTransport();
            }
            return false;
        }


        public override async Task<StatusDataExchange> DataExchangeAsync(ITransportDataProvider dataProvider, CancellationToken ct)
        {
            if (!IsOpen)
                return StatusDataExchange.NotOpenTransport;

            if (dataProvider == null)
                return StatusDataExchange.None;

            StatusDataExchange = StatusDataExchange.Start;
            if (await SendDataAsync(dataProvider.GetDataByte(), ct))
            {
                try
                {

                    var data = await TakeDataConstPeriodAsync(dataProvider.TimeRespone, ct);
                    //var data = await TakeDataInstantlyAsync(dataProvider.CountSetDataByte, timeRespoune, ct);
                    var res = dataProvider.SetDataByte(data);
                    if (!res)
                    {
                        StatusDataExchange = StatusDataExchange.EndWithErrorWrongAnswers;
                        return StatusDataExchange;
                    }
                }
                catch (OperationCanceledException)
                {
                    StatusDataExchange = StatusDataExchange.EndWithCanceled;
                    return StatusDataExchange;
                }
                catch (TimeoutException)
                {
                    StatusDataExchange = StatusDataExchange.EndWithTimeout;
                    return StatusDataExchange;
                }
                catch (IOException ex)
                {
                    Logger.Error(ex, $"TcpIpTransport {KeyTransport}. IOException");
                    StatusDataExchange = StatusDataExchange.EndWithErrorCritical;
                    return StatusDataExchange;
                }
                catch (Exception ex)
                {
                    Logger.Fatal(ex, $"TcpIpTransport {KeyTransport}. Exception");
                    StatusDataExchange = StatusDataExchange.EndWithErrorCritical;
                    return StatusDataExchange;
                }
                StatusDataExchange = StatusDataExchange.End;
                return StatusDataExchange.End;
            }

            StatusDataExchange = StatusDataExchange.EndWithErrorCannotSendData;
            return StatusDataExchange;
        }

        #endregion



        #region Methode

        private async Task<bool> SendDataAsync(byte[] buffer, CancellationToken ct)
        {
            try
            {
                if (_client != null && _netStream != null && _client.Client != null && _client.Client.Connected)
                {
                    await _netStream.WriteAsync(buffer, 0, buffer.Length, ct);
                    return true;
                }
            }
            catch (Exception ex)
            {
                StatusString = $"ИСКЛЮЧЕНИЕ SendDataToServer :{ex.Message}";
                Logger.Error(ex, $"TcpIpTransport/SendDataToServer {KeyTransport}");
            }
            return false;
        }


        /// <summary>
        /// Прием данных с пеерменным периодом.
        /// Прием заканчивается когда нужное кол-во данных поступит в входной буффер порта.
        /// </summary>
        private async Task<byte[]> TakeDataInstantlyAsync(int nbytes, int timeOut, CancellationToken ct)
        {
            var ctsTimeout = new CancellationTokenSource();
            ctsTimeout.CancelAfter(timeOut);
            var cts = CancellationTokenSource.CreateLinkedTokenSource(ctsTimeout.Token, ct); // Объединенный токен, сработает от выставленного ctsTimeout.Token или от ct
            try
            {
                var dataReadTask = Task.Run(async () =>
                {
                    const int polingTime = 100;
                    var sumBuffer = new List<byte>();
                    byte[] data = new byte[256];
                    while (true)
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        if (_netStream.DataAvailable)
                        {
                            var nByteTake = _netStream.Read(data, 0, data.Length);
                            sumBuffer.AddRange(data.Take(nByteTake));
                            if (sumBuffer.Count >= nbytes)
                                return sumBuffer;
                        }
                        await Task.Delay(polingTime, cts.Token);
                    }
                }, cts.Token);

                try
                {
                    var buffer = await dataReadTask;
                    var receivedBytes = buffer.Count;
                    //if (receivedBytes != nbytes)
                    //{
                    //     Logger.Warning($"TcpIpTransport/TakeDataConstPeriodAsync {KeyTransport}. КОЛ-ВО СЧИТАННЫХ БАЙТ НЕ ВЕРНОЕ. Принято/Ожидаем= \"{receivedBytes} / {nbytes}\"");
                    //}
                    if (_netStream.DataAvailable)
                    {
                        Logger.Error($"TcpIpTransport/TakeDataConstPeriodAsync {KeyTransport}. ПОСЛЕ ЧТЕНИЯ В БУФЕРЕ ОСТАЛИСЬ ДАННЫЕ. buferSize= \"{receivedBytes}\"");
                    }
                    var resBuffer = buffer.Take(nbytes).ToArray();
                    return resBuffer;
                }
                catch (OperationCanceledException)
                {
                    throw new TimeoutException();
                }
            }
            finally
            {
                await _netStream.FlushAsync(cts.Token);
                ctsTimeout.Cancel();
                cts.Cancel();
                ctsTimeout.Dispose();
                cts.Dispose();
            }
        }


        /// <summary>
        /// Прием данных с постоянным периодом.
        /// </summary>
        private async Task<byte[]> TakeDataConstPeriodAsync(int timeOut, CancellationToken ct)
        {
            int buferSize = 256;// читаем всегда весь буфер
            byte[] bDataTemp = new byte[buferSize];

            //Ожидаем накопление данных в буффере
            await Task.Delay(timeOut, ct);

            #region DebugMEMLIK
            // Console.WriteLine("TakeDataConstPeriodAsync >>>>>>>>>>>>>>>>>");
            //return new byte[] { 0x02, 0x46, 0x46, 0x30, 0x38, 0x25, 0x41, 0x30, 0x37, 0x37, 0x41, 0x43, 0x4B, 0x45, 0x41, 0x03 };
            #endregion
            try
            {
                if (!_netStream.DataAvailable)
                {
                    throw new TimeoutException();
                }
                int nByteTake = _netStream.Read(bDataTemp, 0, buferSize);
                //if (nByteTake != nbytes)
                //{
                //    Logger.Warning($"TcpIpTransport/TakeDataConstPeriodAsync {KeyTransport}. КОЛ-ВО СЧИТАННЫХ БАЙТ НЕ ВЕРНОЕ. Принято/Ожидаем= \"{nByteTake} / {nbytes}\"");
                //}
                if (_netStream.DataAvailable)
                {
                    Logger.Error($"TcpIpTransport/TakeDataConstPeriodAsync {KeyTransport}. ПОСЛЕ ЧТЕНИЯ В БУФЕРЕ ОСТАЛИСЬ ДАННЫЕ. buferSize= \"{buferSize}\"");
                }

                var bData = new byte[nByteTake];
                Array.Copy(bDataTemp, bData, nByteTake);
                return bData;
            }
            finally
            {
                await _netStream.FlushAsync(ct);
            }
        }

        #endregion



        #region Disposable

        protected override void DisposeTransport()
        {
            if (_netStream != null)
            {
                _netStream.Close();
                StatusString = "Сетевой поток закрыт ...";
            }

            _client?.Client?.Close();
            _client?.Client?.Dispose();
            _client?.Dispose();
        }

        #endregion
    }
}