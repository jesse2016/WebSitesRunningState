using GT.SiteCheck.DAL;
using GT.SiteCheck.DAL.impl;
using GT.SiteCheck.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity;

namespace GT.SiteCheck.BLL.impl
{
    public class UserBLL: IUserBLL
    {
        //创建容器
        UnityContainer container = new UnityContainer();
        IUserDao IUser;

        public UserBLL() {
            //注册依赖对象
            container.RegisterType<IUserDao, UserDao>();
            IUser = container.Resolve<UserDao>();
        }

        public List<User> getUsers()
        {
            return IUser.getUsers();
        }

        public User getUserByName(string name, string pwd)
        {
            return IUser.getUserByName(name, pwd);
        }
    }
}
