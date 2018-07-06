using GT.SiteCheck.Entity;
using GT.SiteCheck.Entity.Entities;
using GT.SiteCheck.Entity.ViewEntity;
using System;

namespace GT.SiteCheck.DAL.impl
{
    public class SiteStatusDao : ISiteStatusDao
    {
        ScDbContext db = new ScDbContext();

        public RetMsg Add(SiteStatus status)
        {
            RetMsg ret = new RetMsg();
            try
            {
                db.Status.Add(status);
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
    }
}
