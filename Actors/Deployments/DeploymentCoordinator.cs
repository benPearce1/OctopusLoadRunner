using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using OctopusLoadRunner.Messages;

namespace OctopusLoadRunner.Actors.Deployments
{
    public class DeploymentCoordinator : ReceiveActor
    {
        List<IActorRef> actors = new List<IActorRef>();
        private string url;
        private string apikey;
        public DeploymentCoordinator()
        {
            Receive<StartMessage>(msg =>
            {
                for (int i = 0; i < msg.NumberOfUsers; i++)
                {
                    url = msg.Url;
                    apikey = msg.ApiKey;
                    var projectQueryActor = Context.ActorOf<ProjectQueryActor>();
                    projectQueryActor.Tell(new BaseApiMessage(msg.Url, msg.ApiKey));
                    
                }
            });

            Receive<ProjectResult>(msg =>
            {
                var releaseCreateActor = Context.ActorOf<ReleaseCreateActor>();
                Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.Zero, TimeSpan.FromSeconds(60),
                    releaseCreateActor, new CreateRelease(msg.ProjectId, "Environments-1", url, apikey), Self);
            });
        }
    }
}
