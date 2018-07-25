using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;
using OctopusLoadRunner.Messages;

namespace OctopusLoadRunner.Actors
{
    public class ProjectCreateCoordinator : ReceiveActor
    {
        List<Tuple<IActorRef, IActorRef>> actors = new List<Tuple<IActorRef, IActorRef>>();
        public ProjectCreateCoordinator()
        {
            Receive<StartMessage>(msg =>
            {
                for (int i = 0; i < msg.NumberOfUsers; i++)
                {
                    IActorRef deploymentProcessActor = Context.ActorOf<DeploymentProcessActor>();
                    IActorRef actor = Context.ActorOf<ProjectCreatorActor>();
                    
                    actors.Add(new Tuple<IActorRef, IActorRef>(actor, deploymentProcessActor));
                    
                    Context.System.Scheduler.ScheduleTellRepeatedly(TimeSpan.FromSeconds(i * 5), TimeSpan.FromSeconds(10), actor,
                        new CreateNewProject($"Project-", "Default Project Group", deploymentProcessActor, msg.Url,
                            msg.ApiKey), Self);
                }
            });
        }
    }
}
