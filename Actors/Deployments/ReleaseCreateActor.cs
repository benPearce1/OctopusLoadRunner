using System;
using Akka.Actor;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusLoadRunner.Messages;

namespace OctopusLoadRunner.Actors.Deployments
{
    public class ReleaseCreateActor : ReceiveActor
    {
        public ReleaseCreateActor()
        {
            Receive<CreateRelease>(async msg =>
            {
                var endpoint = new OctopusServerEndpoint(msg.Url, msg.ApiKey);
                using (var client = await OctopusAsyncClient.Create(endpoint))
                {
                    var now = DateTime.Now;
                    var version =
                        $"{now.Year}.{now.Month}.{now.Day}.{now.Hour * 10000 + now.Minute * 100 + now.Second}";
                    var release =
                        await client.Repository.Releases.Create(new ReleaseResource {ProjectId = msg.ProjectId, Version = version});
                    var deployment = client.Repository.Deployments.Create(new DeploymentResource
                    {
                        ProjectId = msg.ProjectId,
                        EnvironmentId = msg.EnvironmentId
                    });

                    Console.WriteLine($"Created deployment for {msg.ProjectId} to {msg.EnvironmentId}");
                }
            });
        }
    }
}