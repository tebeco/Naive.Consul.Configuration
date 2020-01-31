using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ConsulDevAgentRunner
{
    public class ConsulAgentRunner
    {
        private readonly string _consulPath;

        public ConsulAgentRunner(string consulPath = null)
        {
            _consulPath = consulPath ?? "consul.exe"; //null if consul in the PATH
        }

        private Process GetConsulProcess(string arguments)
        {
            var consulProcess = new Process();
            consulProcess.StartInfo = GetConsulStartInfo(arguments);

            return consulProcess;
        }

        private ProcessStartInfo GetConsulStartInfo(string arguments)
        {
            return new ProcessStartInfo
            {
                FileName = _consulPath,
                Arguments = arguments
            };
        }

        public void Start(CancellationTokenSource cts)
        {
            var consulProcess = GetConsulProcess("agent --dev");
            cts.Token.Register(() => consulProcess.Close());

            consulProcess.Start();
        }

        public void PutRandomKeys()
        {
            for(int i = 0; i < 10; i++)
            {
                PutKeyValue($"Key_{i}", $"Value_{i}");
                PutKeyValue($"SomeSubFolder/Sub_Key_{i}", $"Sub_Value_{i}");
            }
        }

        public void PutKeyValue(string key, string value)
        {
            var consulProcess = GetConsulProcess($"kv put {key} {value}");
            consulProcess.Start();
        }
    }
}