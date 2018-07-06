using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GT.SiteCheck.Entity.Entities
{
    public class User
    {
        public int id { set; get; }

        public string username { set; get; }

        public string password { set; get; }

        public int userType { set; get; }
    }
}
