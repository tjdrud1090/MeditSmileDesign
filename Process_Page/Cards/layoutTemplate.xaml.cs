using GalaSoft.MvvmLight;
using Process_Page_Change.Util;
using Process_Page_Change.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Process_Page_Change.Cards
{
    /// <summary>
    /// layoutTemplate.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class layoutTemplate : UserControl, INotifyPropertyChanged
    {
        public layoutTemplate()
        {
            InitializeComponent();
            this.DataContext = this;
            FillObservableCollection();
            patient.ItemsSource = collection;
        }

        // selected item
        public static readonly DependencyProperty SelectedProperty
        = DependencyProperty.Register("Selected", typeof(layout), typeof(layoutTemplate), new PropertyMetadata(SelectedChanged));

        public layout Selected
        {
            get { return (layout)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }
        static void SelectedChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var bc = o as layoutTemplate;
            if (bc != null) bc.OnPropertyChanged("Selected");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        //private layout _SelectedItem;
        //public layout SelectedItem
        //{
        //    get
        //    {
        //        if (_SelectedItem != null)
        //        {
        //            Selected = _SelectedItem.imagesource;
        //            OnPropertyChanged("Selected");
        //        }
        //        return _SelectedItem;
        //    }
        //    set
        //    {
        //        _SelectedItem = value;
        //    }
        //}


        // Collection
        public static ObservableCollection<layout> collection { get; set; }

        public class layout
        {
            public BitmapImage layoutfile { get; set; }
            public ImageSource imagesource { get; set; }
        }
        private void FillObservableCollection()
        {
            collection = new ObservableCollection<layout>
            {
               new layout { layoutfile = PatientInfo.Patient_Info.frontfile, imagesource =  PatientInfo.Patient_Info.frontfile },
               new layout { layoutfile = PatientInfo.Patient_Info.teeth_opener_file, imagesource =  PatientInfo.Patient_Info.teeth_opener_file },
               new layout { layoutfile = PatientInfo.Patient_Info.downfacefile, imagesource =  PatientInfo.Patient_Info.downfacefile },
               new layout { layoutfile = PatientInfo.Patient_Info.upfacefile, imagesource =  PatientInfo.Patient_Info.upfacefile },
               new layout { layoutfile = PatientInfo.Patient_Info.Lfacefile, imagesource =  PatientInfo.Patient_Info.Lfacefile },
               new layout { layoutfile = PatientInfo.Patient_Info.Rfacefile, imagesource =  PatientInfo.Patient_Info.Rfacefile },
            };
        }
    }
}
