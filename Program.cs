using System;

namespace OctopusLoadRunner
{
    class Program
    {
        
        static void Main(string[] args)
        {
            LoadRunner runner =new LoadRunner();
            runner.Start();
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                Console.WriteLine("Cancel detected");
                runner.Stop();
            };

            runner.WhenTerminated.Wait();
        }
    }
}
