using System;
using System.Collections.Generic;
using System.Text;
using Akka.Actor;

namespace OctopusLoadRunner.Messages
{
    public class BaseApiMessage
    {
        public BaseApiMessage(string url, string apikey)
        {
            Url = url;
            ApiKey = apikey;
        }
        public string Url { get; private set; }

        public string ApiKey { get; private set; }
    }

    public class StartMessage : BaseApiMessage
    {
        public StartMessage(int numberOfUsers, string url, string apikey) : base(url, apikey)
        {
            NumberOfUsers = numberOfUsers;
        }

        public int NumberOfUsers { get; private set; }
    }

    public class StartDashboard : BaseApiMessage
    {
        public StartDashboard(TimeSpan refreshTime, string url, string apikey) : base(url, apikey)
        {
            RefreshTime = refreshTime;
        }
        public TimeSpan RefreshTime { get; private set; }
    }

    public class Stop
    {

    }

    public class CreateNewProject : BaseApiMessage
    {
        public string Name { get; }
        public string ProjectGroup { get; }
        public IActorRef DeploymentProcessActor { get; }

        public CreateNewProject(string name, string projectGroup, IActorRef deploymentProcessActor, string url, string apikey) : base(url,apikey)
        {
            Name = name;
            ProjectGroup = projectGroup;
            DeploymentProcessActor = deploymentProcessActor;
        }
    }

    public class CreateDeploymentProcess : BaseApiMessage
    {
        public string ProjectId { get; }
        public string DeploymentProcessId { get; }

        public CreateDeploymentProcess(string projectId, string deploymentProcessId, string url, string apikey) : base(url, apikey)
        {
            ProjectId = projectId;
            DeploymentProcessId = deploymentProcessId;
        }
    }

    public class CreateRelease : BaseApiMessage
    {
        public string ProjectId { get; }
        public string EnvironmentId { get; }

        public CreateRelease(string projectId, string environmentId, string url, string apikey) : base(url, apikey)
        {
            ProjectId = projectId;
            EnvironmentId = environmentId;
        }
    }

    public class ProjectResult
    {
        public string ProjectId { get; }

        public ProjectResult(string projectId)
        {
            ProjectId = projectId;
        }
    }
}
