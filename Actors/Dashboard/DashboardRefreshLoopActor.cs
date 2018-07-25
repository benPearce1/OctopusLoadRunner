using System;
using System.Threading;
using Akka.Actor;
using OctopusLoadRunner.Messages;

namespace OctopusLoadRunner.Actors.Dashboard
{
    public class DashboardRefreshLoopActor : ReceiveActor
    {
        private IActorRef dashboardActor;


        public DashboardRefreshLoopActor()
        {
            Receive<StartDashboard>(msg =>
            {
                //id = Guid.NewGuid().ToString();
                dashboardActor = Context.ActorOf<DashboardActor>();

                Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.Zero, msg.RefreshTime, dashboardActor,
                    new BaseApiMessage(msg.Url, msg.ApiKey), Context.Self);
            });

            Receive<Stop>(msg =>
            {
                Context.Stop(Self);
            });
        }
    }
}