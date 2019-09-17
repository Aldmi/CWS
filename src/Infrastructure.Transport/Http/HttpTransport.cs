using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Transport.Base.Abstract;
using Infrastructure.Transport.Base.DataProvidert;
using Infrastructure.Transport.Base.RxModel;
using Shared.Enums;
using Shared.Types;

namespace Infrastructure.Transport.Http
{
    public class HttpTransport : IHttp
    {
        #region prop

        public KeyTransport KeyTransport { get; }
        public HttpOption Option { get; }

        #endregion




        #region ctor

        public HttpTransport(HttpOption option, KeyTransport keyTransport)
        {
            KeyTransport = keyTransport;
            Option = option;
        }

        #endregion




        #region Disposable

        public void Dispose()
        {
           
        }

        #endregion

        public bool IsOpen { get; }
        public string StatusString { get; }
        public StatusDataExchange StatusDataExchange { get; }
        public bool IsCycleReopened { get; }
        public ISubject<IsOpenChangeRxModel> IsOpenChangeRx { get; }
        public ISubject<StatusDataExchangeChangeRxModel> StatusDataExchangeChangeRx { get; }
        public ISubject<StatusStringChangeRxModel> StatusStringChangeRx { get; }



        public Task CycleReOpenedExec()
        {
            throw new System.NotImplementedException();
        }

        public void CycleReOpenedExecCancelation()
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> CycleReOpened()
        {
            await Task.CompletedTask;
            return true;
        }

        public void CycleReOpenedCancelation()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> ReOpen()
        {
            throw new System.NotImplementedException();
        }

        public Task<StatusDataExchange> DataExchangeAsync(int timeRespoune, ITransportDataProvider dataProvider, CancellationToken ct)
        {
            throw new System.NotImplementedException();
        }
    }
}