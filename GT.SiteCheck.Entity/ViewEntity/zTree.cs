using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GT.SiteCheck.Entity.ViewEntity
{
    public class zTree
    {
        public int id { get; set; }
        public int pId { get; set; }
        public string name { get; set; }
        public int level { set; get; }
        public bool isParent { get; set; }

        /// <summary>
        /// 子节点列表
        /// </summary>
        public List<zTree> children { get; set; }
    }
}
