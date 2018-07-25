using System;
using System.Threading.Tasks;

namespace OctopusLoadRunner
{
    class Program
    {
        
        static void Main(string[] args)
        {
            AsyncMain().Wait();
        }

        private static async Task AsyncMain()
        {
            using (var runner = new LoadRunner())
            {
                runner.Start();
                Console.CancelKeyPress += (sender, eventArgs) =>
                {
                    Console.WriteLine("Cancel detected");
                    runner.Stop();
                    
                };
                await runner.WhenTerminated;
            }
        }
    }
}
