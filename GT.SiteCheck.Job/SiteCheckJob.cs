using GT.SiteCheck.BLL;
using GT.SiteCheck.BLL.impl;
using GT.SiteCheck.Entity.Entities;
using GT.SiteCheck.Entity.ViewEntity;
using log4net;
using Quartz;
using QuartzDemo.GT.SiteCheck.Tools;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using Unity;

namespace GT.SiteCheck.Job
{
    public class SiteCheckJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private UnityContainer container = new UnityContainer();
        private ISiteStatusBLL IStatus;
        private ISiteBLL ISite;

        public const string site = "site";

        public SiteCheckJob()
        {
            //注册依赖对象
            container.RegisterType<ISiteStatusBLL, SiteStatusBLL>();
            container.RegisterType<ISiteBLL, SiteBLL>();

            IStatus = container.Resolve<SiteStatusBLL>();
            ISite = container.Resolve<SiteBLL>();
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                JobKey jobKey = context.JobDetail.Key;
                JobDataMap data = context.JobDetail.JobDataMap;

                //_logger.InfoFormat("站点检查Job [" + jobKey + "] 开始运行");

                Site s = (Site)data.Get(site);

                CheckUrl(s);
            }
            catch (Exception e)
            {
                _logger.ErrorFormat(e.ToString());
            }
        }

        private void CheckUrl(Site s)
        {
            string res = "";
            if (s.type == 1)
            {
                res = HttpClient.RequestPost(s.url, s.paras);
            }
            if (s.type == 2)
            {
                RetMsg ret = HttpClient.CheckWebService(s.url);
                res = ret.Msg;
            }

            SiteStatus status = new SiteStatus();

            status.siteId = s.id;
            status.paras = res;

            //_logger.InfoFormat("[id: " + s.id + "][name: " + s.name + "]: " + res);

            if (s.type == 1)
            {
                try
                {
                    HttpResponse<DataStatus> data = DataJsonSerializer<HttpResponse<DataStatus>>.JsonToEntity(res);

                    status.status = data.StatusCode;
                    status.errorMsg = data.ErrorMsg;
                }
                catch (Exception e)
                {
                    _logger.ErrorFormat(e.Message);
                    status.status = "500";
                    status.errorMsg = e.Message;
                }
            }
            if (s.type == 2)
            {
                if (res != "")
                {
                    status.status = "500";
                    status.errorMsg = res;
                }
                else
                {
                    status.status = "200";
                    status.errorMsg = "";
                }
            }

            status.createDate = DateTime.Now;

            //将错误信息插入数据库
            if (status.status != "200")
            {
                IStatus.Add(status);
            }

            //更新状态
            ISite.updateSiteStatus(s.id, status.status);

            //异常时发邮件
            if (status.status != "200" && s.mailgroup.Trim() != "")
            {
                string[] mailAry = s.mailgroup.Split(',');
                if (mailAry.Length > 0)
                {
                    string body = "异常信息：" + Environment.NewLine + res;
                    foreach (string mail in mailAry)
                    {
                        SendEmail("服务器(" + s.name + ")出现异常，请尽快处理！", body, mail.Trim());
                    }
                }
            }
        }

        #region 发邮件
        public void SendEmail(string title, string body, string mailTo)
        {
            string Smtp = ConfigurationManager.AppSettings["Smtp"].ToString();
            string Port = ConfigurationManager.AppSettings["Port"].ToString();
            string MailFrom = ConfigurationManager.AppSettings["MailFrom"].ToString();
            string MailPwd = ConfigurationManager.AppSettings["MailPwd"].ToString();

            System.Web.Mail.MailMessage mail = new System.Web.Mail.MailMessage();
            try
            {
                mail.From = MailFrom;
                mail.To = mailTo;
                mail.Subject = title;
                mail.BodyFormat = System.Web.Mail.MailFormat.Html;
                mail.Body = body;

                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1"); //basic authentication
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", MailFrom); //set your username here
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", MailPwd); //set your password here
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", Port);//set port
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");//set is ssl
                System.Web.Mail.SmtpMail.SmtpServer = Smtp;
                System.Web.Mail.SmtpMail.Send(mail);
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("发送邮件异常：" + ex.Message);
            }
        }
        #endregion
    }
}
