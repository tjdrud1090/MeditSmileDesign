using MaterialDesignColors.WpfExample.Domain;
using MaterialDesignDemo.Domain;
using MaterialDesignThemes.Wpf;
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

namespace Process_Page.TranslationDemo
{
    /// <summary>
    /// Slider1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Slider1 : UserControl
    {
        public ICommand RunDialogCommand => new AnotherCommandImplementation(ExecuteRunDialog);


        public Slider1()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel();
        }
        private async void ExecuteRunDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new SampleDialog
            {
                DataContext = new SampleDialogViewModel()
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);

            //check the result...
            Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
        }
        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("You can intercept the closing event, and cancel here.");
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            object a = 0;
            ExecuteRunDialog(a);
        }
    }
}
