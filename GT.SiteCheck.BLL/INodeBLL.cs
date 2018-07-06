using GT.SiteCheck.Entity.Entities;
using GT.SiteCheck.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GT.SiteCheck.BLL
{
    public interface INodeBLL
    {
        List<TreeNode> GetNodeByNodeId(int nodeId);

        Nodes GetNodeById(int Id);
    }
}
