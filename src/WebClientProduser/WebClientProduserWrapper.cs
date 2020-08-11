using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Produser.AbstractProduser.AbstractProduser;
using Infrastructure.Produser.AbstractProduser.Enums;
using Infrastructure.Produser.AbstractProduser.Helpers;
using Infrastructure.Produser.WebClientProduser.Options;
using Shared.Enums;

namespace Infrastructure.Produser.WebClientProduser
{
    public class WebClientProduserWrapper : BaseProduser<WebClientProduserOption>
    {
        #region field
        private readonly WebClientProduserOption _option;
        private readonly IHttpClientSupport _httpClientSupport;
        #endregion



        #region ctor
        public WebClientProduserWrapper(WebClientProduserOption option, IHttpClientSupport httpClientSupport) : base(option)
        {
            _option = option;
            _httpClientSupport = httpClientSupport;
        }
        #endregion



        #region OvverideMembers
        protected override Task<Result<string, ErrorWrapper>> SendInit(object message, CancellationToken ct = default(CancellationToken))
        {
            var strUri = Option.InitUrl;
            var mes = message.ToString();
            return SendConcrete(mes, strUri, ct);
        }

        protected override Task<Result<string, ErrorWrapper>> SendBoardData(object message, CancellationToken ct = default(CancellationToken))
        {
            var strUri = Option.BoardDataUrl;
            var mes = message.ToString();
            return SendConcrete(mes, strUri, ct);
        }

        protected override Task<Result<string, ErrorWrapper>> SendInfo(object message, CancellationToken ct = default(CancellationToken))
        {
            var strUri = Option.InfoUrl;
            var mes = message.ToString();
            return SendConcrete(mes, strUri, ct);
        }

        protected override Task<Result<string, ErrorWrapper>> SendWarning(object message, CancellationToken ct = default(CancellationToken))
        {
            var strUri = Option.WarningUrl;
            var mes = message.ToString();
            return SendConcrete(mes, strUri, ct);
        }
        #endregion



        #region Methods

        private async Task<Result<string, ErrorWrapper>> SendConcrete(string message, string strUri, CancellationToken ct = default(CancellationToken))
        {
            try
            {
                var res = await SendHttp(_option.HttpMethode, strUri, message, ct);
                if (res.IsSuccessStatusCode)
                    return Result.Ok<string, ErrorWrapper>(res.ToString());

                switch (res.StatusCode)
                {
                    case HttpStatusCode.RequestTimeout:
                        return Result.Failure<string, ErrorWrapper>(new ErrorWrapper(Option.Key, ResultError.Timeout));                                   //Timeout ожидания ответа.

                    case HttpStatusCode.NotFound:
                        return Result.Failure<string, ErrorWrapper>(new ErrorWrapper(Option.Key, ResultError.NoClientBySending));                         //Нет соединения с сервером.

                    default:
                        return Result.Failure<string, ErrorWrapper>(new ErrorWrapper(Option.Key, ResultError.SendException, res.ToString()));     //Ошибка отправки
                }
            }
            catch (Exception ex)
            {
                return Result.Failure<string, ErrorWrapper>(new ErrorWrapper(Option.Key, ResultError.RespawnProduserError, ex));                         //Неизвестная ошибка обмена
            }
        }


        private async Task<HttpResponseMessage> SendHttp(HttpMethode methode, string strUri, string message, CancellationToken ct)
        {
            HttpResponseMessage resp;
            Uri uri;
            switch (methode)
            {
                case HttpMethode.Get:
                    uri = new UriBuilder(strUri + $"/{message}").Uri;
                    resp = await _httpClientSupport.GetAsync(uri, ct);
                    return resp;

                case HttpMethode.Post:
                    uri = new UriBuilder(strUri).Uri;
                    resp = await _httpClientSupport.PostAsync(uri, message, ct);
                    return resp;

                default:
                    throw new NotSupportedException($"{_option.HttpMethode} не подерживается обработкой");
            }
        }
        #endregion



        #region Disposable

        public override void Dispose()
        {
            //HttpClientSupport.HttpClient создается через HttpClientFactory, следовательно DI управляет временем его жизни.
        }
        #endregion
    }
}