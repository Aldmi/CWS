using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Background.Enums;
using Shared.Types;

namespace Infrastructure.Background.Abstarct
{
    public interface ITransportBackground : IBackground, ISupportKeyTransport
    {
        void AddCycleAction(Func<CancellationToken, Task> action);
        void RemoveCycleFunc(Func<CancellationToken, Task> action);
        Task<StatusBackground> PutOnStendBy();                            //ПЕРЕВЕСТИ БГ В РЕЖИМ ГОТОВНОСТИ (ОЖИДАНИЯ)
        void PutOnWork();                                                 //ПЕРЕВЕСТИ БГ В РЕЖИМ РАБОТЫ
    }
}