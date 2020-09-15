using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Domain.InputDataModel.Base.Enums;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.ProvidersOption;
using Domain.InputDataModel.OpcServer.Model;
using Serilog;
using Shared.Types;

namespace Domain.InputDataModel.OpcServer.ForProviderImpl.ProvidersSpecial
{
    public class OpcSpecialDataProvider : BaseDataProvider<OpcInputType>, IDataProvider<OpcInputType>
    {
        public string ProviderName { get; }                                     //TODO: возможно вынести в base
        public Dictionary<string, string> StatusDict { get; } = new Dictionary<string, string>();   //TODO: возможно вынести в base
        public Subject<ProviderResult<OpcInputType>> RaiseSendDataRx { get; }  //TODO: возможно вынести в base



        public OpcSpecialDataProvider(Func<ProviderTransfer<OpcInputType>, IDictionary<string, string>, ProviderResult<OpcInputType>> providerResultFactory, ILogger logger) : base(providerResultFactory, logger)
        {
            ProviderName = "OpcSpecial"; //providerOption.Name;
        }




        public async Task StartExchangePipelineAsync(InDataWrapper<OpcInputType> inData, CancellationToken ct)
        {
            await Task.Delay(1000, ct);

            //Добавили статусы
            StatusDict["State 1"] = "qqq";
            StatusDict["State 2"] = "ppp";

            //REQUEST
            var requestOption = new RequestOption
            {
                Header = "FF0102",
                Body = "AAAA",
                Footer = "",
                Format = "HEX",
                MaxBodyLenght = 254
            };
            //RESPONSE
            var respOption = new ResponseOption
            {
                TimeRespone = 1000,
                ValidatorName = ""
            };
            var respValidatorResult = respOption.CreateValidator();

            //TRANSFER
            var transfer = new ProviderTransfer<OpcInputType>
            {
                Request = new RequestTransfer<OpcInputType>(requestOption),
                Response = new ResponseTransfer(respOption, respValidatorResult.Value),
                Command = Command4Device.None
            };
            //заполнили статистику обработки элемента ВХОДНЫХ данных
            transfer.Request.ProcessedItemsInBatch = new ProcessedItemsInBatch<OpcInputType>(0, 1, new List<ProcessedItem<OpcInputType>>
                {
                   new ProcessedItem<OpcInputType>(new OpcInputType(1, "prop1", 220.0),
                  new ReadOnlyDictionary<string, Change<string>>(new Dictionary<string, Change<string>>
                      {
                          { "1", new Change<string>("220.0", "220.0") }
                      }))
                });

            var providerResult = ProviderResultFactory(transfer, StatusDict);
            RaiseSendDataRx.OnNext(providerResult);
        }



        public ProviderOption GetCurrentOption()
        {
            throw new NotImplementedException();
        }
    }
}