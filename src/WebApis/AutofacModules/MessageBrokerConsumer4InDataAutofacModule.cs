﻿using System;
using System.Collections.Generic;
using App.Services.InputData;
using App.Services.MessageBroker;
using Autofac;
using Autofac.Core;
using Domain.InputDataModel.Base.InData;
using Infrastructure.Background.Abstarct;
using Infrastructure.Background.Concrete.HostingBackground;
using Infrastructure.MessageBroker.Abstract;
using Infrastructure.MessageBroker.Options;
using Microsoft.Extensions.Configuration;


namespace WebApiSwc.AutofacModules
{
    public class MessageBrokerConsumer4InDataAutofacModule<TIn> : Module where TIn : InputTypeBase
    {
        public string BackgroundName { get;  }
        public bool AutoStartBg { get;  }
        public int BatchSize { get;  }
        public ConsumerOption ConsumerOption { get;}


        #region ctor
        public MessageBrokerConsumer4InDataAutofacModule(IConfigurationSection config)
        {
            BackgroundName= config["Name"];
            AutoStartBg=  bool.Parse(config["AutoStart"]);
            BatchSize = int.Parse(config["BatchSize"]);
            ConsumerOption= new ConsumerOption
            {
                BrokerEndpoints = config["BrokerEndpoints"],
                GroupId = config["GroupId"],
                Topics = new List<string> {config["Topics"]}
            };
        }
        #endregion


        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InputDataApplyService<TIn>>().InstancePerDependency();

            builder.RegisterType<HostingBackgroundSimple>()
                .Named<ISimpleBackground>(BackgroundName)
                .WithParameters(new List<ResolvedParameter>
                {
                    new ResolvedParameter(
                        (pi, ctx) => (pi.ParameterType == typeof(string) && (pi.Name == "key")),
                        (pi, ctx) => BackgroundName),
                    new ResolvedParameter(
                        (pi, ctx) => (pi.ParameterType == typeof(bool) && (pi.Name == "autoStart")),
                        (pi, ctx) => AutoStartBg),
                }).SingleInstance();

            builder.RegisterType<ConsumerMessageBroker4InputData<TIn>>()
                .WithParameters(new List<ResolvedParameter>
                {
                    new ResolvedParameter(
                        (pi, ctx) => (pi.ParameterType == typeof(ISimpleBackground) && (pi.Name == "background")),
                        (pi, ctx) => ctx.ResolveNamed<ISimpleBackground>(BackgroundName)),
                    new ResolvedParameter(
                        (pi, ctx) => (pi.ParameterType == typeof(int) && (pi.Name == "batchSize")),
                        (pi, ctx) => BatchSize),           
                    new ResolvedParameter(
                        (pi, ctx) => (pi.ParameterType == typeof(IConsumer) && (pi.Name == "consumer")),
                        (pi, ctx) =>
                        {
                            var consumerFactory = ctx.Resolve<Func<ConsumerOption, IConsumer>>();
                            return consumerFactory(ConsumerOption);
                        })
                }).SingleInstance();        
        }
    }
}
