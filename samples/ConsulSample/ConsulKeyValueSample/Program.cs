using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsulSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            var consulSettings = builder
                                    .AddJsonFile("settings.json")
                                    .Build();

            var consulAgentUri = new Uri(consulSettings["ConsulAgentUri"]);

            var consulConfiguration = builder
                                        .AddConsulKeyValue(consulAgentUri)
                                        .Build();

            foreach (var child in consulConfiguration.GetChildren())
            {
                Console.WriteLine($"Key :'{child.Key}'\nPath :'{child.Path}'\nValue :'{child.Value}'\n");
            }
        }
    }
}

