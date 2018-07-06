using GT.SiteCheck.Entity;
using GT.SiteCheck.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GT.SiteCheck.DAL.impl
{
    public class UserDao: IUserDao
    {
        ScDbContext db = new ScDbContext();

        public List<User> getUsers()
        {
            return db.Users.Where(p => p.id < 10).ToList();
        }

        public User getUserByName(string name, string pwd)
        {
            return db.Users.Where(p => p.username == name && p.password == pwd).FirstOrDefault();
        }
    }
}
