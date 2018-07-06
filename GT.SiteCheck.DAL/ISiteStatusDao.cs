using GT.SiteCheck.DAL.impl;
using GT.SiteCheck.Entity.Entities;
using GT.SiteCheck.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GT.SiteCheck.DAL
{
    public interface ISiteStatusDao
    {
        RetMsg Add(SiteStatus status);
    }
}
