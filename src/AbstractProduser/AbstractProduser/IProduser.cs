using System;
using System.Threading.Tasks;
using AbstractProduser.Helpers;
using AbstractProduser.Options;
using CSharpFunctionalExtensions;

namespace AbstractProduser.AbstractProduser
{
    public interface IProduser<out TOption> : IDisposable where TOption : BaseProduserOption
    {
        TOption Option { get; }
        Task<Result<string, ErrorWrapper>> Send(string message, string invokerName = null);
        Task<Result<string, ErrorWrapper>> Send(object message, string invokerName = null);
    }
}