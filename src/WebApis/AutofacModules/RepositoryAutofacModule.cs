using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using Domain.Device.Repository.Abstract;
using Domain.Device.Repository.Concrete.EF;
using Domain.Exchange.Repository.Abstract;
using Domain.Exchange.Repository.Concrete;
using Domain.InputDataModel.Shared.Repository.Abstract;
using Domain.InputDataModel.Shared.Repository.Concrete.EF;
using Infrastructure.Dal.Abstract;
using Infrastructure.Dal.EfCore;
using Infrastructure.Dal.EfCore.Entities.StringInsertModelExt;
using Infrastructure.Transport.Repository.Abstract;
using Infrastructure.Transport.Repository.Concrete.EF;

namespace WebApiSwc.AutofacModules
{
    public class RepositoryAutofacModule : Module
    {
        private readonly string _connectionString;



        #region ctor

        public RepositoryAutofacModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion



        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EfActionDb>().As<IActionDb>()
                .WithParameters(new List<Parameter>
                {
                    new NamedParameter("connectionString", _connectionString),
                })
                .InstancePerLifetimeScope();

            builder.RegisterType<EfSerialPortOptionRepository>().As<ISerialPortOptionRepository>()
                .WithParameters(new List<Parameter>
                {
                    new NamedParameter("connectionString", _connectionString),
                })
                .InstancePerLifetimeScope();

            builder.RegisterType<EfTcpIpOptionRepository>().As<ITcpIpOptionRepository>()
                .WithParameters(new List<Parameter>
                {
                    new NamedParameter("connectionString", _connectionString),
                })
                .InstancePerLifetimeScope();

            builder.RegisterType<EfHttpOptionRepository>().As<IHttpOptionRepository>()
                .WithParameters(new List<Parameter>
                {
                    new NamedParameter("connectionString", _connectionString),
                })
                .InstancePerLifetimeScope();

            builder.RegisterType<EfExchangeOptionRepository>().As<IExchangeOptionRepository>()
                .WithParameters(new List<Parameter>
                {
                    new NamedParameter("connectionString", _connectionString),
                })
                .InstancePerLifetimeScope();

            builder.RegisterType<EfDeviceOptionRepository>().As<IDeviceOptionRepository>()
                .WithParameters(new List<Parameter>
                {
                    new NamedParameter("connectionString", _connectionString),
                })
                .InstancePerLifetimeScope();

            builder.RegisterType<EfProduserUnionOptionRepository>().As<IProduserUnionOptionRepository>()
                .WithParameters(new List<Parameter>
                {
                    new NamedParameter("connectionString", _connectionString),
                })
                .InstancePerLifetimeScope();

            builder.RegisterType<EfStringInseartModelExtRepository>().As<IStringInsertModelExtRepository>()
                .WithParameters(new List<Parameter>
                {
                    new NamedParameter("connectionString", _connectionString),
                })
                .InstancePerLifetimeScope();


            builder.RegisterType<EfInlineStringInsertModelRepository>().As<IInlineStringInsertModelRepository>()
                .WithParameters(new List<Parameter>
                {
                    new NamedParameter("connectionString", _connectionString),
                })
                .InstancePerLifetimeScope();

        }
    }
}