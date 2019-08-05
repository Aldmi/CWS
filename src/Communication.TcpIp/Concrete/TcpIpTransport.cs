using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAL.Abstract.Entities.Options.Transport;
using Serilog;
using SerilogTimings.Extensions;
using Shared.Enums;
using Shared.Extensions;
using Shared.Helpers;
using Shared.Types;
using Transport.Base.DataProvidert;
using Transport.Base.RxModel;
using Transport.TcpIp.Abstract;
using Worker.Background.Abstarct;
using Worker.Background.Concrete.HostingBackground;

namespace Transport.TcpIp.Concrete
{
    //TODO: выделить базовый абстрактный класс
    public class TcpIpTransport : ITcpIp
    {
        #region fields

        private TcpClient _client;
        private NetworkStream _netStream;

        private const int TimeCycleReOpened = 500; //Большее время занимает ожидание ответа от ConnectAsync()
        private readonly ITransportBackground _transportBg;
        private readonly ILogger _logger;

        #endregion



        #region prop

        public TcpIpOption Option { get; }
        public KeyTransport KeyTransport { get; }

        private bool _isOpen;
        public bool IsOpen
        {
            get => _isOpen;
            private set
            {
                if (value == _isOpen) return;
                _isOpen = value;
                IsOpenChangeRx.OnNext(new IsOpenChangeRxModel { IsOpen = _isOpen, TransportName = Option.Name });
            }
        }

        private string _statusString;
        public string StatusString
        {
            get => _statusString;
            private set
            {
                if (value == _statusString) return;
                _statusString = value;
                StatusStringChangeRx.OnNext(new StatusStringChangeRxModel { Status = _statusString, TransportName = Option.Name });
            }
        }

        private StatusDataExchange _statusDataExchange;
        public StatusDataExchange StatusDataExchange
        {
            get => _statusDataExchange;
            private set
            {
                if (value == _statusDataExchange) return;
                _statusDataExchange = value;
                StatusDataExchangeChangeRx.OnNext(new StatusDataExchangeChangeRxModel { StatusDataExchange = _statusDataExchange, TransportName = Option.Name });
            }
        }

        public bool IsCycleReopened { get; private set; }

        #endregion




        #region ctor

        public TcpIpTransport(ITransportBackground transportBg, TcpIpOption option, KeyTransport keyTransport, ILogger logger) //TODO: ITransportBackground вынести в абстрактный базовый класс
        {
            Option = option;
            KeyTransport = keyTransport;
            _transportBg = transportBg;
            _logger = logger;
        }

        #endregion



        #region Rx

        public ISubject<IsOpenChangeRxModel> IsOpenChangeRx { get; } = new Subject<IsOpenChangeRxModel>();                                        //СОБЫТИЕ ИЗМЕНЕНИЯ ОТКРЫТИЯ/ЗАКРЫТИЯ ПОРТА
        public ISubject<StatusDataExchangeChangeRxModel> StatusDataExchangeChangeRx { get; } = new Subject<StatusDataExchangeChangeRxModel>();     //СОБЫТИЕ ИЗМЕНЕНИЯ ОТПРАВКИ ДАННЫХ ПО ПОРТУ
        public ISubject<StatusStringChangeRxModel> StatusStringChangeRx { get; } = new Subject<StatusStringChangeRxModel>();                       //СОБЫТИЕ ИЗМЕНЕНИЯ СТРОКИ СТАТУСА ПОРТА

        #endregion



        #region Methode

        /// <summary>
        /// Циклическое открытие подключения
        /// </summary>
        private CancellationTokenSource _cycleReOpenedCts;
        public async Task CycleReOpenedExec()
        {
            if (IsCycleReopened)
            {
                _logger.Error("{Type} KeyTransport: \"{KeyTransport}\" ", "ТРАНСПОРТ УЖЕ НАХОДИТСЯ В ЦИКЛЕ ПЕРЕОТКРЫТИЯ",  KeyTransport);
                return;
            }

            //дожидаемся Перевода БГ в режим ожидания.
            await _transportBg.PutOnStendBy();

            //Запускаем задачу циклического переоткрытия соединения.
            _cycleReOpenedCts?.Cancel();
            _cycleReOpenedCts?.Dispose();
            _cycleReOpenedCts = new CancellationTokenSource();
            await Task.Factory.StartNew(async () =>
            {
               await CycleReOpened(_cycleReOpenedCts.Token);
            }, _cycleReOpenedCts.Token);
        }


        /// <summary>
        /// Отмена задачи циклического открытия подключения
        /// </summary>
        public void CycleReOpenedExecCancelation()
        {
            if (IsCycleReopened)
            {
                _cycleReOpenedCts.Cancel();
            }
        }


