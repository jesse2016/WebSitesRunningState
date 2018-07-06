using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GT.SiteCheck.Web.Quartz
{
    public class JobDetail
    {
        public string jobName { set; get; }

        public string jobGroup { set; get; }

        public TriggerState trigerState { set; get; }

        public double timeDiff { set; get; }

        public DateTimeOffset startTime { set; get; }

        public DateTimeOffset? preTime { set; get; }

        public DateTimeOffset? nextTime { set; get; }
    }
}