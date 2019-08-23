using System;
using System.Collections;
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

namespace Process_Page.ToothTemplate
{
    /// <summary>
    /// Teeth.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public partial class Teeth : UserControl
    {
        public Teeth()
        {
            InitializeComponent();
        }

        #region Points

        public static readonly DependencyProperty PointsProperty
            = DependencyProperty.Register("Points", typeof(IEnumerable), typeof(Teeth));

        public IEnumerable Points
        {
            get { return (IEnumerable)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        #endregion

        #region ShowLengths
        public static readonly DependencyProperty ShowLengthXYProperty
            = DependencyProperty.Register("ShowLengthXY", typeof(bool), typeof(Teeth));

        public bool ShowLengthXY
        {
            get { return (bool)GetValue(ShowLengthXYProperty); }
            set { SetValue(ShowLengthXYProperty, value); }
        }
        #endregion

        #region Fill
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(bool), typeof(Teeth));

        public bool Fill
        {
            get { return (bool)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }
        #endregion


    }
}
