using GT.SiteCheck.DAL;
using GT.SiteCheck.DAL.impl;
using GT.SiteCheck.Entity.Entities;
using GT.SiteCheck.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity;

namespace GT.SiteCheck.BLL.impl
{
    public class NodeBLL : INodeBLL
    {
        //创建容器
        UnityContainer container = new UnityContainer();
        INodeDao INode;

        public NodeBLL()
        {
            //注册依赖对象
            container.RegisterType<INodeDao, NodeDao>();
            INode = container.Resolve<NodeDao>();
        }

        public List<TreeNode> GetNodeByNodeId(int nodeId)
        {
            return INode.GetNodeByNodeId(nodeId);
        }

        public Nodes GetNodeById(int Id)
        {
            return INode.GetNodeById(Id);
        }
    }
}
