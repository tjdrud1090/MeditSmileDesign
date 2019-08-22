using GalaSoft.MvvmLight;
using Process_Page;
using Process_Page_Change.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Process_Page_Change.ViewModel
{
    class layoutViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public static ObservableCollection<layout> _collection { get; set; }
        public static BitmapImage layoutimage;

        public layoutViewModel()
        {
            FillObservableCollection();
        }

        private void FillObservableCollection()
        {

            _collection = new ObservableCollection<layout>
            {

               new layout { layoutfile = PatientInfo.Patient_Info.frontfile},
               new layout { layoutfile = PatientInfo.Patient_Info.teeth_opener_file},
               new layout { layoutfile = PatientInfo.Patient_Info.downfacefile},
               new layout { layoutfile = PatientInfo.Patient_Info.upfacefile},
               new layout { layoutfile = PatientInfo.Patient_Info.Lfacefile},
               new layout { layoutfile = PatientInfo.Patient_Info.Rfacefile},
            };



        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public class layout
        {
            public BitmapImage layoutfile { get; set; }
        }



        private layout p_SelectedItem;
        public layout Selected
        {
            get { return p_SelectedItem; }

            set
            {
                p_SelectedItem = value;
                //  MessageBox.Show(p_SelectedItem.Name);
                layoutimage = p_SelectedItem.layoutfile;

                //Num1 = p_SelectedItem.Age;

                //((SmileDesign_Page)((MainWindow)Application.Current.MainWindow).Content).layoutimage.Source = layoutimage;
                OnPropertyChanged("Selected");


            }
        }
    }
}
