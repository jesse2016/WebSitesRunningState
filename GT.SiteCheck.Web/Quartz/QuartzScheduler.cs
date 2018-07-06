using Common.Logging;
using GT.SiteCheck.BLL;
using GT.SiteCheck.BLL.impl;
using GT.SiteCheck.Entity.ViewEntity;
using GT.SiteCheck.Job;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using Unity;

namespace GT.SiteCheck.Web.Quartz
{
    public class QuartzScheduler
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(QuartzScheduler));

        #region 运行JOB
        public static void Run()
        {
            try
            {
                UnityContainer container = new UnityContainer();
                container.RegisterType<ISiteBLL, SiteBLL>();

                //返回调用者
                ISiteBLL ISite = container.Resolve<SiteBLL>();

                List<JobSite> sites = ISite.getJobSites();

                RemoteScheduler sched = QuartzBase.GetRemoteScheduler();

                ILog log = LogManager.GetLogger(typeof(QuartzScheduler));

                DateTimeOffset startTime = DateBuilder.NextGivenSecondDate(null, 10);

                foreach (JobSite s in sites)
                {
                    IJobDetail job = JobBuilder.Create<SiteCheckJob>().WithIdentity(s.site.name, s.node.nodeName).Build();

                    ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                                                                   .WithIdentity(s.site.name, s.node.nodeName)
                                                                   .StartAt(startTime)
                                                                   .WithSimpleSchedule(x => x.WithIntervalInSeconds(s.site.interval.Value).RepeatForever()) //.WithRepeatCount(2)
                                                                   .Build();

                    job.JobDataMap.Put(SiteCheckJob.site, s.site);

                    sched.ScheduleJob(job, trigger);
                }

                sched.Start();
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("JOB启动异常：" + ex.ToString());
            }
        }
        #endregion
    }
}