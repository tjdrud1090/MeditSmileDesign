using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using MaterialDesignColors.WpfExample.Domain;
using MaterialDesignDemo.Domain;
using MaterialDesignThemes.Wpf;
using Process_Page_Change.Util;

namespace Process_Page
{
    public class DialogsViewModel : INotifyPropertyChanged
    {
        private string originName;
        private string originDate;

        public static string Name1;
        public static string Num1;
        public static string Date1;
        public static string Memo1;
        public static BitmapImage FileName;
        public static BitmapImage GGFileName;
        public static BitmapImage upFileName;
        public static BitmapImage downFileName;
        public static BitmapImage LFileName;
        public static BitmapImage RFileName;

        public static string infoName1;
        public static string infoNum1;
        public static string InfoDate;
        public static string InfoMemo;
        public static BitmapImage infoFileName;
        public static BitmapImage infoGGFileName;
        public static BitmapImage infodownFileName;
        public static BitmapImage infoupFileName;
        public static BitmapImage infoLFileName;
        public static BitmapImage infoRFileName;
        public DialogsViewModel()
        {

            //Sample 4
            OpenSample4DialogCommand = new AnotherCommandImplementation(OpenSample4Dialog);
            //        AcceptSample4DialogCommand = new AnotherCommandImplementation(AcceptSample4Dialog);
            CancelSample4DialogCommand = new AnotherCommandImplementation(CancelSample4Dialog);
            //   AddPatCommand = new AnotherCommandImplementation(AddComm);
            ChangePageCommand = new AnotherCommandImplementation(nextPage);
            delPatient = new AnotherCommandImplementation(deletePatient);
            changinfo = new AnotherCommandImplementation(change);
            logoutCommand = new AnotherCommandImplementation(logout);
            exitCommand = new AnotherCommandImplementation(exit);
           
            FillObservableCollection();
        }

        private void logout(object obj)
        {
            Login_Page page = new Login_Page();
            System.Windows.Application.Current.MainWindow.Content = page;
        }

