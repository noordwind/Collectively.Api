using System;
using System.IO;
using System.Linq;
using Autofac;
using Coolector.Common.Commands;
using Coolector.Common.Events;
using Coolector.Services.Extensions;
using Microsoft.AspNetCore.Hosting;
using RawRabbit;

namespace Coolector.Services.Host
{
    public class WebServiceHost : IWebServiceHost
    {
        private readonly IWebHost _webHost;

        public WebServiceHost(IWebHost webHost)
        {
            _webHost = webHost;
        }

        public void Run()
        {
            _webHost.Run();
        }

        public static Builder Create<TStartup>(string name = "", int port = 80) where TStartup : class
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = $"Warden Service: {typeof(TStartup).Namespace.Split('.').Last()}";
            }            

            Console.Title = name;
            var webHost = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseStartup<TStartup>()
                .UseUrls($"http://*:{port}")
                .Build();
            var builder = new Builder(webHost);

            return builder;
        }

        public abstract class BuilderBase
        {
            public abstract WebServiceHost Build();
        }

        public class Builder : BuilderBase
        {
            private IResolver _resolver;
            private IBusClient _bus;
            private readonly IWebHost _webHost;

            public Builder(IWebHost webHost)
            {
                _webHost = webHost;
            }

            public Builder UseAutofac(ILifetimeScope scope)
            {
                _resolver = new AutofacResolver(scope);

                return this;
            }

            public BusBuilder UseRabbitMq()
            {
                _bus = _resolver.Resolve<IBusClient>();

                return new BusBuilder(_webHost, _bus, _resolver);
            }

            public override WebServiceHost Build()
            {
                return new WebServiceHost(_webHost);
            }
        }

        public class BusBuilder : BuilderBase
        {
            private readonly IWebHost _webHost;
            private readonly IBusClient _bus;
            private readonly IResolver _resolver;

            public BusBuilder(IWebHost webHost, IBusClient bus, IResolver resolver)
            {
                _webHost = webHost;
                _bus = bus;
                _resolver = resolver;
            }

            public BusBuilder SubscribeToCommand<TCommand>() where TCommand : ICommand
            {
                var commandHandler = _resolver.Resolve<ICommandHandler<TCommand>>();
                _bus.WithCommandHandlerAsync(commandHandler);

                return this;
            }

            public BusBuilder SubscribeToEvent<TEvent>() where TEvent : IEvent
            {
                var eventHandler = _resolver.Resolve<IEventHandler<TEvent>>();
                _bus.WithEventHandlerAsync(eventHandler);

                return this;
            }

            public override WebServiceHost Build()
            {
                return new WebServiceHost(_webHost);
            }
        }
    }
}