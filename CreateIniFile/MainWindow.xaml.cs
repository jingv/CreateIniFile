using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private int gridMarginTop = 0;
        private ViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();

            iniFile = IniFileHandler.CopyFile();
            viewModel = new ViewModel();

            ItemsView.ItemsSource = viewModel.TreeDatas;

            // 窗口关闭事件
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            File.Delete(iniFile);
        }
 
        // 点击TreeView中的item
        private void StackPanel_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            KeyValuesArea.Children.Clear();
            gridMarginTop = 0;

            if (ItemsView.SelectedItem is null)
            {
                // 如果未选中任何元素，则将右侧信息框清空
                TreePath.Text = "";
                SectionName.Text = "";
                return;
            }

            TreeData selectedItem = ItemsView.SelectedItem as TreeData;
            TreePath.Text = viewModel.getTreePath((ItemsView.SelectedItem as TreeData), viewModel.Nodes);
            // 如果选中的是Section
            if (selectedItem.ParentId == 0)
            {
                SectionName.Text = selectedItem.NodeValue;
                KeyValuesArea.Children.Add(CreateGrid(ViewModel.getChildNodes(selectedItem.NodeId, viewModel.Nodes)));
            }
            // 如果选中的是KeyValuePair
            else
            {
                List<TreeData> a = new List<TreeData>();
                SectionName.Text = ViewModel.getNodes(selectedItem.ParentId, viewModel.Nodes).NodeValue;
                KeyValuesArea.Children.Add(CreateGrid((new List<TreeData>() {
                    ViewModel.getNodes(selectedItem.NodeId, viewModel.Nodes),
                })));
            }

        }
        private Grid CreateGrid(List<TreeData> treeDatas)
        {
            //int textBoxMarginTop = 0;
            Grid grid = new Grid
            {
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, gridMarginTop, 10, 0),
            };
            gridMarginTop += 5;
            foreach (TreeData item in treeDatas)
            {
                grid.Children.Add(new TextBox
                {
                    Width = 300,
                    Height = 30,
                    Text = item.NodeValue.Split("=")[0].Trim(),
                    Margin = new Thickness(0, gridMarginTop, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                });
                grid.Children.Add(new TextBlock
                {
                    Width = 10,
                    Height = 30,
                    Text = "=",
                    Margin = new Thickness(300, gridMarginTop, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    
                });
                foreach (string value in item.NodeValue.Split("=")[1].Split(","))
                {
                    grid.Children.Add(new TextBox
                    {
                        Width = 300,
                        Height = 30,
                        Text = value.Trim(),
                        Margin = new Thickness(310, gridMarginTop, 0, 0),
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Left,
                    });
                    gridMarginTop += 35;
                }
            }
            return grid;
        } 
    }
}
