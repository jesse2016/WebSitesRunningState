using GT.SiteCheck.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GT.SiteCheck.Tools
{
    public class Util
    {
        #region 封装成ztree类
        /// <summary>
        /// 封装成ztree类
        /// </summary>
        /// <param name="listAll">全部数据list</param>
        /// <param name="parentid">根节点的id</param>
        /// <returns></returns>
        public static List<zTree> GetJsonTreeData(List<TreeNode> listAll, int parentid)
        {
            List<zTree> listTree = new List<zTree>();

            IEnumerable<TreeNode> list = listAll.Where(p => p.parentId == parentid);//使用linq查询，必须重复查询数据库，数据量小时适用
            if (list.Count() > 0)
            {
                zTree ztree = null;

                foreach (TreeNode item in list)
                {
                    ztree = new zTree();
                    ztree.id = item.nodeId;
                    ztree.pId = item.parentId;
                    ztree.name = item.nodeName;
                    ztree.level = item.level;
                    List<zTree> listChildren = GetJsonTreeData(listAll, item.nodeId);
                    if (listChildren.Count > 0)
                    {
                        ztree.isParent = true;
                        ztree.children = listChildren;
                    }
                    else
                    {
                        ztree.isParent = false;
                        ztree.children = null;
                    }

                    listTree.Add(ztree);
                }
            }

            return listTree;
        }
        #endregion
    }
}
