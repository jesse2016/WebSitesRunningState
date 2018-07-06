using GT.SiteCheck.Entity;
using GT.SiteCheck.Entity.Entities;
using GT.SiteCheck.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace GT.SiteCheck.DAL.impl
{
    public class SiteDao: ISiteDao
    {
        ScDbContext db = new ScDbContext();

        public List<Site> getSites()
        {
            return db.Sites.ToList();
        }

        public List<JobSite> getJobSites()
        {
            return db.Sites.Where(p => p.isuse == 1).Join(db.Nodes, a => a.pId, g => g.nodeId, (a, g) => new JobSite { site = a, node = g }).ToList();
        }

        public List<Site> getSiteByPid(int pId)
        {
            return db.Sites.Where(p => p.pId == pId).ToList();
        }

        public Site getSiteById(int Id)
        {
            return db.Sites.Find(Id);
        }

        public bool updateSiteStatus(int id, string code)
        {
            bool isSuccess = false;
            try
            {
                Site site = db.Sites.Find(id);

                //不相等时才更新
                if (site.status != code)
                {
                    site.status = code;
                    site.lastUpdateDate = DateTime.Now;
                    db.SaveChanges();
                }
                isSuccess = true;
            }
            catch (Exception)
            {
            }
            return isSuccess;
        }

        public RetMsg updateSiteUseFlag(string id, int flag)
        {
            RetMsg ret = new RetMsg();
            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    string[] idary = id.TrimEnd(',').Split(',');
                    foreach (string _id in idary)
                    {
                        Site site = db.Sites.Find(int.Parse(_id));

                        site.isuse = flag;
                        site.lastUpdateDate = DateTime.Now;
                        db.SaveChanges();
                    }
                    scope.Complete();

                    ret.Result = true;
                }
                catch (Exception e)
                {
                    ret.Result = false;
                    ret.Msg = e.Message;
                }
                finally
                {
                    scope.Dispose();
                }
            }
            return ret;
        }

        public int getSiteCount(int pId)
        {
            return db.Sites.Where(p => p.pId == pId).Count();
        }

        public List<Site> getSiteByPage(int pageSize, int pageIndex, int parentId)
        {
            string sql = Sql.GetSiteListByPId(pageSize, pageIndex, parentId);
            return db.Database.SqlQuery<Site>(sql).ToList<Site>();
        }

        public RetMsg Add(Site site)
        {
            RetMsg ret = new RetMsg();
            try
            {
                db.Sites.Add(site);
                db.SaveChanges();

                ret.Result = true;
            }
            catch (Exception e)
            {
                ret.Result = false;
                ret.Msg = e.Message;
            }
            return ret;
        }

        public RetMsg Update(Site site)
        {
            RetMsg ret = new RetMsg();
            try
            {
                IQueryable<Site> siteList = db.Sites.Where(p => p.pId == site.pId && p.id != site.id && p.name == site.name);
                if (siteList.Count() == 0)
                {
                    Site s = db.Sites.Find(site.id);

                    s.name = site.name;
                    s.description = site.description;
                    s.type = site.type;
                    s.url = site.url;
                    s.paras = site.paras;
                    s.mailgroup = site.mailgroup;
                    s.interval = site.interval;
                    s.isuse = site.isuse;
                    s.lastUpdatePerson = site.lastUpdatePerson;
                    s.lastUpdateDate = site.lastUpdateDate;

                    db.SaveChanges();

                    ret.Result = true;
                }
                else
                {
                    ret.Result = false;
                    ret.Msg = "服务器名称已存在";
                }
            }
            catch (Exception e)
            {
                ret.Result = false;
                ret.Msg = e.Message;
            }
            return ret;
        }
    }
}
