using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GT.SiteCheck.Entity
{
    public class Sql
    {
        /// <summary>
        /// 根据节点id递归查询子节点
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public static string GetNodesByNodeId(int nodeId)
        {
            string sql = @"with cte_child(nodeId, parentId, nodeName,level) 
                            as 
                            ( 
                                select nodeId, parentId, nodeName,0 as level
                                from Nodes n
                                where nodeId = 1
                                union all 
                                select a.nodeId, a.parentId, a.nodeName, b.level+1
                                from Nodes a 
                                inner join  
                                cte_child b 
                                on a.parentId=b.nodeId 
                            ) 
                            select c.* from cte_child c";
            return string.Format(sql, nodeId);
        }

        /// <summary>
        /// 根据父id分页查询服务器列表
        /// </summary>
        /// <param name="pageSize">分页大小</param>
        /// <param name="pageIndex">当前页数-1</param>
        /// <param name="parentId">父id</param>
        /// <returns></returns>
        public static string GetSiteListByPId(int pageSize, int pageIndex, int parentId)
        {
            string sql = @"select top {0} * 
                            from Sites n 
                            where n.pId = {2} 
                            and n.Id not in (select top ({0}*{1}) t.Id 
                                             from Sites t 
                                             where t.pId = {2})";
            return string.Format(sql, pageSize, pageIndex, parentId);
        }
    }
}
