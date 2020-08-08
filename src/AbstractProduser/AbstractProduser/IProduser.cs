using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Produser.AbstractProduser.Helpers;
using Infrastructure.Produser.AbstractProduser.Options;
using Infrastructure.Produser.AbstractProduser.RxModels;

namespace Infrastructure.Produser.AbstractProduser.AbstractProduser
{
    public enum ProduserSendingDataType { Init, BoardData, Info, Warning }

    public interface IProduser<out TOption> : IDisposable where TOption : BaseProduserOption
    {
        TOption Option { get; }
        Task<Result<string, ErrorWrapper>> Send(object message, ProduserSendingDataType dataType);
        ISubject<ClientCollectionChangedRxModel> ClientCollectionChangedRx { get; }  // добавили/удалили клиента
    }
}