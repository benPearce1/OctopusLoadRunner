using System;
using System.Diagnostics;
using Akka.Actor;
using Octopus.Client;
using OctopusLoadRunner.Messages;

namespace OctopusLoadRunner.Actors.Dashboard
{
    public class DashboardActor : ReceiveActor
    {
        public DashboardActor()
        {
            Receive<BaseApiMessage>(async msg =>
            {
                var endpoint = new OctopusServerEndpoint(msg.Url, msg.ApiKey);
                using (var client = await OctopusAsyncClient.Create(endpoint))
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    var dashboard = await client.Repository.Dashboards.GetDashboard();
                    sw.Stop();
                    Console.WriteLine($"Dashboard with {dashboard.Items.Count} items, {dashboard.Projects.Count} projects, received in {sw.ElapsedMilliseconds} ms");
                }
            });
        }
    }
}