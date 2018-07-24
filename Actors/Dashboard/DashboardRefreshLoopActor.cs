using System;
using System.Threading;
using Akka.Actor;
using OctopusLoadRunner.Messages;

namespace OctopusLoadRunner.Actors.Dashboard
{
    public class DashboardRefreshLoopActor : ReceiveActor
    {
        private bool running;
        private string id;

        public DashboardRefreshLoopActor()
        {
            Receive<StartDashboard>(msg =>
            {
                id = Guid.NewGuid().ToString();
                running = true;
                while (running)
                {
                    Thread.Sleep(msg.RefreshTime);
                    var actor = Context.ActorOf<DashboardActor>();
                    Console.WriteLine($"{DateTime.Now.ToLongTimeString()} {id} receiving dashboard");
                    actor.Tell(new BaseApiMessage(msg.Url, msg.ApiKey));
                }
            });

            Receive<Stop>(msg =>
            {
                Context.Stop(Self);
            });
        }
    }
}