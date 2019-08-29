using Infrastructure.Storages;
using Shared.Types;
using Worker.Background.Abstarct;

namespace BL.Services.Storages
{
    public class BackgroundStorage : BaseStorage<KeyTransport, ITransportBackground>
    {

    }
}