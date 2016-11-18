using Autofac;
using Coolector.Api.Authentication;

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