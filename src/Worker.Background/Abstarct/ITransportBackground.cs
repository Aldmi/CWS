using System;
using System.Threading;
using System.Threading.Tasks;
using Shared.Types;
using Worker.Background.Enums;

namespace Worker.Background.Abstarct
{
    public interface ITransportBackground : IBackground, ISupportKeyTransport
    {
        void AddCycleAction(Func<CancellationToken, Task> action);
        void RemoveCycleFunc(Func<CancellationToken, Task> action);
        Task<StatusBackground> PutOnStendBy();                            //ПЕРЕВЕСТИ БГ В РЕЖИМ ГОТОВНОСТИ (ОЖИДАНИЯ)
        void PutOnWork();                                                 //ПЕРЕВЕСТИ БГ В РЕЖИМ РАБОТЫ
    }
}