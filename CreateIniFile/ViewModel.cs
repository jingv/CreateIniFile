using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Windows.Diagnostics;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shell;

namespace CreateIniFile
{
    class ViewModel : INotifyPropertyChanged
    {
        private int _nodeId = 1;
        // 只是一个包含TreeNode的集合
        public List<TreeData> Nodes = new List<TreeData>();
        public event PropertyChangedEventHandler PropertyChanged;
        // 组织为Tree的TreeNode的集合
        private List<TreeData> treeValues = new List<TreeData>();
        public List<TreeData> TreeDatas
        {
            get { return treeValues; }
            set
            {
                treeValues = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("TreeDatas"));
            }
        }

        public ViewModel()
        {
            InitData();
            //Nodes是第一层节点
            TreeDatas = getChildNodes(0, Nodes);
        }
        
        /// <summary>
        /// 将所有的nodes组织成树
        /// </summary>
        /// <param name="parentId">父id</param>
        /// <param name="nodes">包含所有node的List</param>
        /// <returns>返回树状结构</returns>
        public static List<TreeData> getChildNodes(int parentId, List<TreeData> nodes)
        {
            List<TreeData> mainNodes = nodes.Where(x => x.ParentId == parentId).ToList();
            List<TreeData> otherNodes = nodes.Where(x => x.ParentId != parentId).ToList();
            foreach (TreeData node in mainNodes)
                node.ChildNodes = getChildNodes(node.NodeId, otherNodes);
            return mainNodes;
        }

        /// <summary>
        /// 查找id为nodeId的Node
        /// </summary>
        /// <param name="NodeId">指定nodeId</param>
        /// <param name="nodes">查找node的集合</param>
        /// <returns>node</returns>
        public static TreeData getNodes(int nodeId, List<TreeData> nodes)
        {
            List<TreeData> temp = nodes.Where(x => x.NodeId == nodeId).ToList();
            if (temp.Count == 0)
                return null;
            else
                return temp[0];
        }  

        public string getTreePath(TreeData node, List<TreeData> nodes)
        {
            string result = $">{node.NodeValue}";
            if (result.Contains("="))
                result = result.Split("=")[0];
            TreeData parentNode = getNodes(node.ParentId, nodes);
            while (parentNode != null)
            {
                result = $">{parentNode.NodeValue}{result}";
                parentNode = getNodes(parentNode.ParentId, nodes);
            }
            return result;
        }
        
        /// <summary>
        /// 初始化Tree中的Data（Data从Ini文件中读取）
        /// </summary>
        private void InitData()
        {
            foreach (string section in IniFileHandler.ReadSections())
            {
                int sectionId = _nodeId;
                _nodeId += 1;
                Nodes.Add(new TreeData { NodeId = sectionId, NodeValue = section, ParentId = 0 });
                foreach (string key in IniFileHandler.ReadKeys(section))
                {
                    Nodes.Add(new TreeData { NodeId = _nodeId, NodeValue = $"{key}  =   {IniFileHandler.ReadValue(section, key)}", ParentId = sectionId });
                    _nodeId += 1;
                }
            }
        }
    }
}
