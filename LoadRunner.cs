using System;
using System.IO;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Bootstrap.Docker;
using Akka.Configuration;
using OctopusLoadRunner.Actors;
using OctopusLoadRunner.Actors.Dashboard;
using OctopusLoadRunner.Actors.Deployments;
using OctopusLoadRunner.Messages;

namespace OctopusLoadRunner
{
    public class LoadRunner : IDisposable
    {
        private ActorSystem LoadSystem;
        IActorRef dashboardCoordinator;
        private IActorRef projectCoordinator;
        private IActorRef deploymentCoordinator;

        public bool Start()
        {
            var config = ConfigurationFactory.ParseString(@"akka {
            # here we are configuring log levels
            log-config-on-start = off
            stdout-loglevel = INFO
            loglevel = ERROR
            # this config section will be referenced as akka.actor
            actor {
              provider = remote
              debug {
                  receive = on
                  autoreceive = on
                  lifecycle = on
                  event-stream = on
                  unhandled = on
              }
            }");
            LoadSystem = ActorSystem.Create("loadrunner");
            
            dashboardCoordinator = LoadSystem.ActorOf<DashboardRefreshCoordinator>();
            var apikey = "API-FEL3J6OH4NQLYJTXRJCKGMBAS";
            var url = "http://localhost:8065";
            dashboardCoordinator.Tell(new StartMessage(10, url, apikey));

            projectCoordinator = LoadSystem.ActorOf<ProjectCreateCoordinator>();
            projectCoordinator.Tell(new StartMessage(1, url, apikey));

            deploymentCoordinator = LoadSystem.ActorOf<DeploymentCoordinator>();
            deploymentCoordinator.Tell(new StartMessage(10, url, apikey));
            return true;
        }

        public Task Stop()
        {
            dashboardCoordinator.Tell(new Stop());
           
            //LoadSystem.ActorSelection("/user/*").Tell(PoisonPill.Instance);
            //return dashboardCoordinator.GracefulStop(TimeSpan.FromSeconds(2));
            //dashboardCoordinator.Tell(PoisonPill.Instance);
            return CoordinatedShutdown.Get(LoadSystem).Run();
        }

        public Task WhenTerminated => LoadSystem.WhenTerminated;

        public void Dispose()
        {
            LoadSystem?.Dispose();
        }
    }
}
