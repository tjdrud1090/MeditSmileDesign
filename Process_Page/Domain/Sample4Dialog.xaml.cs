using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Process_Page;
using Process_Page.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace MaterialDesignColors.WpfExample.Domain
{
    /// <summary>
    /// Interaction logic for SampleDialog.xaml
    /// </summary>s
    public partial class Sample4Dialog : UserControl
    {
        public Sample4Dialog()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //  List<DataList> p = DataList.GetPerson(name.Text, num.Text);


            DialogsViewModel.Name1 = name.Text;
            DialogsViewModel.Num1 = num.Text;
            DialogsViewModel.Date1 = date.Text;
            DialogsViewModel.Memo1 = memo.Text;
            DialogsViewModel.addadd();

        }
    }
}
