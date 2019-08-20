﻿using Autofac;
using Exchange.Base.DataProviderAbstract;
using Exchange.Base.Model;
using InputDataModel.Autodictor.DataProviders.ByRuleDataProviders;
using InputDataModel.Autodictor.DataProviders.ManualDataProviders;
using InputDataModel.Autodictor.Model;

namespace WebApiSwc.AutofacModules
{
    /// <summary>
    /// Регистрируем КОНКРЕТНЫЕ провайдеры по типу.
    /// </summary>
    public class DataProviderExchangeAutofacModule<TIn> : Module
    {
        protected override void Load(ContainerBuilder builder)
        {          
            switch (typeof(TIn).Name)
            {
                case "AdInputType":
                    builder.RegisterType<VidorBinaryDataProvider>().Named<IExchangeDataProvider<AdInputType, ResponseInfo>>("VidorBinary").InstancePerDependency();
                    builder.RegisterType<ByRulesDataProvider>().Named<IExchangeDataProvider<AdInputType, ResponseInfo>>("ByRules").InstancePerDependency();
                    break;

                case "OtherType":
                    //builder.RegisterType<OtherDataProvider>().As<IExchangeDataProvider<TIn, TransportResponse>>().InstancePerDependency();
                    break;
            }      
        }
    }
}