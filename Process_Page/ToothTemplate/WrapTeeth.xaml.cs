using Process_Page.ToothTemplate.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
    /// WrapTeeth.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WrapTeeth : UserControl
    {
        public WrapTeeth()
        {
            InitializeComponent();
        }

        #region Points

        public IEnumerable Points
        {
            get { return (IEnumerable)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Points. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointsProperty = DependencyProperty.Register(
            "Points", typeof(IEnumerable), typeof(WrapTeeth), new PropertyMetadata(null, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wrapPoints = d as WrapTeeth;
            if (wrapPoints == null)
                return;

            if (e.NewValue is INotifyCollectionChanged)
            {
                (e.NewValue as INotifyCollectionChanged).CollectionChanged += wrapPoints.OnPointCollectionChanged;
                wrapPoints.RegisterCollectionItemPropertyChanged(e.NewValue as IEnumerable);
            }

            if (e.OldValue is INotifyCollectionChanged)
            {
                (e.OldValue as INotifyCollectionChanged).CollectionChanged -= wrapPoints.OnPointCollectionChanged;
                wrapPoints.UnRegisterCollectionItemPropertyChanged(e.OldValue as IEnumerable);
            }

            if (e.NewValue != null)
            {
                wrapPoints.SetWrapTeethRectAndLine();
            }
        }

        #endregion

        #region ShowLength

        public bool ShowLength
        {
            get { return (bool)GetValue(ShowLengthProperty); }
            set { SetValue(ShowLengthProperty, value); }
        }

        public static readonly DependencyProperty ShowLengthProperty
            = DependencyProperty.Register("ShowLength", typeof(bool), typeof(WrapTeeth),
                                          new PropertyMetadata(false, ShowLengthPropertyChangedCallback));

        private static void ShowLengthPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wrap = d as WrapTeeth;
            if (wrap == null)
                return;

            if (e.NewValue != null)
            {
                if (wrap.ShowLength == true)
                {
                    wrap.lineH.Visibility = Visibility.Visible;
                    wrap.lineV.Visibility = Visibility.Visible;
                    wrap.lengthH.Visibility = Visibility.Visible;
                    wrap.lengthV.Visibility = Visibility.Visible;
                    wrap.RatioHV.Visibility = Visibility.Visible;
                }
                else
                {
                    wrap.lineH.Visibility = Visibility.Hidden;
                    wrap.lineV.Visibility = Visibility.Hidden;
                    wrap.lengthH.Visibility = Visibility.Hidden;
                    wrap.lengthV.Visibility = Visibility.Hidden;
                    wrap.RatioHV.Visibility = Visibility.Hidden;
                }
            }
        }

        #endregion

        #region OpacitySlider

        private static readonly DependencyProperty OpacitySliderProperty =
            DependencyProperty.Register("OpacitySlider", typeof(double), typeof(WrapTeeth));

        public double OpacitySlider
        {
            get { return (double)GetValue(OpacitySliderProperty); }
            set { SetValue(OpacitySliderProperty, value); }
        }

        #endregion

        void SetWrapTeethRectAndLine()
        {
            if (Points == null) return;
            var points = new List<Point>();

            Border_WrapTeeth.Visibility = Visibility.Visible;
            Rectangle_WrapTeeth.Visibility = Visibility.Visible;

            foreach (var point in Points)
            {
                var pointProperties = point.GetType().GetProperties();
                if (pointProperties.All(p => p.Name != "X") ||
                pointProperties.All(p => p.Name != "Y"))
                    continue;
                var x = (double)point.GetType().GetProperty("X").GetValue(point, new object[] { });
                var y = (double)point.GetType().GetProperty("Y").GetValue(point, new object[] { });
                points.Add(new Point(x, y));
            }

            if (points.Count <= 1)
                return;

            Point minP = new Point(Numerics.GetMinX_Teeth(points).X, Numerics.GetMinY_Teeth(points).Y);
            Point maxP = new Point(Numerics.GetMaxX_Teeth(points).X, Numerics.GetMaxY_Teeth(points).Y);

            DrawRect(minP, maxP);
            DrawLineXY(minP, maxP);
        }

        #region PropertyChanged
        private void RegisterCollectionItemPropertyChanged(IEnumerable collection)
        {
            if (collection == null)
                return;
            foreach (INotifyPropertyChanged point in collection)
                point.PropertyChanged += OnPointPropertyChanged;
        }

        private void UnRegisterCollectionItemPropertyChanged(IEnumerable collection)
        {
            if (collection == null)
                return;
            foreach (INotifyPropertyChanged point in collection)
                point.PropertyChanged -= OnPointPropertyChanged;
        }

        private void OnPointCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RegisterCollectionItemPropertyChanged(e.NewItems);
            UnRegisterCollectionItemPropertyChanged(e.OldItems);

            SetWrapTeethRectAndLine();
        }

        private void OnPointPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "X" || e.PropertyName == "Y")
                SetWrapTeethRectAndLine();
        }
        #endregion

        #region DrawRect

        public double Top;
        public double Left;

        readonly double padding = 8;
        private void DrawRect(Point min, Point max)
        {
            // Grid의 크기 설정
            Rectangle_WrapTeeth.Height = max.Y - min.Y + padding;
            Rectangle_WrapTeeth.Width = max.X - min.X + padding;

            Top = min.Y - padding / 2;
            Left = min.X - padding / 2;

            Canvas.SetLeft(this, Left);
            Canvas.SetTop(this, Top);

            //Canvas cv = this.Parent as Canvas;
            //Slider slider = null;
            //foreach (var ctrl in cv.Children)
            //{
            //    if (ctrl is Slider)
            //    {
            //        slider = ctrl as Slider;
            //        break;
            //    }
            //}

            //if (slider != null)
            //{
            //    Canvas.SetLeft(slider, Left);
            //    Canvas.SetTop(slider, Top);
            //}
        }

        #endregion

        #region DrawLine

        private void DrawLineXY(Point min, Point max)
        {
            double widthRect = max.X - min.X;
            double heightRect = max.Y - min.Y;
            Point startHorizontal = new Point(min.X, min.Y + heightRect / 2);
            Point endHorizontal = new Point(max.X, min.Y + heightRect / 2);
            Point startVertical = new Point(min.X + widthRect / 2, min.Y);
            Point endVertical = new Point(min.X + widthRect / 2, max.Y);

            // point자체는 전체 canvas에 대한 좌표이므로 각 WrapTeeth를 감싸는 Grid에 대한 좌표로 변환
            lineH.X1 = startHorizontal.X - Left;
            lineH.Y1 = startHorizontal.Y - Top;
            lineH.X2 = endHorizontal.X - Left;
            lineH.Y2 = endHorizontal.Y - Top;

            lineV.X1 = startVertical.X - Left;
            lineV.Y1 = startVertical.Y - Top;
            lineV.X2 = endVertical.X - Left;
            lineV.Y2 = endVertical.Y - Top;

            // infomation of length
            double leftH, topH, leftV, topV;
            double padding = 10;

            // for length of Horizontal Line
            lengthH.Content = widthRect.ToString("N2");
            leftH = lineH.X1 + widthRect / 2 + padding / 2;
            topH = lineH.Y1 + padding / 2;

            Canvas.SetLeft(lengthH, leftH);
            Canvas.SetTop(lengthH, topH);

            // for length of Vertical Line
            lengthV.Content = heightRect.ToString("N2");
            leftV = lineV.X1 + padding / 2;
            topV = lineV.Y1 + heightRect / 2 - (padding * 3);

            Canvas.SetLeft(lengthV, leftV);
            Canvas.SetTop(lengthV, topV);

            var ratio = (widthRect / heightRect) * 100;
            RatioHV.Content = ratio.ToString("N0") + "%";
            var leftRatio = min.X - Left + (widthRect / 8);
            var topRatio = min.Y - Top + (heightRect / 5);
            Canvas.SetLeft(RatioHV, leftRatio);
            Canvas.SetTop(RatioHV, topRatio);
        }

        #endregion
    }
}
