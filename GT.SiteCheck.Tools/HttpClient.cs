using GT.SiteCheck.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace QuartzDemo.GT.SiteCheck.Tools
{
    public class HttpClient
    {
        #region 执行http调用(POST/GET)
        public static string RequestPost(string url, string postData, string sessionId = "")
        {
            string response = string.Empty;
            StreamReader sr = null;
            HttpWebResponse wr = null;

            HttpWebRequest hp = null;
            try
            {
                hp = (HttpWebRequest)WebRequest.Create(url);

                hp.Timeout = 10 * 60 * 1000; //超时时间
                byte[] data = { };
                if (!string.IsNullOrWhiteSpace(postData))
                {
                    data = Encoding.UTF8.GetBytes(postData);
                    hp.Method = "POST";
                    hp.ContentLength = data.Length;
                }
                else
                {
                    hp.Method = "GET";
                }

                hp.ContentType = "text/json";
                if (sessionId != string.Empty)
                {
                    hp.Headers.Add("SessionId", sessionId);
                }

                if (!string.IsNullOrWhiteSpace(postData))
                {
                    Stream ws = hp.GetRequestStream();

                    // 发送数据
                    ws.Write(data, 0, data.Length);
                    ws.Close();
                }

                wr = (HttpWebResponse)hp.GetResponse();
                sr = new StreamReader(wr.GetResponseStream(), Encoding.UTF8);

                response = sr.ReadToEnd();
                sr.Close();
                wr.Close();
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }

            return response;
        }
        #endregion

        #region 检查WebService
        public static RetMsg CheckWebService(string url)
        {
            RetMsg ret = new RetMsg();
            try
            {
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                using (HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse())
                {
                    ret.Msg = "";
                    ret.Result = true;
                }
            }
            catch (WebException e)
            {
                string msg = string.Empty;
                if (e.Status == WebExceptionStatus.ProtocolError)
                {
                    msg = string.Format("Status Code : {0}", ((HttpWebResponse)e.Response).StatusCode);
                    msg += ";" + string.Format("Status Description : {0}",
                               ((HttpWebResponse)e.Response).StatusDescription);
                }
                else
                {
                    msg = e.Message;
                }
                ret.Result = false;
                ret.Msg = msg;
            }
            return ret;
        }
        #endregion
    }
}