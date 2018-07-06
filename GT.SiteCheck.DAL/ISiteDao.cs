using GT.SiteCheck.Entity.Entities;
using GT.SiteCheck.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GT.SiteCheck.DAL
{
    public interface ISiteDao
    {
        List<Site> getSites();

        List<JobSite> getJobSites();

        List<Site> getSiteByPid(int pId);

        Site getSiteById(int Id);

        bool updateSiteStatus(int id, string code);

        RetMsg updateSiteUseFlag(string id, int flag);

        int getSiteCount(int pId);

        List<Site> getSiteByPage(int pageSize, int pageIndex, int parentId);

        RetMsg Add(Site site);

        RetMsg Update(Site site);
    }
}
