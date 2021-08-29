using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Domain.InputDataModel.Base.InData;
using Serilog;
using Shared.Paged;

namespace Domain.Device.Paged4InData
{
    /// <summary>
    /// Обертка над PagedService.
    /// Добаквляет к выходным данным NextPageRx, InputData объект.
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    public class PagingInvokeService<TIn> : IDisposable
    {
        private readonly PagedService<TIn> _pagedService;
        private readonly ILogger _logger;
        private InputData<TIn> _inputData;
        public IObservable<InputData<TIn>> NextPageRx { get; }
        
        
        public PagingInvokeService(PagedOption option, ILogger logger)
        {
            _logger = logger;
            _pagedService = new PagedService<TIn>(option);
            NextPageRx= _pagedService.NextPageRx.Select(mem =>
            {
                _inputData.Data = mem.ToArray().ToList();
                return _inputData;
            });
        }

        

        public void SetData(InputData<TIn> inData)
        {
            _inputData = inData;
            _pagedService.SetData(inData.Data.ToArray());
        }
        
        
        
        public void Dispose()
        {
            _pagedService.Dispose();
        }
    }
}