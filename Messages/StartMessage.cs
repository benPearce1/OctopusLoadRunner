using System;
using System.Collections.Generic;
using System.Text;

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
}
