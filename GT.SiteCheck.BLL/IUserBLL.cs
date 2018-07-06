using GT.SiteCheck.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GT.SiteCheck.BLL
{
    public interface IUserBLL
    {
        List<User> getUsers();

        User getUserByName(string name, string pwd);
    }
}
