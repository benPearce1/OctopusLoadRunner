using System;
using Akka.Actor;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusLoadRunner.Messages;

namespace OctopusLoadRunner.Actors
{
    public class ProjectCreatorActor : ReceiveActor
    {
        public ProjectCreatorActor()
        {
            Receive<CreateNewProject>(async msg =>
            {
                var endpoint = new OctopusServerEndpoint(msg.Url, msg.ApiKey);
                using (var client = await OctopusAsyncClient.Create(endpoint))
                {
                    var projectGroup = await client.Repository.ProjectGroups.FindByName(msg.ProjectGroup);
                    var project = await client.Repository.Projects.Create(new ProjectResource
                    {
                        ProjectGroupId = projectGroup.Id,
                        Name = $"{msg.Name}{Guid.NewGuid()}",
                        LifecycleId = "Lifecycles-1"
                    });
                    
                    msg.DeploymentProcessActor.Tell(new CreateDeploymentProcess(project.Id, project.DeploymentProcessId, msg.Url, msg.ApiKey));
                    Console.WriteLine($"New project created {project.Name}");
                }
            });
        }
    }
}
