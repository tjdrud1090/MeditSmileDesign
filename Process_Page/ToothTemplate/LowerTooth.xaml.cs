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
    /// LowerTooth.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    using ToothType = ObservableCollection<ObservableCollection<PointViewModel>>;
    using TeethType = ObservableCollection<PointViewModel>;

    public partial class LowerTooth : UserControl
    {
        public LowerTooth()
        {
            InitializeComponent();
        }

        //toothtype points binding
        #region tooth point data template
        public static readonly DependencyProperty ToothDataProperty
                = DependencyProperty.Register("Tooth_Points", typeof(IEnumerable), typeof(LowerTooth));

        public IEnumerable Tooth_Points
        {
            get { return (ToothType)GetValue(ToothDataProperty); }
            set
            {
                SetValue(ToothDataProperty, value);
            }
        }
        #endregion

        #region ShowLengths
        public static readonly DependencyProperty ShowLengthsProperty =
            DependencyProperty.Register("ShowLengths", typeof(bool), typeof(LowerTooth));

        public bool ShowLengths
        {
            get { return (bool)GetValue(ShowLengthsProperty); }
            set { SetValue(ShowLengthsProperty, value); }
        }
        #endregion

        #region Fill
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(bool), typeof(LowerTooth));

        public bool Fill
        {
            get { return (bool)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }
        #endregion
    }
}
