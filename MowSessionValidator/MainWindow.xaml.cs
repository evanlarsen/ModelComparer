using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Validator;

namespace MowSessionValidator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            btnAssembly1.Click += BtnAssembly1_Click;
            btnAssembly2.Click += BtnAssembly2_Click;
            btnRun.Click += BtnRun_Click;
            txtAssembly1.Text = @"C:\git\MobileWeb\Website\m.alaskaair.com\bin";
            txtAssembly2.Text = @"C:\git\MobileWebShopping\Shopping\bin";
        }

        private void BtnRun_Click(object sender, RoutedEventArgs e)
        {
            var main = new Main(txtAssembly1.Text, txtAssembly2.Text);
            var info = main.Run();
        }

        private void BtnAssembly2_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new FolderDialog();
            string assembly1 = fileDialog.GetFolder();
            txtAssembly2.Text = assembly1;
        }

        private void BtnAssembly1_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new FolderDialog();
            string assembly1 = fileDialog.GetFolder();
            txtAssembly1.Text = assembly1;
        }
    }
}
