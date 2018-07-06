using Common.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace GT.SiteCheck.Web.Quartz
{
    public class QuartzBase
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(QuartzBase));
        private static string InstanceName = ConfigurationManager.AppSettings["InstanceName"].ToString();
        private static string tcpPort = ConfigurationManager.AppSettings["tcpPort"].ToString();

        #region 获取JOB实例
        public static RemoteScheduler GetRemoteScheduler()
        {
            RemoteScheduler scheduler = null;
            try
            {
                RemotingSchedulerProxyFactory rcpf = new RemotingSchedulerProxyFactory();
                rcpf.Address = "tcp://localhost:"+ tcpPort + "/QuartzScheduler";
                scheduler = new RemoteScheduler(InstanceName, rcpf);
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("远程获取JOB实例异常：" + ex.InnerException);
            }
            return scheduler;
        }
        #endregion
    }
}