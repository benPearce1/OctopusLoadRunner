using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Akka.Actor;
using OctopusLoadRunner.Messages;

namespace OctopusLoadRunner.Actors.Dashboard
{
    public class DashboardRefreshCoordinator : ReceiveActor
    {
        public DashboardRefreshCoordinator()
        {
            List<IActorRef> actors = new List<IActorRef>();
            Receive<StartMessage>(msg =>
            {
                for (int i = 0; i < msg.NumberOfUsers; i++)
                {
                    var loopActor = Context.ActorOf<DashboardRefreshLoopActor>();
                    actors.Add(loopActor);
                    Thread.Sleep(100);
                    loopActor.Tell(new StartDashboard(TimeSpan.FromSeconds(2), msg.Url, msg.ApiKey));
                }
            });

            //Receive<Stop>(msg =>
            //{
            //    Console.WriteLine($"Stopping {actors.Count} dashboard actors");
            //    foreach (var actorRef in actors)
            //    {
            //        Context.Stop(actorRef);
            //    }
            //});
        }
    }
}
