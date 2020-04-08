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
        private string iniFile;
        private ViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            iniFile = IniFileHandler.CopyFile();
            viewModel = new ViewModel();

            ItemsView.ItemsSource = viewModel.TreeDatas;

            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            File.Delete(iniFile);
        }
 
        // 点击TreeView中的item
        private void StackPanel_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            TreePath.Text = viewModel.getTreePath((ItemsView.SelectedItem as TreeData), viewModel.Nodes);
        }
    }
}
