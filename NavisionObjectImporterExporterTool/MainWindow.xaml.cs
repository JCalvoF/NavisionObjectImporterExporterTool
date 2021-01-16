using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NavisionObjectImporterExporterTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainPageViewModel vm;

        public MainWindow()
        {
            InitializeComponent();

            vm = (MainPageViewModel)this.DataContext;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ExecuteCommand();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ExecuteCommand();
        }

        private void ExecuteCommand()
        {
            if (vm.CanGenerateCommand())
            {
                ICommand cmd = this.btn_GenerateCommand.Command;
                cmd.Execute("GenerateCommandCommand");
                vm.RefreshCanexecute();
            }
            
        }
    }
}
