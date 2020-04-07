using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Windows.Diagnostics;

namespace CreateIniFile
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
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
            //Nodes是第一层节点
            //TreeDatas = getChildNodes(0, Nodes);
        }
        public static List<TreeData> getChildNodes(int parentId, List<TreeData> nodes)
        {
            List<TreeData> mainNodes = nodes.Where(x => x.ParentId == parentId).ToList();
            List<TreeData> otherNodes = nodes.Where(x => x.ParentId != parentId).ToList();
            foreach (TreeData node in mainNodes)
                node.ChildNodes = getChildNodes(node.NodeId, otherNodes);
            return mainNodes;
        }
    }
}
