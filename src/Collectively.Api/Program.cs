using System.IO;
using Collectively.Common.Host;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Collectively.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost
                .Create<Startup>(args: args)
                .Build()
                .Run();
        }
    }
}