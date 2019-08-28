using Process_Page.ViewModel;
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

namespace Process_Page.Cards
{
    /// <summary>
    /// Measurement_Step.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Measurement_Step : UserControl
    {
        public Measurement_Step()
        {
            InitializeComponent();
        }

        // face align viewmodel에서 받아올 값
        public readonly static DependencyProperty RefPointLProperty 
            = DependencyProperty.Register("RefPointL", typeof(Point), typeof(Measurement_Step), new FrameworkPropertyMetadata(new PropertyChangedCallback(SetRefPoints)));

        public Point RefPointL
        {
            get { return (Point)GetValue(RefPointLProperty); }
            set { SetValue(RefPointLProperty, value); }
        }

        private static void SetRefPoints(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LeftRef = ((Point)e.NewValue);
        }

        public readonly static DependencyProperty RefPointRProperty 
            = DependencyProperty.Register("RefPointR", typeof(Point), typeof(Measurement_Step), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnRefSizeChanged)));

        public Point RefPointR
        {
            get { return (Point)GetValue(RefPointRProperty); }
            set { SetValue(RefPointRProperty, value); }
        }

        private static void OnRefSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RightRef = ((Point)e.NewValue);
        }

        // 측정된 값 저장 
        static Point LeftRef;
        static Point RightRef;

        public double Pixel;
        public double ActualSize;
        public double Pixel2Acutalsize;

        private void SetMeasurement(object sender, RoutedEventArgs e)
        {
            String temp = TeethActualSize.Value.ToString("N2");
            Double xdf = RefPointR.X - RefPointL.X;
            Double ydf = RefPointR.Y - RefPointL.Y;
            Double length = Math.Sqrt(Math.Pow(xdf, 2) + Math.Pow(ydf, 2));

            Pixel = length;
            double.TryParse(temp, out ActualSize);
            Pixel2Acutalsize = ActualSize / Pixel;
        }

        // 픽셀당 길이를 측정한 비율 반환
        public double GetMeasurement()
        {
            return Pixel2Acutalsize;
        }

        // Checkbox checked event
        private void TeethPointSelect_Checked(object sender, RoutedEventArgs e)
        {
            ((FaceAlign_PageViewModel)(((FaceAlign_Page)(Application.Current.MainWindow.Content)).DataContext)).refclicked = true;
        }
    }
}
