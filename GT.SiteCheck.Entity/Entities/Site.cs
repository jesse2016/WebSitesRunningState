using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GT.SiteCheck.Entity.Entities
{
    [Serializable]
    public class Site
    {
        public int id { set; get; }

        public int pId { set; get; }

        public string name { set; get; }

        public string description { set; get; }

        public int type { set; get; }

        public string url { set; get; }

        public string paras { set; get; }

        public int? interval { set; get; }

        public int isuse { set; get; }

        public string status { set; get; }

        public string mailgroup { set; get; }

        public string msggroup { set; get; }

        public string createPerson { set; get; }

        public string lastUpdatePerson { set; get; }

        public DateTime lastUpdateDate { set; get; }

        public DateTime createDate { set; get; }
    }
}
