using Common.Logging;
using GT.SiteCheck.BLL;
using GT.SiteCheck.BLL.impl;
using GT.SiteCheck.Entity.ViewEntity;
using Quartz;
using System;
using System.Collections.Generic;
using System.Reflection;
using Unity;

namespace GT.SiteCheck.Job
{
    public class GTSchedule
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static IScheduler GetSchedulers(IScheduler sched)
        {
            DateTimeOffset startTime = DateBuilder.NextGivenSecondDate(null, 10);

            UnityContainer container = new UnityContainer();
            container.RegisterType<ISiteBLL, SiteBLL>();

            //返回调用者
            ISiteBLL ISite = container.Resolve<SiteBLL>();

            List<JobSite> sites = ISite.getJobSites();

            _logger.InfoFormat("任务初始化共" + sites.Count + "个站点");

            foreach (JobSite s in sites)
            {
                IJobDetail job = JobBuilder.Create<GT.SiteCheck.Job.SiteCheckJob>().WithIdentity(s.site.name, s.node.nodeName).Build();

                ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                                                               .WithIdentity(s.site.name, s.node.nodeName)
                                                               .StartAt(startTime)
                                                               .WithSimpleSchedule(x => x.WithIntervalInSeconds(s.site.interval.Value).RepeatForever()) //.WithRepeatCount(2)
                                                               .Build();

                job.JobDataMap.Put(GT.SiteCheck.Job.SiteCheckJob.site, s.site);

                sched.ScheduleJob(job, trigger);
            }

            return sched;
        }
    }
}
