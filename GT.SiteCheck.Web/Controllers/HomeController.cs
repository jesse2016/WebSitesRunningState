using GT.SiteCheck.BLL;
using GT.SiteCheck.BLL.impl;
using GT.SiteCheck.Entity.Entities;
using GT.SiteCheck.Entity.ViewEntity;
using GT.SiteCheck.Tools;
using GT.SiteCheck.Web.Quartz;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Unity;

namespace GT.SiteCheck.Web.Controllers
{
    public class HomeController : BaseController
    {
        private UnityContainer container = new UnityContainer();
        private ISiteBLL ISite;
        private INodeBLL INode;

        public HomeController()
        {
            container.RegisterType<ISiteBLL, SiteBLL>();
            container.RegisterType<INodeBLL, NodeBLL>();

            //返回调用者
            ISite = container.Resolve<SiteBLL>(); 
            INode = container.Resolve<NodeBLL>(); 
        }

        public ActionResult Index()
        {
            ViewData["userName"] = userName;
            return View();
        }

        public ActionResult list()
        {
            return View();
        }

        public ActionResult modify()
        {
            return View();
        }

        #region 加载左侧目录树
        public JsonResult LoadTree()
        {
            List<TreeNode> nodeList = INode.GetNodeByNodeId(1).ToList();
            List<zTree> treeList = Util.GetJsonTreeData(nodeList, 1);

            string jsonstr = JsonConvert.SerializeObject(treeList);
            return Json(new { Data = jsonstr }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 加载服务器列表
        public JsonResult LoadServerByPid(string pId)
        {
            List<Site> siteList = ISite.getSiteByPid(int.Parse(pId));
            string nodeName = INode.GetNodeById(int.Parse(pId)).nodeName;

            string jsonstr = JsonConvert.SerializeObject(siteList);
            return Json(new { Data = jsonstr, Name = nodeName }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 根据父id查询服务器列表
        public JsonResult GetServerListByPId(string parentId, string pageSize, string currentPage)
        {
            int _parentId = int.Parse(parentId);
            int _articleCount = ISite.getSiteCount(_parentId);

            int _pageCount = 0;
            int _pageSize = int.Parse(pageSize);
            int _currentPage = int.Parse(currentPage);
            int _pageIndex = _currentPage - 1;         

            _pageCount = _articleCount / _pageSize;
            if (_articleCount % _pageSize > 0)
            {
                _pageCount += 1;
            }

            if (_pageCount < _currentPage && _pageCount > 0)
            {
                _pageIndex = _pageCount - 1;
            }
            List<Site> nodeList = ISite.getSiteByPage(_pageSize, _pageIndex, _parentId);
            string jsonstr = JsonConvert.SerializeObject(nodeList);

            return Json(new { ArticleCount = _articleCount, PageCount = _pageCount, CurrentPage = _pageIndex + 1, Data = jsonstr }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 添加服务器
        [HttpPost]
        public JsonResult Add()
        {
            RetMsg ret = new RetMsg();
            try
            {
                int _pId = int.Parse(Request["pId"]);
                string _name = Request["name"];
                string _desc = Request["desc"];
                int _type = int.Parse(Request["type"]);
                string _url = Request["url"];
                string _paras = Request["paras"];
                if (!string.IsNullOrWhiteSpace(_paras))
                {
                    _paras = HttpUtility.UrlDecode(_paras);
                }
                string _mails = Request["mails"];
                int _interval = int.Parse(Request["interval"]);
                int _isuse = int.Parse(Request["isuse"]);

                Site site = new Site();

                site.pId = _pId;
                site.name = _name;
                site.description = _desc;
                site.type = _type;
                site.url = _url;
                site.paras = _paras;
                site.mailgroup = _mails;
                site.interval = _interval;
                site.isuse = _isuse;
                site.createPerson = userName;
                site.createDate = DateTime.Now;
                site.lastUpdatePerson = userName;
                site.lastUpdateDate = DateTime.Now;

                ret = ISite.Add(site);
                InitQuartzJob();
            }
            catch (Exception e)
            {
                ret.Result = false;
                ret.Msg = e.Message;
            }
            return Json(new { Result = ret.Result, Msg = ret.Msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 根据id查询服务器
        public JsonResult getServerById(string Id)
        {
            Site site = ISite.getSiteById(int.Parse(Id));
            string jsonstr = JsonConvert.SerializeObject(site);
            return Json(new { Data = jsonstr }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 更新服务器
        [HttpPost]
        public JsonResult Update()
        {
            RetMsg ret = new RetMsg();
            try
            {
                int _Id = int.Parse(Request["Id"]);
                int _pId = int.Parse(Request["pId"]);
                string _name = Request["name"];
                string _desc = Request["desc"];
                int _type = int.Parse(Request["type"]);
                string _url = Request["url"];
                string _paras = Request["paras"];
                if (!string.IsNullOrWhiteSpace(_paras))
                {
                    _paras = HttpUtility.UrlDecode(_paras);
                }
                string _mails = Request["mails"];
                int _interval = int.Parse(Request["interval"]);
                int _isuse = int.Parse(Request["isuse"]);

                Site site = new Site();

                site.id = _Id;
                site.pId = _pId;
                site.name = _name;
                site.description = _desc;
                site.type = _type;
                site.url = _url;
                site.paras = _paras;
                site.mailgroup = _mails;
                site.interval = _interval;
                site.isuse = _isuse;
                site.lastUpdatePerson = userName;
                site.lastUpdateDate = DateTime.Now;

                ret = ISite.Update(site);
                InitQuartzJob();
            }
            catch (Exception e)
            {
                ret.Result = false;
                ret.Msg = e.Message;
            }
            return Json(new { Result = ret.Result, Msg = ret.Msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 更新服务器是否可用标识
        [HttpPost]
        public JsonResult UpdateUseFlag()
        {
            RetMsg ret = new RetMsg();
            try
            {
                int flag = int.Parse(Request["flag"]);
                string idstr = Request["idstr"];
                ret = ISite.updateSiteUseFlag(idstr, flag);
                InitQuartzJob();
            }
            catch (Exception e)
            {
                ret.Result = false;
                ret.Msg = e.Message;
            }
            return Json(new { Result = ret.Result, Msg = ret.Msg }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 初始化全部JOB
        private void InitQuartzJob()
        {
            Thread t = new Thread(RestartJob);
            t.Start();
        }

        private void RestartJob()
        {
            try
            {
                IScheduler sched = QuartzBase.GetRemoteScheduler();
                if (!sched.IsShutdown)
                {
                    sched.Clear();
                }
                QuartzScheduler.Run();
            }
            catch (Exception)
            {
            }
        }
        #endregion 
    }
}