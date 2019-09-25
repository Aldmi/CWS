using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Background.Abstarct;
using Infrastructure.Transport.Base.Abstract;
using Infrastructure.Transport.Base.DataProvidert;
using Serilog;
using Shared.Enums;
using Shared.Types;

namespace Infrastructure.Transport.SibWay
{
    public class SibWayTransport : BaseTransport, ISibWay

    {
        public SibWayTransport(ITransportBackground transportBg, KeyTransport keyTransport, ILogger logger) : base(transportBg, keyTransport, logger)
        {
        }

        protected override Task<bool> ReOpen()
        {
            throw new System.NotImplementedException();
        }

        public override Task<StatusDataExchange> DataExchangeAsync(int timeRespoune, ITransportDataProvider dataProvider, CancellationToken ct)
        {
            throw new System.NotImplementedException();
        }

        protected override void DisposeTransport()
        {
            throw new System.NotImplementedException();
        }

        public SibWayTransportOption Option { get; }
    }
}