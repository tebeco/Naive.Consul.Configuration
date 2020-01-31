using System;
using Microsoft.Extensions.Configuration;

namespace ConsulSample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var consulSettings = new ConfigurationBuilder().AddJsonFile("settings.json").Build();
            var consulAgentUri = new Uri(consulSettings["ConsulAgentUri"]);

            var consulConfiguration = new ConfigurationBuilder().AddConsulKeyValue(consulAgentUri).Build();

            foreach (var child in consulConfiguration.GetChildren())
            {
                Console.WriteLine($"Key :'{child.Key}'\nPath :'{child.Path}'\nValue :'{child.Value}'\n");
            }

#if DEBUG
            Console.ReadLine();
#endif
        }
    }
}

