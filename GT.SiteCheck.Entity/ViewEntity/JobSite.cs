using GT.SiteCheck.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace GT.SiteCheck.Entity.ViewEntity
{
    [Serializable]
    public class JobSite
    {
        [DataMember(Name = "site")]
        public Site site { set; get; }

        public Nodes node { set; get; }
    }
}
