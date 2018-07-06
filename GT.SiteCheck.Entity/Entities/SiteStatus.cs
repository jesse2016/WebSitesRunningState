using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GT.SiteCheck.Entity.Entities
{
    public class SiteStatus
    {
        public int id { set; get; }

        public int siteId { set; get; }

        public string paras { set; get; }

        public string status { set; get; }

        public string errorMsg { set; get; }

        public DateTime createDate { set; get; }
    }
}
