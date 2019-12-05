﻿using Autofac;
using Domain.InputDataModel.Autodictor.IndependentInseartsHandlers;
using Domain.InputDataModel.Autodictor.Model;
using Domain.InputDataModel.Autodictor.ProvidersSpecial;
using Domain.InputDataModel.Autodictor.StronglyTypedResponse;
using Domain.InputDataModel.Base.InData;
using Domain.InputDataModel.Base.InseartServices.IndependentInsearts.IndependentInseartsHandlers;
using Domain.InputDataModel.Base.ProvidersAbstract;
using Domain.InputDataModel.Base.ProvidersConcrete.ByRuleDataProviders;
using Domain.InputDataModel.Base.Response;
using Infrastructure.Transport.Base.DataProvidert;

namespace WebApiSwc.AutofacModules
{
    /// <summary>
    /// Регистрируем КОНКРЕТНЫЕ провайдеры по типу.
    /// </summary>
    public class DataProviderAutofacModule<TIn> : Module where TIn : InputTypeBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            switch (typeof(TIn).Name)
            {
                case "AdInputType":
                    builder.RegisterType<AdStronglyTypedResponseFactory>().As<IStronglyTypedResponseFactory>().SingleInstance();
                    builder.RegisterType<AdInputTypeIndependentInsertsHandler>().As<IIndependentInsertsHandler>().SingleInstance();

                    builder.RegisterType<VidorBinaryDataProvider>().Named<IDataProvider<TIn, ResponseInfo>>("VidorBinary").InstancePerDependency();
                    builder.RegisterType<ByRulesDataProvider<TIn>>().Named<IDataProvider<TIn, ResponseInfo>>("ByRules").InstancePerDependency();
                    break;

                case "OtherType":
                    //builder.RegisterType<OtherTypeStronglyTypedResponseFactory>().As<IStronglyTypedResponseFactory>().SingleInstance();
                    //builder.RegisterType<OtherDataProvider>().As<IExchangeDataProvider<TIn, TransportResponse>>().InstancePerDependency();
                    break;
            }

            builder.RegisterType<ProviderResult<TIn>>().InstancePerDependency();
        }
    }
}