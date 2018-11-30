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
using Shared.Helpers;
using Shared.Types;
using Transport.Base.DataProvidert;
using Transport.Base.RxModel;
using Transport.TcpIp.Abstract;

namespace Transport.TcpIp.Concrete
{
    public class TcpIpTransport : ITcpIp
    {
        #region fields

        private TcpClient _client;
        private NetworkStream _netStream;

        private const int TimeCycleReOpened = 3000;
        private CancellationTokenSource _ctsCycleReOpened;
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

        public TcpIpTransport(TcpIpOption option, KeyTransport keyTransport, ILogger logger)
        {
            Option = option;
            KeyTransport = keyTransport;
            _logger = logger;
        }

        #endregion



        #region Rx

        public ISubject<IsOpenChangeRxModel> IsOpenChangeRx { get; } = new Subject<IsOpenChangeRxModel>();                                        //СОБЫТИЕ ИЗМЕНЕНИЯ ОТКРЫТИЯ/ЗАКРЫТИЯ ПОРТА
        public ISubject<StatusDataExchangeChangeRxModel> StatusDataExchangeChangeRx { get; } = new Subject<StatusDataExchangeChangeRxModel>();     //СОБЫТИЕ ИЗМЕНЕНИЯ ОТПРАВКИ ДАННЫХ ПО ПОРТУ
        public ISubject<StatusStringChangeRxModel> StatusStringChangeRx { get; } = new Subject<StatusStringChangeRxModel>();                       //СОБЫТИЕ ИЗМЕНЕНИЯ СТРОКИ СТАТУСА ПОРТА

        #endregion



        #region Methode

        public async Task<bool> CycleReOpened()
        {
            IsCycleReopened = true;
            _ctsCycleReOpened = new CancellationTokenSource();
            bool res = false;
            while (!_ctsCycleReOpened.IsCancellationRequested && !res)
            {
                res = await ReOpen();
                if (!res)
                {
                    _logger.Warning($"коннект для транспорта НЕ ОТКРЫТ: {KeyTransport}");
                    await Task.Delay(TimeCycleReOpened, _ctsCycleReOpened.Token);
                }
            }
            _logger.Information($"коннект для транспорта ОТКРЫТ: {KeyTransport}");
            IsCycleReopened = false;
            return true;
        }


        public void CycleReOpenedCancelation()
        {
            if (IsCycleReopened)
                _ctsCycleReOpened.Cancel();
        }


        public async Task<bool> ReOpen()
        {
            try
            {
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
                Dispose();
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
                    //var data = await TakeDataInstantlyAsync(dataProvider.CountSetDataByte, timeRespoune, ct);
                    var data = await TakeDataConstPeriodAsync(dataProvider.CountSetDataByte, timeRespoune, ct);
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
            using (_logger.TimeOperation("TimeOpertaion TakeDataAsync"))
            {
                var ctsTimeout = new CancellationTokenSource();//токен сработает по таймауту в функции WithTimeout
                var cts = CancellationTokenSource.CreateLinkedTokenSource(ctsTimeout.Token, ct); // Объединенный токен, сработает от выставленного ctsTimeout.Token или от ct
                //await Task.Delay(timeOut, ct);//DEBUG
                var task = Task<List<byte>>.Factory.StartNew(() =>
                 {
                     var sumBuffer = new List<byte>();
                     byte[] data = new byte[1024];
                     do
                     {
                         //var nByteTake = _netStream.ReadAsync(data, 0, data.Length, cts.Token).GetAwaiter().GetResult();
                         var nByteTake = _netStream.Read(data, 0, data.Length);
                         sumBuffer.AddRange(data.Take(nByteTake));
                     }
                     while (_netStream.DataAvailable); // пока данные есть в потоке
                     return sumBuffer;
                 }, cts.Token);

                var buffer = await task.WithTimeout2CanceledTask(timeOut, ctsTimeout);
                var resBuffer = buffer.Take(nbytes).Where(val => val != 0x00).ToArray();
                if (resBuffer.Length == nbytes)
                {
                    var bData = new byte[nbytes];
                    Array.Copy(buffer.ToArray(), bData, nbytes);
                    return bData;
                }

                _logger.Warning($"TcpIpTransport/TakeDataAsync {KeyTransport}.  Кол-во считанных данных не верное  Принято= {resBuffer.Length}  Ожидаем= {nbytes}");
                return null;
            }
        }


        /// <summary>
        /// Прием данных с постоянным периодом.
        /// </summary>
        public async Task<byte[]> TakeDataConstPeriodAsync(int nbytes, int timeOut, CancellationToken ct)
        {
            byte[] bDataTemp = new byte[256];

            //timeOut = 150;//DEBUG

            //Ожидаем накопление данных в буффере
            _netStream.ReadTimeout = timeOut;
            _client.ReceiveTimeout = timeOut;
            await Task.Delay(timeOut, ct);

            //Мгновенно с ожиданием в 100мс вычитываем поступивщий буффер
            var ctsTimeout = new CancellationTokenSource();//токен сработает по таймауту в функции WithTimeout
            var cts = CancellationTokenSource.CreateLinkedTokenSource(ctsTimeout.Token, ct); // Объединенный токен, сработает от выставленного ctsTimeout.Token или от ct
            int nByteTake = await _netStream.ReadAsync(bDataTemp, 0, nbytes, cts.Token).WithTimeout2CanceledTask(50, ctsTimeout);
            if (nByteTake == nbytes)
            {
                var bData = new byte[nByteTake];
                Array.Copy(bDataTemp, bData, nByteTake);
                return bData;
            }


            //DEBUG-----------------------
            for (int j = 0; j < 10; j++)
            {
                var trash = new byte[4096];
                var res = _netStream.DataAvailable;
                if (res)
                {
                    var n = _netStream.Read(trash, 0, trash.Length);
                    Debug.WriteLine("В буффере после чтения остались данные. Маленкьое время ожидания ответа.");
                    break;
                }
                await Task.Delay(200, ct);
            }
            //DEBUG-----------------------

            //Очистить остатки буфера от мусора
            //using (_logger.TimeOperation("TimeOpertaion Clear In buffer"))
            //{
            //    var trash = new byte[4096];
            //    while (_netStream.DataAvailable)
            //    {
            //        _netStream.Read(trash, 0, trash.Length);
            //    }
            //}

            _logger.Warning($"TcpIpTransport/TakeDataAsync {KeyTransport}.  Кол-во считанных БАЙТ не верное.  Принято/Ожидаем= \"{nByteTake} / {nbytes}\"");
            return null;
        }

        #endregion



        #region Disposable

        public void Dispose()
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