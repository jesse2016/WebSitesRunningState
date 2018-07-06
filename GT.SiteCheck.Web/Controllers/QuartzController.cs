using GT.SiteCheck.Web.Quartz;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace GT.SiteCheck.Web.Controllers
{
    public class QuartzController : BaseController
    {
        RemoteScheduler sched = null;

        public QuartzController()
        {
            sched = QuartzBase.GetRemoteScheduler();
        }

        // GET: Quartz
        public ActionResult Index()
        {
            return View();
        }

        public string HandleScheduler(string type)
        {
            string msg = "";
            try
            {               
                if (type == "Run")
                {
                    if (sched != null)
                    {
                        if (!sched.IsShutdown)
                        {
                            sched.Clear();
                        }
                    }
                    QuartzScheduler.Run();
                }
                if (type == "Pause")
                {
                    if (sched != null)
                    {
                        if (!sched.IsShutdown)
                        {
                            sched.PauseAll();
                        }
                    }
                }
                if (type == "Resume")
                {
                    if (sched != null)
                    {
                        if (!sched.IsShutdown)
                        {
                            sched.ResumeAll();
                        }
                    }
                }
                if (type == "Shutdown")
                {
                    if (sched != null)
                    {
                        if (!sched.IsShutdown)
                        {
                            sched.Shutdown(false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return msg;
        }

        public JsonResult GetSchedulerInfo()
        {
            string json = string.Empty;
            bool ret = false;
            try
            {
                SchedulerMetaData data = sched.GetMetaData();

                json = JsonConvert.SerializeObject(data);
                ret = true;
            }
            catch (Exception ex)
            {
                json = ex.Message;
            }
            return Json(new { Result = ret, Data = json }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllJobs()
        {
            string msg = "";
            List<JobDetail> jobList = new List<JobDetail>();
            try
            {
                foreach(String groupName in sched.GetJobGroupNames())
                {
                    GroupMatcher<JobKey> matcher = GroupMatcher<JobKey>.GroupEquals(groupName);

                    foreach (JobKey jobKey in sched.GetJobKeys(matcher))
                    {
                        JobDetail job = new JobDetail();

                        String jobName = jobKey.Name;
                        String jobGroup = jobKey.Group;

                        IJobDetail detail = sched.GetJobDetail(jobKey);

                        //get job's trigger
                        IList<ITrigger> triggers = (IList<ITrigger>)sched.GetTriggersOfJob(jobKey);
                        foreach (ITrigger trigger in triggers)
                        {
                            DateTimeOffset startTime = trigger.StartTimeUtc;
                            DateTimeOffset? preTime = trigger.GetPreviousFireTimeUtc();
                            DateTimeOffset? nextTime = trigger.GetNextFireTimeUtc();

                            job.startTime = startTime;
                            job.preTime = preTime;
                            job.nextTime = nextTime;

                            if (job.nextTime.HasValue && job.preTime.HasValue)
                            {
                                job.timeDiff = job.nextTime.Value.Subtract(job.preTime.Value).TotalSeconds;
                            }

                            job.trigerState = sched.GetTriggerState(trigger.Key);
                        }

                        job.jobName = jobName;
                        job.jobGroup = jobGroup;

                        jobList.Add(job);
                    }
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            string jsonstr = JsonConvert.SerializeObject(jobList);
            return Json(new { Msg = msg, Data = jsonstr }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult HandleJob(string name, string group, string type)
        {
            string msg = "";
            try
            {
                JobKey key = null;
                foreach (String groupName in sched.GetJobGroupNames())
                {
                    GroupMatcher<JobKey> matcher = GroupMatcher<JobKey>.GroupEquals(groupName);

                    foreach (JobKey jobKey in sched.GetJobKeys(matcher))
                    {
                        if (name == jobKey.Name && group == jobKey.Group)
                        {
                            key = jobKey;
                            break;
                        }
                    }
                    if (key != null)
                    {
                        break;
                    }
                }
                if (type == "pause")
                {
                    sched.PauseJob(key);
                }
                if (type == "start")
                {
                    sched.ResumeJob(key);
                }
                if (type == "delete")
                {
                    sched.DeleteJob(key);
                }
                if (type == "Interrupt")
                {
                    sched.Interrupt(key);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Json(new { Msg = msg }, JsonRequestBehavior.AllowGet);
        }
    }
}