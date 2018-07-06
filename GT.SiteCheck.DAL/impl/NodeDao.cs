using GT.SiteCheck.Entity;
using GT.SiteCheck.Entity.Entities;
using GT.SiteCheck.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GT.SiteCheck.DAL.impl
{
    public class NodeDao : INodeDao
    {
        ScDbContext db = new ScDbContext();

        public List<TreeNode> GetNodeByNodeId(int nodeId)
        {
            string sql = Sql.GetNodesByNodeId(1);
            return db.Database.SqlQuery<TreeNode>(sql).ToList<TreeNode>();
        }

        public Nodes GetNodeById(int Id)
        {
            return db.Nodes.Find(Id);
        }
    }
}
