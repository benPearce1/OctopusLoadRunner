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
                    await client.Repository.Dashboards.GetDashboard();
                }
            });
        }
    }
}