        private async Task<bool> CycleReOpened(CancellationToken ct)
        {
            IsCycleReopened = true;
            bool res = false;
            try
            {
                while (!ct.IsCancellationRequested && !res)
                {
                    res = await ReOpen();
                    if (!res)
                    {
                        _logger.Warning($"коннект для транспорта НЕ ОТКРЫТ: {KeyTransport}  {StatusString}");
                        await Task.Delay(TimeCycleReOpened, ct);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.Information($"ОТМЕНА ПЕРЕОТКРЫТИЯ СОЕДИНЕНИЯ ДЛЯ ТРАНСПОРТА: {KeyTransport}");
                IsCycleReopened = false;
                return false;
            }

            _logger.Information($"коннект для транспорта ОТКРЫТ: {KeyTransport}");
            IsCycleReopened = false;
            _cycleReOpenedCts?.Dispose();
            return true;
        }


        private async Task<bool> ReOpen() 
        {
            DisposeTcpClient();
            try
            {
                IsOpen = false;
                _client = new TcpClient { NoDelay = false };  //true - пакет будет отправлен мгновенно (при NetworkStream.Write). false - пока не собранно значительное кол-во данных отправки не будет.
                var ipAddress = IPAddress.Parse(Option.IpAddress);
                StatusString = $"Conect to {ipAddress} : {Option.IpPort} ...";
                await _client.ConnectAsync(ipAddress, Option.IpPort);
                _netStream = _client.GetStream();
                IsOpen = true;
                return true;
            }
            catch (Exception ex)
            {
                IsOpen = false;
                StatusString = $"Ошибка инициализации соединения: \"{ex.Message}\"";
                _logger.Debug(ex, StatusString); //TODO:??
                DisposeTcpClient();
            }
            return false;
        }


        public async Task<StatusDataExchange> DataExchangeAsync(int timeRespoune, ITransportDataProvider dataProvider, CancellationToken ct)
        {
            if (!IsOpen)
                return StatusDataExchange.NotOpenTransport;

            if (dataProvider == null)
                return StatusDataExchange.None;

            StatusDataExchange = StatusDataExchange.Start;
            if (await SendDataAsync(dataProvider, ct))
            {
                try
                {

                    var data = await TakeDataConstPeriodAsync(dataProvider.CountSetDataByte, timeRespoune, ct);
                    //var data = await TakeDataInstantlyAsync(dataProvider.CountSetDataByte, timeRespoune, ct);
                    var res = dataProvider.SetDataByte(data);
                    if (!res)
                    {
                        StatusDataExchange = StatusDataExchange.EndWithError;
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
                    _logger.Error(ex, $"TcpIpTransport {KeyTransport}. IOException");
                    StatusDataExchange = StatusDataExchange.EndWithErrorCritical;
                    return StatusDataExchange;
                }
                catch (Exception ex)
                {
                    _logger.Fatal(ex, $"TcpIpTransport {KeyTransport}. Exception");
                    StatusDataExchange = StatusDataExchange.EndWithErrorCritical;
                    return StatusDataExchange;
                }
                StatusDataExchange = StatusDataExchange.End;
                return StatusDataExchange.End;
            }

            StatusDataExchange = StatusDataExchange.EndWithError;
            return StatusDataExchange;
        }


        public async Task<bool> SendDataAsync(ITransportDataProvider dataProvider, CancellationToken ct)
        {
            byte[] buffer = dataProvider.GetDataByte();
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
                _logger.Error(ex, $"TcpIpTransport/SendDataToServer {KeyTransport}");
            }
            return false;
        }


        /// <summary>
        /// Прием данных с пеерменным периодом.
        /// Прием заканчивается когда нужное кол-во данных поступит в входной буффер порта.
        /// </summary>
        public async Task<byte[]> TakeDataInstantlyAsync(int nbytes, int timeOut, CancellationToken ct)
        {
            var ctsTimeout = new CancellationTokenSource();
            ctsTimeout.CancelAfter(timeOut);
            var cts = CancellationTokenSource.CreateLinkedTokenSource(ctsTimeout.Token, ct); // Объединенный токен, сработает от выставленного ctsTimeout.Token или от ct
            try
            {
                var dataReadTask = Task.Run(async() =>
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
                    if (receivedBytes != nbytes)
                    {
                        _logger.Warning($"TcpIpTransport/TakeDataConstPeriodAsync {KeyTransport}. КОЛ-ВО СЧИТАННЫХ БАЙТ НЕ ВЕРНОЕ. Принято/Ожидаем= \"{receivedBytes} / {nbytes}\"");
                    }
                    if (_netStream.DataAvailable)
                    {
                        _logger.Error($"TcpIpTransport/TakeDataConstPeriodAsync {KeyTransport}. ПОСЛЕ ЧТЕНИЯ В БУФЕРЕ ОСТАЛИСЬ ДАННЫЕ. buferSize= \"{receivedBytes}\"");
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
        public async Task<byte[]> TakeDataConstPeriodAsync(int nbytes, int timeOut, CancellationToken ct)
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
                if (nByteTake != nbytes)
                {
                    _logger.Warning($"TcpIpTransport/TakeDataConstPeriodAsync {KeyTransport}. КОЛ-ВО СЧИТАННЫХ БАЙТ НЕ ВЕРНОЕ. Принято/Ожидаем= \"{nByteTake} / {nbytes}\"");
                }
                if (_netStream.DataAvailable)
                {
                    _logger.Error($"TcpIpTransport/TakeDataConstPeriodAsync {KeyTransport}. ПОСЛЕ ЧТЕНИЯ В БУФЕРЕ ОСТАЛИСЬ ДАННЫЕ. buferSize= \"{buferSize}\"");
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

        private void DisposeTcpClient() 
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


        public void Dispose()
        {
            DisposeTcpClient();

            _cycleReOpenedCts?.Cancel(); 
            _cycleReOpenedCts?.Dispose();
        }

        #endregion
    }
}