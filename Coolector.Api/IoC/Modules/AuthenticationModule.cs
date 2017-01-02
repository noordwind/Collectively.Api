using Autofac;
using Coolector.Common.Security.Authentication;

namespace Coolector.Api.IoC.Modules
{
    public class AuthenticationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JwtTokenHandler>()
                .As<IJwtTokenHandler>()
                .SingleInstance();
        }
    }
}