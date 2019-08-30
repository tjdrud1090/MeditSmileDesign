using Process_Page;
using System.Windows.Controls;

namespace MaterialDesignColors.WpfExample.Domain
{
    /// <summary>
    /// Interaction logic for SampleDialog.xaml
    /// </summary>
    public partial class SampleDialog : UserControl
    {
        public SampleDialog()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            LoginViewModel.Dr_ID = sign_ID.Text;
            LoginViewModel.Dr_name = sign_name.Text;
            LoginViewModel.Dr_num = sign_num.Text;
            LoginViewModel.Dr_Password = sign_pass.Text;
        }


    }
}

