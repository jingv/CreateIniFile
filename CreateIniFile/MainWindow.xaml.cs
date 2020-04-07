using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CreateIniFile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<TreeData> nodes;
        private string IniFile;
        public MainWindow()
        {
            InitializeComponent();

            

            InputData();
            ItemsView.ItemsSource = ViewModel.getChildNodes(0, nodes);// getNodes(0, nodes);
            //this.DataContext = new ViewModel();

            IniFile = IniFileHandler.CopyFile();
            IniFileHandler.Write("S1", "1", "a", IniFile);
            IniFileHandler.Write("S1", "2", "a", IniFile);
            IniFileHandler.Write("S1", "3", "a", IniFile);
            IniFileHandler.Write("S2", "4", "a", IniFile);
            IniFileHandler.Write("S2", "5", "a", IniFile);
            IniFileHandler.Write("S2", "6", "a", IniFile);


            IniFileHandler.DeleteKey("S1", "2", IniFile);
            IniFileHandler.DeleteSection("S2", IniFile);





            this.Closing += MainWindow_Closing;




        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            File.Delete(IniFile);
        }

        private void InputData()
        {
            nodes = new List<TreeData>()
            {
                new TreeData(){ParentId = 0, NodeId = 1, NodeValue = "1"},
                new TreeData(){ParentId = 1, NodeId = 2, NodeValue = "2"},
                new TreeData(){ParentId = 2, NodeId = 3, NodeValue = "3"},
                new TreeData(){ParentId = 3, NodeId = 4, NodeValue = "4"},
                new TreeData(){ParentId = 4, NodeId = 5, NodeValue = "5"},
                new TreeData(){ParentId = 5, NodeId = 6, NodeValue = "6"},
                new TreeData(){ParentId = 6, NodeId = 7, NodeValue = "7"}
            };
        }
        private List<TreeData> getNodes(int parentId, List<TreeData> nodes)
        {
            List<TreeData> mainNodes = nodes.Where(x => x.ParentId == parentId).ToList();
            List<TreeData> otherNodes = nodes.Where(x => x.ParentId != parentId).ToList();
            foreach (TreeData node in mainNodes)
                node.ChildNodes = getNodes(node.NodeId, otherNodes);
            return mainNodes;
        }
    }
}
