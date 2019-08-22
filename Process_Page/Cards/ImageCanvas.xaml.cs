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

using Process_Page.ViewModel;

namespace Process_Page.Cards
{
    /// <summary>
    /// ImageCanvas.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageCanvas : UserControl
    {
        public ImageCanvas()
        {
            InitializeComponent();
            this.DataContext = new FaceControlViewModel();
        }

        //Image Source Property
        public static readonly DependencyProperty ImageSourceProperty
                = DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ImageCanvas));

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        //face points IEnumerable Property : capacity(6)
        //  - eye_midpoint 0
        //  - mouth_midpoint 1
        //  - eye_L 2
        //  - eye_R 3
        //  - mouth_L 4
        //  - mouth_R 5
        public static readonly DependencyProperty FacePointsProperty
        = DependencyProperty.Register("FacePoints", typeof(IEnumerable), typeof(ImageCanvas));

        public IEnumerable FacePoints
        {
            get { return (IEnumerable)GetValue(FacePointsProperty); }
            set { SetValue(FacePointsProperty, value); }
        }

        //Angle Property
        public static readonly DependencyProperty RotateAngleProperty
        = DependencyProperty.Register("RotateAngle", typeof(double), typeof(ImageCanvas));

        public double RotateAngle
        {
            get { return (double)GetValue(RotateAngleProperty); }
            set { SetValue(RotateAngleProperty, value); }
        }

        //Scale Property
        public static readonly DependencyProperty ScaleProperty
        = DependencyProperty.Register("ScaleXY", typeof(double), typeof(ImageCanvas));

        public double ScaleXY
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }
    }
}
