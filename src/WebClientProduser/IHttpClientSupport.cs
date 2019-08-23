using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebClientProduser
{
    public interface IHttpClientSupport
    {
        Task<HttpResponseMessage> GetAsync(Uri uri, CancellationToken ct);
        Task<HttpResponseMessage> PostAsync(Uri uri, string message, CancellationToken ct);
    }
}