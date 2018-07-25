using System;
using System.Linq;
using System.Threading.Tasks;
using Akka.Actor;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusLoadRunner.Messages;

namespace OctopusLoadRunner.Actors.Deployments
{
    public class ProjectQueryActor : ReceiveActor
    {
        public ProjectQueryActor()
        {
            Receive<BaseApiMessage>(msg =>
            {
                var projectId = GetProjectId(msg);
                Sender.Tell(new ProjectResult(projectId));
            });
        }

        private string GetProjectId(BaseApiMessage msg)
        {
            ProjectResource project;
            var projectTask = GetProjectResource(msg);
            projectTask.Wait();
            project = projectTask.Result;

            return project.Id;
        }

        private static async Task<ProjectResource> GetProjectResource(BaseApiMessage msg)
        {
            ProjectResource project;
            var endpoint = new OctopusServerEndpoint(msg.Url, msg.ApiKey);
            using (var client = await OctopusAsyncClient.Create(endpoint))
            {
                var projects = await client.Repository.Projects.GetAll();
                project = projects.OrderBy(x => Guid.NewGuid()).First();
            }

            return project;
        }
    }
}