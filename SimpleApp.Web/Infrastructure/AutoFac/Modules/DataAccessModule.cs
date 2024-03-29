﻿using Autofac;
using SimpleApp.Infrastructure.Repositories;

namespace SimpleApp.Web.Infrastructure.AutoFac.Modules
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterAssemblyTypes(typeof(Repository<>).Assembly)
                .AsClosedTypesOf(typeof(Repository<>))
                .AsImplementedInterfaces();
        }
    }
}