        private void exit(object obj)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void change(object obj)
        {
            for (int i = 0; i < _collection.Count; i++)
            {
                if (_collection[i].Name == originName)
                {
                    _collection.RemoveAt(i);

                    _collection.Insert(i, new Person1 { Name = infoName1, Age = infoNum1, Memo = InfoMemo, Date = InfoDate, file = infoFileName, GGfile = infoGGFileName, downfile = infodownFileName, upfile = infoupFileName, Lfile = infoLFileName, Rfile = infoRFileName });

                    string dir = @"C:\Users\bit\Desktop\Process_Page (4)\Process_Page\save\" + originName + "(" + originDate + ")" + ".txt";
                    FileInfo file = new FileInfo(dir);
                    if (file.Exists) //삭제할 파일이 있는지
                    {
                        file.Delete();
                        //  System.Console.WriteLine("폴더 삭제");

                    }
                    string[] lines = { infoName1, infoNum1, InfoMemo, InfoDate, LoginViewModel.front_img, LoginViewModel.opener_img, LoginViewModel.down_img, LoginViewModel.up_img, LoginViewModel.L_img, LoginViewModel.R_img };

                    using (StreamWriter outputFile = new StreamWriter(@"C:\Users\bit\Desktop\Process_Page (4)\Process_Page\save\" + originName + "(" + originDate + ")" + ".txt"))
                    {
                        foreach (string line in lines)
                        {
                            outputFile.WriteLine(line);
                        }
                    }

                    return;
                }
            }
           

        }

        private void deletePatient(object obj)
        {
            string dir = @"C:\Users\bit\Desktop\Process_Page (4)\Process_Page\save\" + infoName1 + "(" + InfoDate + ")" + ".txt";
            FileInfo file = new FileInfo(dir);
            if (file.Exists) //삭제할 파일이 있는지
            {
                file.Delete();
                //  System.Console.WriteLine("폴더 삭제");

            }
            for (int i = 0; i < _collection.Count; i++)
            {
                if (_collection[i].Name == infoName1)
                {
                    _collection.RemoveAt(i);

                }

            }

            List<string> lines = new List<string>();

            using (TextReader tReader = new StreamReader(@"C:\Users\bit\Desktop\Process_Page (4)\Process_Page\save\TitleList.txt"))
            {
                string line = string.Empty;
                while ((line = tReader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            var tempLines = lines.ToArray();

            foreach (var line in tempLines)
            {
                if (line.Contains(infoName1))
                {
                    lines.Remove(line);
                }
            }

            using (TextWriter tWriter = new StreamWriter(@"C:\Users\bit\Desktop\Process_Page (4)\Process_Page\save\TitleList.txt"))
            {
                foreach (var line in lines)
                {
                    tWriter.WriteLine(line);
                }
            }

            infoName1 = null;
            infoNum1 = null;
            InfoDate = null;
            InfoMemo = null;
            infoFileName = null;
            infoGGFileName = null;
            infodownFileName = null;
            infoupFileName = null;
            infoLFileName = null;
            infoRFileName = null;


            OnPropertyChanged("Nameinfo");
            OnPropertyChanged("Numinfo");
            OnPropertyChanged("Dateinfo");
            OnPropertyChanged("Memoinfo");
            OnPropertyChanged("Selectedfile");
            OnPropertyChanged("SelectedGGfile");
            OnPropertyChanged("Selecteddownfile");
            OnPropertyChanged("Selectedupfile");
            OnPropertyChanged("SelectedLfile");
            OnPropertyChanged("SelectedRfile");





        }

        public ICommand ChangePageCommand { get; }
        public void nextPage(object obj)
        {
            SmileDesign_Page page = new SmileDesign_Page();
            System.Windows.Application.Current.MainWindow.Content = page;
        }

        #region SAMPLE 4

        //pretty much ignore all the stuff provided, and manage everything via custom commands and a binding for .IsOpen
        public ICommand OpenSample4DialogCommand { get; }
        public ICommand AcceptSample4DialogCommand { get; }
        public ICommand CancelSample4DialogCommand { get; }
        public ICommand AddPatCommand { get; }
        public ICommand delPatient { get; }
        public ICommand changinfo { get; }
        public ICommand logoutCommand { get; }
        public ICommand exitCommand { get; }
        private bool _isSample4DialogOpen;
        private object _sample4Content;

        public bool IsSample4DialogOpen
        {
            get { return _isSample4DialogOpen; }
            set
            {
                if (_isSample4DialogOpen == value) return;
                _isSample4DialogOpen = value;
                OnPropertyChanged();
            }
        }

        public object Sample4Content
        {
            get { return _sample4Content; }
            set
            {
                if (_sample4Content == value) return;
                _sample4Content = value;
                OnPropertyChanged();
            }
        }

        private void OpenSample4Dialog(object obj)
        {
            Sample4Content = new Sample4Dialog();
            IsSample4DialogOpen = true;
        }

        private void CancelSample4Dialog(object obj)
        {
            IsSample4DialogOpen = false;
            Sample4Content = null;
        }

        //private void AcceptSample4Dialog(object obj)
        //{
        //    //pretend to do something for 3 seconds, then close
        //    Sample4Content = new SampleProgressDialog();
        //    Task.Delay(TimeSpan.FromSeconds(3))
        //        .ContinueWith((t, _) => IsSample4DialogOpen = false, null,
        //            TaskScheduler.FromCurrentSynchronizationContext());
        //}

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public static ObservableCollection<Person1> _collection { get; set; }

        private void FillObservableCollection()
        {

            _collection = new ObservableCollection<Person1>
            {

                // new Person1 { Name = NameF, Age = telF, Memo = MemoF, Date= DateF, file =fileF, GGfile=GGfileF},

            };
            string NameF = null;
            string telF = null;
            string DateF = null;
            string MemoF = null;
            BitmapImage fileF = null;
            BitmapImage GGfileF = null;
            BitmapImage downfileF = null;
            BitmapImage upfileF = null;
            BitmapImage LfileF = null;
            BitmapImage RfileF = null;
            string fileFname = null;
            string GGfileFname = null;
            string downfileFname = null;
            string upfileFname = null;
            string LfileFname = null;
            string RfileFname = null;


            string[] lines = File.ReadAllLines(@"C:\Users\bit\Desktop\Process_Page (4)\Process_Page\save\TitleList.txt");
            foreach (string show in lines)
            {
                int count = 0;
                string[] Flines = File.ReadAllLines(@"C:\Users\bit\Desktop\Process_Page (4)\Process_Page\save\" + show);
                foreach (string Fshow in Flines)
                {
                    if (count == 0)
                    {
                        NameF = Fshow;
                    }
                    else if (count == 1)
                    {
                        telF = Fshow;
                    }
                    else if (count == 2)
                    {
                        MemoF = Fshow;
                    }
                    else if (count == 3)
                    {
                        DateF = Fshow;
                    }
                    else if (count == 4)
                    {
                        BitmapImage bb = new BitmapImage();
                        bb.BeginInit();
                        fileFname = Fshow;
                        bb.UriSource = new Uri(Fshow);
                        
                        bb.Rotation = Rotation.Rotate90;
                        bb.EndInit();
                        fileF = bb;
                    }
                    else if (count == 5)
                    {
                        BitmapImage bb = new BitmapImage();
                        bb.BeginInit();
                        bb.UriSource = new Uri(Fshow);
                        GGfileFname = Fshow;
                        bb.Rotation = Rotation.Rotate90;
                        bb.EndInit();
                        GGfileF = bb;
                    }
                    else if (count == 6)
                    {
                        BitmapImage bb = new BitmapImage();
                        bb.BeginInit();
                        bb.UriSource = new Uri(Fshow);
                        downfileFname = Fshow;
                        bb.Rotation = Rotation.Rotate90;
                        bb.EndInit();
                        downfileF = bb;
                    }
                    else if (count == 7)
                    {
                        BitmapImage bb = new BitmapImage();
                        bb.BeginInit();
                        bb.UriSource = new Uri(Fshow);
                        upfileFname = Fshow;
                        bb.Rotation = Rotation.Rotate90;
                        bb.EndInit();
                        upfileF = bb;
                    }
                    else if (count == 8)
                    {
                        BitmapImage bb = new BitmapImage();
                        bb.BeginInit();
                        bb.UriSource = new Uri(Fshow);
                        LfileFname = Fshow;
                        bb.Rotation = Rotation.Rotate90;
                        bb.EndInit();
                        LfileF = bb;
                    }
                    else if (count == 9)
                    {
                        BitmapImage bb = new BitmapImage();
                        bb.BeginInit();
                        bb.UriSource = new Uri(Fshow);
                        RfileFname = Fshow;
                        bb.Rotation = Rotation.Rotate90;
                        bb.EndInit();
                        RfileF = bb;
                    }
                    count++;
                }

                _collection.Add(new Person1 { Name = NameF, Age = telF, Memo = MemoF, Date = DateF, file = fileF, GGfile = GGfileF, downfile = downfileF, upfile = upfileF, Lfile = LfileF, Rfile = RfileF , filename=fileFname, GGfilename=GGfileFname, downfilename = downfileFname,upfilename=upfileFname, Lfilename=LfileFname, Rfilename = RfileFname});
            }




            // lstMy.ItemsSource = _collection;      
        }

        //public void AddComm(object obj)
        //{
        //    _collection.Add(new Person1 { Name = Name1, Age = Num1, file = FileName, Date = Date1, downfile= downFileName, Memo= });

        //}




        private Person1 p_SelectedItem;
        public Person1 Selected
        {
            get { return p_SelectedItem; }

            set
            {
                p_SelectedItem = value;
                //  MessageBox.Show(p_SelectedItem.Name);
                infoName1 = p_SelectedItem.Name;
                originName = p_SelectedItem.Name;
                infoNum1 = p_SelectedItem.Age;
                InfoDate = p_SelectedItem.Date;
                originDate = p_SelectedItem.Date;
                InfoMemo = p_SelectedItem.Memo;
                infoFileName = p_SelectedItem.file;
                infoGGFileName = p_SelectedItem.GGfile;
                infodownFileName = p_SelectedItem.downfile;
                infoupFileName = p_SelectedItem.upfile;
                infoLFileName = p_SelectedItem.Lfile;
                infoRFileName = p_SelectedItem.Rfile;
                LoginViewModel.front_img = p_SelectedItem.filename;
                LoginViewModel.opener_img = p_SelectedItem.GGfilename;
                LoginViewModel.up_img = p_SelectedItem.upfilename;
                LoginViewModel.down_img = p_SelectedItem.downfilename;
                LoginViewModel.L_img = p_SelectedItem.Lfilename;
                LoginViewModel.R_img = p_SelectedItem.Rfilename;
                //Num1 = p_SelectedItem.Age;
                PatientInfo.Patient_Info.Name = infoName1;
                PatientInfo.Patient_Info.Num = infoNum1;
                PatientInfo.Patient_Info.Date = InfoDate;
                PatientInfo.Patient_Info.Memo = InfoMemo;
                PatientInfo.Patient_Info.frontfile = infoFileName;
                PatientInfo.Patient_Info.teeth_opener_file = infoGGFileName;
                PatientInfo.Patient_Info.downfacefile = infodownFileName;
                PatientInfo.Patient_Info.upfacefile = infoupFileName;
                PatientInfo.Patient_Info.Lfacefile = infoLFileName;
                PatientInfo.Patient_Info.Rfacefile = infoRFileName;
                PatientInfo.Patient_Info.frontfilename = p_SelectedItem.filename;
                PatientInfo.Patient_Info.teeth_opener_filename = p_SelectedItem.GGfilename;
                PatientInfo.Patient_Info.downfacefilename = p_SelectedItem.downfilename;
                PatientInfo.Patient_Info.Lfacefilename = p_SelectedItem.Lfilename;
                PatientInfo.Patient_Info.Rfacefilename = p_SelectedItem.Rfilename;

                OnPropertyChanged("Selected");
                OnPropertyChanged("Nameinfo");
                OnPropertyChanged("Numinfo");
                OnPropertyChanged("Dateinfo");
                OnPropertyChanged("Memoinfo");
                OnPropertyChanged("Selectedfile");
                OnPropertyChanged("SelectedGGfile");
                OnPropertyChanged("Selecteddownfile");
                OnPropertyChanged("Selectedupfile");
                OnPropertyChanged("SelectedLfile");
                OnPropertyChanged("SelectedRfile");

            }
        }

        public class Person1
        {
            public string Name { get; set; }
            public string Age { get; set; }

            public string Date { get; set; }

            public string Memo { get; set; }
            public BitmapImage file { get; set; }

            public BitmapImage GGfile { get; set; }

            public BitmapImage downfile { get; set; }

            public BitmapImage upfile { get; set; }

            public BitmapImage Lfile { get; set; }

            public BitmapImage Rfile { get; set; }

            public string filename { get; set; }

            public string GGfilename { get; set; }

            public string downfilename { get; set; }

            public string upfilename { get; set; }

            public string Lfilename { get; set; }

            public string Rfilename { get; set; }
        }

        public static void addadd()
        {
            _collection.Add(new Person1 { Name = Name1, Age = Num1, file = FileName, Date = Date1, Memo = Memo1, GGfile = GGFileName, downfile = downFileName, upfile = upFileName, Lfile = LFileName, Rfile = RFileName , filename=LoginViewModel.front_img, GGfilename=LoginViewModel.opener_img, downfilename=LoginViewModel.down_img, upfilename= LoginViewModel.up_img, Lfilename= LoginViewModel.L_img, Rfilename=LoginViewModel.R_img});
            //string path = @"C:\Users\bit-user\Desktop\Process_Page_Copy\Process_Page\save\test.txt";
            //try
            //{
            //    System.IO.File.Create(path);
            //    System.IO.File.WriteAllText(path, "gg");

            //}
            //catch (Exception e)
            //{

            //    Console.WriteLine(e.Message.ToString());
            //}

            string[] lines = { Name1, Num1, Memo1, Date1, LoginViewModel.front_img, LoginViewModel.opener_img, LoginViewModel.down_img, LoginViewModel.up_img, LoginViewModel.L_img, LoginViewModel.R_img };

            using (StreamWriter outputFile = new StreamWriter(@"C:\Users\bit\Desktop\Process_Page (4)\Process_Page\save\TitleList.txt", true))
            {
                outputFile.WriteLine(Name1 + "(" + Date1 + ")" + ".txt");
            }

            using (StreamWriter outputFile = new StreamWriter(@"C:\Users\bit\Desktop\Process_Page (4)\Process_Page\save\" + Name1 + "(" + Date1 + ")" + ".txt"))
            {
                foreach (string line in lines)
                {
                    outputFile.WriteLine(line);
                }
            }

            FileName = null;

        }


        private string _Nameinfo;
        public string Nameinfo
        {
            get
            {
                _Nameinfo = infoName1;
                return _Nameinfo;
            }

            set
            {
                _Nameinfo = value;
                //  MessageBox.Show(p_SelectedItem.Name);
                //   _Nameinfo = infoName1; 
                //Num1 = p_SelectedItem.Age;
                originName = infoName1;

                infoName1 = value;

                OnPropertyChanged("Nameinfo");
            }
        }


        private ImageSource _Selectedfile;
        public ImageSource Selectedfile
        {
            get
            {
                _Selectedfile = infoFileName;
                return _Selectedfile;
            }

            set
            {
                _Selectedfile = value;
                //  MessageBox.Show(p_SelectedItem.Name);
                infoFileName = (BitmapImage)value;
                //Num1 = p_SelectedItem.Age;


                OnPropertyChanged("Selectedfile");
            }
        }

        private ImageSource _SelectedGGfile;
        public ImageSource SelectedGGfile
        {
            get
            {
                _SelectedGGfile = infoGGFileName;
                return _SelectedGGfile;
            }

            set
            {
                _SelectedGGfile = value;
                //  MessageBox.Show(p_SelectedItem.Name);
                infoGGFileName = (BitmapImage)value;
                //Num1 = p_SelectedItem.Age;


                OnPropertyChanged("SelectedGGfile");
            }
        }

        private ImageSource _Selecteddownfile;
        public ImageSource Selecteddownfile
        {
            get
            {
                _Selecteddownfile = infodownFileName;
                return _Selecteddownfile;
            }

            set
            {
                _Selecteddownfile = value;
                //  MessageBox.Show(p_SelectedItem.Name);
                infodownFileName = (BitmapImage)value;
                //Num1 = p_SelectedItem.Age;


                OnPropertyChanged("Selecteddownfile");
            }
        }

        private ImageSource _Selectedupfile;
        public ImageSource Selectedupfile
        {
            get
            {
                _Selectedupfile = infoupFileName;
                return _Selectedupfile;
            }

            set
            {
                _Selectedupfile = value;
                //  MessageBox.Show(p_SelectedItem.Name);
                infoupFileName = (BitmapImage)value;
                //Num1 = p_SelectedItem.Age;


                OnPropertyChanged("Selectedupfile");
            }
        }

        private ImageSource _SelectedLfile;
        public ImageSource SelectedLfile
        {
            get
            {
                _SelectedLfile = infoLFileName;
                return _SelectedLfile;
            }

            set
            {
                _SelectedLfile = value;
                //  MessageBox.Show(p_SelectedItem.Name);
                infoLFileName = (BitmapImage)value;
                //Num1 = p_SelectedItem.Age;


                OnPropertyChanged("SelectedLfile");
            }
        }

        private ImageSource _SelectedRfile;
        public ImageSource SelectedRfile
        {
            get
            {
                _SelectedRfile = infoRFileName;
                return _SelectedRfile;
            }

            set
            {
                _SelectedRfile = value;
                //  MessageBox.Show(p_SelectedItem.Name);
                infoRFileName = (BitmapImage)value;
                //Num1 = p_SelectedItem.Age;


                OnPropertyChanged("SelectedRfile");
            }
        }


        private string _Dateinfo;
        public string Dateinfo
        {
            get
            {
                _Dateinfo = InfoDate;
                return _Dateinfo;
            }

            set
            {
                _Dateinfo = value;
                //  MessageBox.Show(p_SelectedItem.Name);
                originDate = InfoDate;
                InfoDate = value;
                //Num1 = p_SelectedItem.Age;


                OnPropertyChanged("Dateinfo");
            }
        }

        private string _Numinfo;
        public string Numinfo
        {
            get
            {
                _Numinfo = infoNum1;
                return _Numinfo;
            }

            set
            {
                _Numinfo = value;
                //  MessageBox.Show(p_SelectedItem.Name);
                infoNum1 = value;
                //Num1 = p_SelectedItem.Age;


                OnPropertyChanged("Numinfo");
            }
        }


        private string _Memoinfo;
        public string Memoinfo
        {
            get
            {
                _Memoinfo = InfoMemo;
                return _Memoinfo;
            }

            set
            {
                _Memoinfo = value;
                //  MessageBox.Show(p_SelectedItem.Name);
                InfoMemo = value;
                //Num1 = p_SelectedItem.Age;


                OnPropertyChanged("Memoinfo");
            }
        }


        private string _Drname;
        public string Drname
        {
            get
            {
                _Drname = LoginViewModel.logDr_name;
                return _Drname;
            }

            set
            {
                //_Drname = value;
                //  MessageBox.Show(p_SelectedItem.Name);
                //   _Nameinfo = infoName1; 
                //Num1 = p_SelectedItem.Age;
                //originName = _Drname;



                OnPropertyChanged("Drname");
            }
        }
    }
}
