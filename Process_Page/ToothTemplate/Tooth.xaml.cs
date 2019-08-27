using Process_Page.Cards;
using Process_Page.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Process_Page.ToothTemplate
{
    /// <summary>
    /// Tooth.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    using ToothType = ObservableCollection<ObservableCollection<PointViewModel>>;
    using TeethType = ObservableCollection<PointViewModel>;
    public partial class Tooth : UserControl
    {
        public Tooth()
        {
            InitializeComponent();
        }

        //toothtype points binding
        #region tooth point data template
        public static readonly DependencyProperty ToothDataProperty
                = DependencyProperty.Register("Tooth_Points", typeof(IEnumerable), typeof(Tooth));

        public IEnumerable Tooth_Points
        {
            get { return (ToothType)GetValue(ToothDataProperty); }
            set {
                SetValue(ToothDataProperty, value);
            }
        }
        #endregion

        public static readonly DependencyProperty ShowLengthsProperty =
            DependencyProperty.Register("ShowLengths", typeof(bool), typeof(Tooth));

        public bool ShowLengths
        {
            get { return (bool)GetValue(ShowLengthsProperty); }
            set { SetValue(ShowLengthsProperty, value); }
        }
    }
}
