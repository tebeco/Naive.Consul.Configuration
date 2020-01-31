using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsulDevAgentRunner
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += Console_CancelKeyPressed;

            var consulAgentRunner = new ConsulAgentRunner();
            consulAgentRunner.Start(cts);
            consulAgentRunner.PutRandomKeys();

            await cts.Token.WhenCanceled();

            void Console_CancelKeyPressed(object s, ConsoleCancelEventArgs e)
            {
                cts.Cancel();
                e.Cancel = true;
            }
        }

        public static Task WhenCanceled(this CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            return tcs.Task;
        }
    }
}
