using GT.SiteCheck.DAL;
using GT.SiteCheck.DAL.impl;
using GT.SiteCheck.Entity.Entities;
using GT.SiteCheck.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity;

namespace GT.SiteCheck.BLL.impl
{
    public class SiteBLL : ISiteBLL
    {
        //创建容器
        UnityContainer container = new UnityContainer();
        ISiteDao ISite;

        public SiteBLL()
        {
            //注册依赖对象
            container.RegisterType<ISiteDao, SiteDao>();
            ISite = container.Resolve<SiteDao>();
        }

        public List<Site> getSites()
        {
            return ISite.getSites();
        }

        public List<JobSite> getJobSites()
        {
            return ISite.getJobSites();
        }

        public List<Site> getSiteByPid(int pId)
        {
            return ISite.getSiteByPid(pId);
        }

        public Site getSiteById(int Id)
        {
            return ISite.getSiteById(Id);
        }

        public bool updateSiteStatus(int id, string code)
        {
            return ISite.updateSiteStatus(id, code);
        }

        public RetMsg updateSiteUseFlag(string id, int flag)
        {
            return ISite.updateSiteUseFlag(id, flag);
        }

        public int getSiteCount(int pId)
        {
            return ISite.getSiteCount(pId);
        }

        public List<Site> getSiteByPage(int pageSize, int pageIndex, int parentId)
        {
            return ISite.getSiteByPage(pageSize, pageIndex, parentId);
        }

        public RetMsg Add(Site site)
        {
            return ISite.Add(site);
        }

        public RetMsg Update(Site site)
        {           
            return ISite.Update(site);
        }
    }
}
