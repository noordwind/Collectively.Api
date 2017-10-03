using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Collectively.Api.Framework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nancy.Owin;
using Lockbox.Client.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Collectively.Common.Logging;
using Collectively.Common.Caching;

namespace Collectively.Api
{
    public class Startup
    {
        public string EnvironmentName { get; set; }
        public IConfiguration Configuration { get; set; }
        public IServiceCollection Services { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSerilog(Configuration);
            services.AddWebEncoders();
            services.AddCors();
            services.AddMemoryCache();
            Services = services;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseSerilog(loggerFactory);
            app.UseCors(builder => builder.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials());
            app.UseOwin().UseNancy(x => x.Bootstrapper = new Bootstrapper(Configuration, Services));
        }
    }
}