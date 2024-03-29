﻿using Autofac;
using Microsoft.AspNetCore.Identity;
using SimpleApp.Core.Interfaces.Services;
using SimpleApp.Core.Models.Entities;
using SimpleApp.Infrastructure.Services;

namespace SimpleApp.WebApi.Infrastructure.AutoFac.Modules
{
    public class IdentityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<PasswordHasher<User>>().As<IPasswordHasher<User>>();
            builder.RegisterType<AccountService>().As<IAccountService>();
        }
    }
}
