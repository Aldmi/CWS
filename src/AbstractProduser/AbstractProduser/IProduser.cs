using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Infrastructure.Produser.AbstractProduser.Helpers;
using Infrastructure.Produser.AbstractProduser.Options;

namespace Infrastructure.Produser.AbstractProduser.AbstractProduser
{
    public interface IProduser<out TOption> : IDisposable where TOption : BaseProduserOption
    {
        TOption Option { get; }
        Task<Result<string, ErrorWrapper>> Send(string message, string invokerName = null);
        Task<Result<string, ErrorWrapper>> Send(object message, string invokerName = null);
    }
}