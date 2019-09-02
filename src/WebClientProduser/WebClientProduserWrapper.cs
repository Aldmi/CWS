using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AbstractProduser.AbstractProduser;
using AbstractProduser.Helpers;
using AbstractProduser.Options;
using CSharpFunctionalExtensions;
using WebClientProduser.Options;

namespace WebClientProduser
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

        protected override async Task<Result<string, ErrorWrapper>> SendConcrete(string message, string invokerName = null, CancellationToken ct = default(CancellationToken))
        {
            try
            {
                var res = await SendHttpMessage(_option.HttpMethode, _option.Url, message, ct);
                if (res.IsSuccessStatusCode)
                    return Result.Ok<string, ErrorWrapper>(res.ToString());

                switch (res.StatusCode)
                {
                    case HttpStatusCode.RequestTimeout: 
                        return Result.Fail<string, ErrorWrapper>(new ErrorWrapper(Option.Key, ResultError.Timeout));                                   //Timeout ожидания ответа.

                    case HttpStatusCode.NotFound:
                        return Result.Fail<string, ErrorWrapper>(new ErrorWrapper(Option.Key, ResultError.NoClientBySending));                         //Нет соединения с сервером.

                    default:
                        return Result.Fail<string, ErrorWrapper>(new ErrorWrapper(Option.Key, ResultError.SendException, res.ToString()));     //Ошибка отправки
                }
            }
            catch (Exception ex)
            {
                return Result.Fail<string, ErrorWrapper>(new ErrorWrapper(Option.Key, ResultError.RespawnProduserError, ex));                         //Неизвестная ошибка обмена
            }
        }


        protected override Task<Result<string, ErrorWrapper>> SendConcrete(object message, string invokerName = null, CancellationToken ct = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        #endregion



        #region Methods

        private async Task<HttpResponseMessage> SendHttpMessage(HttpMethode methode, string strUri, string message, CancellationToken ct)
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