using System;
using System.Threading;
using System.Threading.Tasks;
using AbstractProduser.AbstractProduser;
using AbstractProduser.Helpers;
using CSharpFunctionalExtensions;
using Domain.Device.Repository.Entities.ResponseProduser;
using Microsoft.AspNetCore.SignalR;
using WebApiSwc.Hubs;
using WebApiSwc.SignalRClients;

namespace WebApiSwc.Produsers
{
    public class SignalRProduserWrapper : BaseProduser<SignalRProduserOption>
    {
        private readonly IHubContext<ProviderHub> _hubProxy;
        private readonly SignaRProduserClientsStorage<SignaRProdusserClientsInfo> _clientsStorage;



        #region ctor

        public SignalRProduserWrapper(IHubContext<ProviderHub> hubProxy, SignaRProduserClientsStorage<SignaRProdusserClientsInfo> clientsStorage, SignalRProduserOption option) 
            : base(option)
        {
            _hubProxy = hubProxy;
            _clientsStorage = clientsStorage;
        }

        #endregion

        
    
        #region OvverideMembers

        protected override async Task<Result<string, ErrorWrapper>> SendConcrete(string message, string invokerName = null, CancellationToken ct = default(CancellationToken))
        {
            if(!_clientsStorage.Any)
                return Result.Fail<string, ErrorWrapper>(new ErrorWrapper(Option.Key, ResultError.NoClientBySending));

            try
            {                
                invokerName = invokerName ?? Option.MethodeName;
                await _hubProxy.Clients.All.SendCoreAsync(invokerName, new object[] { message }, ct);
                return Result.Ok<string, ErrorWrapper>("Ok");
            }
            catch (Exception ex)
            {
                return Result.Fail<string, ErrorWrapper>(new ErrorWrapper(Option.Key, ResultError.RespawnProduserError, ex));
            }
        }


        protected override Task<Result<string, ErrorWrapper>> SendConcrete(object message, string invokerName = null, CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        #endregion



        #region Disposable

        public override void Dispose()
        {
        }

        #endregion
    }
}