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
    public class SiteStatusBLL : ISiteStatusBLL
    {
        //创建容器
        UnityContainer container = new UnityContainer();
        ISiteStatusDao IStatus;

        public SiteStatusBLL()
        {
            //注册依赖对象
            container.RegisterType<ISiteStatusDao, SiteStatusDao>();
            IStatus = container.Resolve<SiteStatusDao>();
        }

        public RetMsg Add(SiteStatus status)
        {
            return IStatus.Add(status);
        }
    }
}
