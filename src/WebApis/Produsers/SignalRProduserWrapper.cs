using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Domain.Device.Repository.Entities.ResponseProduser;
using Infrastructure.Produser.AbstractProduser.AbstractProduser;
using Infrastructure.Produser.AbstractProduser.Enums;
using Infrastructure.Produser.AbstractProduser.Helpers;
using Microsoft.AspNetCore.SignalR;
using WebApiSwc.Hubs;
using WebApiSwc.SignalRClients;

namespace WebApiSwc.Produsers
{
    public class SignalRProduserWrapper : BaseProduser<SignalRProduserOption>
    {
        #region fields
        private readonly IHubContext<ProviderHub> _hubProxy;
        private readonly SignaRProduserClientsStorage<SignaRProdusserClientsInfo> _clientsStorage;
        #endregion



        #region ctor
        public SignalRProduserWrapper(IHubContext<ProviderHub> hubProxy, SignaRProduserClientsStorage<SignaRProdusserClientsInfo> clientsStorage, SignalRProduserOption option) 
            : base(option)
        {
            _hubProxy = hubProxy;
            _clientsStorage = clientsStorage;
            _clientsStorage.CollectionChangedRx.Subscribe(ClientCollectionChangedRxEh);
        }
        #endregion



        #region OvverideMembers

        protected override async Task<Result<string, ErrorWrapper>> SendConcrete(string message, string invokerName = null, CancellationToken ct = default(CancellationToken))
        {
            if (!_clientsStorage.Any)
                return Result.Failure<string, ErrorWrapper>(new ErrorWrapper(Option.Key, ResultError.NoClientBySending));

            try
            {                
                invokerName ??= Option.MethodeName;
                await _hubProxy.Clients.All.SendCoreAsync(invokerName, new object[] { message }, ct);
                return Result.Ok<string, ErrorWrapper>("Ok");
            }
            catch (Exception ex)
            {
                return Result.Failure<string, ErrorWrapper>(new ErrorWrapper(Option.Key, ResultError.RespawnProduserError, ex));
            }
        }


        protected override async Task<Result<string, ErrorWrapper>> SendConcrete(object message, string invokerName = null, CancellationToken ct = default(CancellationToken))
        {
            if (!_clientsStorage.Any)
                return Result.Failure<string, ErrorWrapper>(new ErrorWrapper(Option.Key, ResultError.NoClientBySending));

            try
            {
                invokerName ??= Option.MethodeName;
                await _hubProxy.Clients.All.SendCoreAsync(invokerName, new[] { message }, ct);
                return Result.Ok<string, ErrorWrapper>("Ok");
            }
            catch (Exception ex)
            {
                return Result.Failure<string, ErrorWrapper>(new ErrorWrapper(Option.Key, ResultError.RespawnProduserError, ex));
            }
        }

        #endregion



        #region Disposable

        public override void Dispose()
        {
        }

        #endregion
    }
}