using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignColors.WpfExample.Domain;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Process_Page;
using Process_Page.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Process_Page
{
    public class LoginViewModel : ViewModelBase
    {
        public static string Dr_name;
        public static string Dr_num;
        public static string Dr_ID;
        public static string Dr_Password;

        public static string front_img;
        public static string opener_img;
        public static string up_img;
        public static string down_img;
        public static string L_img;
        public static string R_img;


        public static string logDr_name;

        public RelayCommand<object> Loginclicked { get; }
        public RelayCommand<object> Signupcked { get; }
        public LoginViewModel()
        {
            Loginclicked = new RelayCommand<object>(param => this.Loginck());
            Signupcked = new RelayCommand<object>(param => this.Signup());
        }

        public void Loginck()
        {
            System.Windows.Application.Current.MainWindow.UpdateLayout();
            Login_Page currentPage = (System.Windows.Application.Current.MainWindow.Content) as Login_Page;
            string a = currentPage.IDtext.Text;

            //string[] lines = File.ReadAllLines(@"C:\Users\bit-user\Desktop\Process_Page_Copy\Process_Page\save\TitleList.txt");
            //foreach (string show in lines)
            //{
            //    int count = 0;
            //    string[] Flines = File.ReadAllLines(@"C:\Users\bit-user\Desktop\Process_Page_Copy\Process_Page\save\" + show);
            //    foreach (string Fshow in Flines)
            //    {
            FileInfo fileInfo = new FileInfo(@"C:\Users\bit\Desktop\Process_Page (4)\Process_Page\DrInfo\" + currentPage.IDtext.Text + "(" + currentPage.PasswordBox.Password + ")" + ".txt");

            if (fileInfo.Exists)
            {

                string[] lines = File.ReadAllLines(@"C:\Users\bit\Desktop\Process_Page (4)\Process_Page\DrInfo\" + currentPage.IDtext.Text + "(" + currentPage.PasswordBox.Password + ")" + ".txt");
                foreach (string show in lines)
                {
                    logDr_name = show;
                    break;
                }
                PatientInfo_Page page = new PatientInfo_Page();
                System.Windows.Application.Current.MainWindow.Content = page;
            }
            else
            {
                ExecuteRunDialog(0);
                return;
            }


        }

        public void Signup()
        {
            string[] lines = { Dr_name, Dr_num, Dr_ID, Dr_Password };
            using (StreamWriter outputFile = new StreamWriter(@"C:\Users\bit\Desktop\Process_Page (4)\Process_Page\DrInfo\" + Dr_ID + "(" + Dr_Password + ")" + ".txt"))
            {
                foreach (string line in lines)
                {
                    outputFile.WriteLine(line);
                }
            }

        }


        private async void ExecuteRunDialog(object o)
        {
            //let's set up a little MVVM, cos that's what the cool kids are doing:
            var view = new SampleMessageDialog
            {
                DataContext = new LoginViewModel()
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


        public ImageSource SourceP
        {
            get { return bi; }
            set { }
        }

        public ImageSource GGSourceP
        {
            get { return GGbi; }
            set { }
        }

        public ImageSource upSourceP
        {
            get { return upbi; }
            set { }
        }
        public ImageSource downSourceP
        {
            get { return downbi; }
            set { }
        }
        public ImageSource LSourceP
        {
            get { return Lbi; }
            set { }
        }
        public ImageSource RSourceP
        {
            get { return Rbi; }
            set { }
        }
        //image original
        BitmapImage bi = new BitmapImage();
        //openfile dialog
        OpenFileDialog ofd = new OpenFileDialog();

        BitmapImage GGbi = new BitmapImage();
        //openfile dialog
        OpenFileDialog GGofd = new OpenFileDialog();

        BitmapImage upbi = new BitmapImage();
        //openfile dialog
        OpenFileDialog upofd = new OpenFileDialog();

        BitmapImage downbi = new BitmapImage();
        //openfile dialog
        OpenFileDialog downofd = new OpenFileDialog();

        BitmapImage Lbi = new BitmapImage();
        //openfile dialog
        OpenFileDialog Lofd = new OpenFileDialog();

        BitmapImage Rbi = new BitmapImage();
        //openfile dialog
        OpenFileDialog Rofd = new OpenFileDialog();

        //command에 들어갈 file 열기 명령
        private void openFile()
        {
            if (ofd.ShowDialog() == true)
            {
                // 파일 열기
                front_img = ofd.FileName;
                BitmapImage bb = new BitmapImage();
                bb.BeginInit();
                bb.UriSource = new Uri(ofd.FileName);

                bb.Rotation = Rotation.Rotate90;
                bb.EndInit();
                //FaceDetector faceDetector = new FaceDetector(ofd.FileName);
                bi = bb;
                RaisePropertyChanged("SourceP");
                DialogsViewModel.FileName = bb;

            }
        }

        private void GGopenFile()
        {

            if (GGofd.ShowDialog() == true)
            {
                // 파일 열기
                opener_img = GGofd.FileName;
                BitmapImage bb = new BitmapImage();
                bb.BeginInit();
                bb.UriSource = new Uri(GGofd.FileName);

                bb.Rotation = Rotation.Rotate90;
                bb.EndInit();
                //FaceDetector faceDetector = new FaceDetector(ofd.FileName);
                GGbi = bb;
                RaisePropertyChanged("GGSourceP");
                DialogsViewModel.GGFileName = bb;

            }
        }

        private void upopenFile()
        {

            if (upofd.ShowDialog() == true)
            {
                // 파일 열기
                up_img = upofd.FileName;
                BitmapImage bb = new BitmapImage();
                bb.BeginInit();
                bb.UriSource = new Uri(upofd.FileName);

                bb.Rotation = Rotation.Rotate90;
                bb.EndInit();
                //FaceDetector faceDetector = new FaceDetector(ofd.FileName);
                upbi = bb;
                RaisePropertyChanged("upSourceP");
                DialogsViewModel.upFileName = bb;

            }
        }
        private void downopenFile()
        {

            if (downofd.ShowDialog() == true)
            {
                // 파일 열기
                down_img = downofd.FileName;
                BitmapImage bb = new BitmapImage();
                bb.BeginInit();
                bb.UriSource = new Uri(downofd.FileName);

                bb.Rotation = Rotation.Rotate90;
                bb.EndInit();
                //FaceDetector faceDetector = new FaceDetector(ofd.FileName);
                downbi = bb;
                RaisePropertyChanged("downSourceP");
                DialogsViewModel.downFileName = bb;

            }
        }
        private void LopenFile()
        {

            if (Lofd.ShowDialog() == true)
            {
                // 파일 열기
                L_img = Lofd.FileName;
                BitmapImage bb = new BitmapImage();
                bb.BeginInit();
                bb.UriSource = new Uri(Lofd.FileName);

                bb.Rotation = Rotation.Rotate90;
                bb.EndInit();
                //FaceDetector faceDetector = new FaceDetector(ofd.FileName);
                Lbi = bb;
                RaisePropertyChanged("LSourceP");
                DialogsViewModel.LFileName = bb;

            }
        }
        private void RopenFile()
        {

            if (Rofd.ShowDialog() == true)
            {
                // 파일 열기
                R_img = Rofd.FileName;
                BitmapImage bb = new BitmapImage();
                bb.BeginInit();
                bb.UriSource = new Uri(Rofd.FileName);

                bb.Rotation = Rotation.Rotate90;
                bb.EndInit();
                //FaceDetector faceDetector = new FaceDetector(ofd.FileName);
                Rbi = bb;
                RaisePropertyChanged("RSourceP");
                DialogsViewModel.RFileName = bb;

            }
        }



        private RelayCommand<object> _openFileClickP;
        public RelayCommand<object> openFileClickP
        {
            get
            {
                if (_openFileClickP == null)
                    return _openFileClickP = new RelayCommand<object>(param => this.openFile());
                return _openFileClickP;
            }
            set
            {
                _openFileClickP = value;
            }
        }


        private RelayCommand<object> _openGGFileClickP;
        public RelayCommand<object> openGGFileClickP
        {
            get
            {
                if (_openGGFileClickP == null)
                    return _openGGFileClickP = new RelayCommand<object>(param => this.GGopenFile());
                return _openGGFileClickP;
            }
            set
            {
                _openGGFileClickP = value;
            }
        }




        private RelayCommand<object> _openupFileClickP;
        public RelayCommand<object> openupFileClickP
        {
            get
            {
                if (_openupFileClickP == null)
                    return _openupFileClickP = new RelayCommand<object>(param => this.upopenFile());
                return _openupFileClickP;
            }
            set
            {
                _openupFileClickP = value;
            }
        }

        private RelayCommand<object> _opendownFileClickP;
        public RelayCommand<object> opendownFileClickP
        {
            get
            {
                if (_opendownFileClickP == null)
                    return _opendownFileClickP = new RelayCommand<object>(param => this.downopenFile());
                return _opendownFileClickP;
            }
            set
            {
                _opendownFileClickP = value;
            }
        }

        private RelayCommand<object> _openLFileClickP;
        public RelayCommand<object> openLFileClickP
        {
            get
            {
                if (_openLFileClickP == null)
                    return _openLFileClickP = new RelayCommand<object>(param => this.LopenFile());
                return _openLFileClickP;
            }
            set
            {
                _openLFileClickP = value;
            }
        }

        private RelayCommand<object> _openRFileClickP;
        public RelayCommand<object> openRFileClickP
        {
            get
            {
                if (_openRFileClickP == null)
                    return _openRFileClickP = new RelayCommand<object>(param => this.RopenFile());
                return _openRFileClickP;
            }
            set
            {
                _openRFileClickP = value;
            }
        }
    }
}
