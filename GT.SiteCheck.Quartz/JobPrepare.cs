using GT.SiteCheck.Job;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GT.SiteCheck.Quartz
{
    public static class JobPrepare
    {
        private static IScheduler sched;

        public static void Run()
        {
            string InstanceName = ConfigurationManager.AppSettings["InstanceName"].ToString();
            string ThreadNum = ConfigurationManager.AppSettings["ThreadNum"].ToString();
            string tcpPort = ConfigurationManager.AppSettings["tcpPort"].ToString();

            var properties = new System.Collections.Specialized.NameValueCollection();
            properties["quartz.scheduler.instanceName"] = InstanceName;

            // 设置线程池
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = ThreadNum;
            properties["quartz.threadPool.threadPriority"] = "Normal";

            // 远程输出配置
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = tcpPort;
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            properties["quartz.scheduler.exporter.channelType"] = "tcp";

            ISchedulerFactory sf = new StdSchedulerFactory(properties);
            IScheduler sched = sf.GetScheduler();
            sched = GTSchedule.GetSchedulers(sched);
            sched.Start();
        }

        public static void Shut()
        {
            if (sched != null)
            {
                sched.Shutdown(false);
            }
        }
    }
}
