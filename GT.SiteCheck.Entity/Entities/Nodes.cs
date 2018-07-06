using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace GT.SiteCheck.Entity.Entities
{
    public class Nodes
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [Key]
        public int nodeId { get; set; }

        /// <summary>
        /// 父节点ID
        /// </summary>
        public int parentId { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string nodeName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string createPerson { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime createDate { get; set; }
    }
}
