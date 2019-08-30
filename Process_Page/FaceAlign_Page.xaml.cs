using Process_Page.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Process_Page
{
    /// <summary>
    /// FaceAlign_Page.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FaceAlign_Page : Page
    {
        public FaceAlign_Page()
        {
            InitializeComponent();
            this.DataContext = new FaceAlign_PageViewModel();
          
        }

        private void Logout_Clicked(object sender, RoutedEventArgs e)
        {
            Login_Page page = new Login_Page();
            System.Windows.Application.Current.MainWindow.Content = page;
        }

        private void End_Clicked(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
