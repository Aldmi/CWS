﻿using App.Services.Config;
using Autofac;

namespace WebApiSwc.AutofacModules
{
    /// <summary>
    /// Регистрируем сервисы хранения бизнесс логики. (оперативные данные, хранятся в памяти (CuncurrentDictionary))
    /// </summary>
    public class BlConfigAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppConfigWrapper>().SingleInstance(); 
        }
    }
}