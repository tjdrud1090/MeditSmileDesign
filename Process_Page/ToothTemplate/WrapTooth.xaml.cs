using Process_Page.ToothTemplate.Utils;
using Process_Page.ViewModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// WrapTooth.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    using TeethType = ObservableCollection<PointViewModel>;

    public partial class WrapTooth : UserControl
    {
        public WrapTooth()
        {
            InitializeComponent();
            this.DataContext=this;
            fillImgName = "color3";
        }

        #region Points

        public IEnumerable Points
        {
            get { return (IEnumerable)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Points. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointsProperty = DependencyProperty.Register(
            "Points", typeof(IEnumerable), typeof(WrapTooth), new PropertyMetadata(null, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wrap = d as WrapTooth;
            if (wrap == null)
                return;

            if (e.NewValue is INotifyCollectionChanged)
            {
                (e.NewValue as INotifyCollectionChanged).CollectionChanged += wrap.OnPointCollectionChanged;
                wrap.RegisterCollectionItemPropertyChanged(e.NewValue as IEnumerable);
            }

            if (e.OldValue is INotifyCollectionChanged)
            {
                (e.OldValue as INotifyCollectionChanged).CollectionChanged -= wrap.OnPointCollectionChanged;
                wrap.UnRegisterCollectionItemPropertyChanged(e.OldValue as IEnumerable);
            }

            if (e.NewValue != null)
            {
                Canvas cv = wrap.Parent as Canvas;
                if (cv == null)
                    return;
                foreach (FrameworkElement fr in cv.Children)
                    fr.Visibility = Visibility.Visible;

                wrap.SetWrapToothRect();
            }
            else
            {
                Canvas cv = wrap.Parent as Canvas;
                if (cv == null)
                    return;
                foreach (FrameworkElement fr in cv.Children)
                    fr.Visibility = Visibility.Hidden;
                return;
            }
        }

        #endregion

        #region Fill

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(bool), typeof(WrapTooth), new PropertyMetadata(false, FillPropertyChangedCallback));

        public bool Fill
        {
            get { return (bool)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        private string fillImgName;
        private static void FillPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wrap = d as WrapTooth;
            if (wrap == null)
                return;

            if (e.NewValue != null)
            {
                Canvas tooth = wrap.Parent as Canvas;
                Grid grid = tooth.FindName("GridInTooth") as Grid;
                foreach (Teeth teeth in grid.Children)
                {
                    DrawTeeth draw = teeth.FindName("drawTeeth") as DrawTeeth;
                    draw.path.Fill = wrap.Fill ? draw.FindResource(wrap.fillImgName) as ImageBrush : null;
                }
            }
        }

        #endregion

        void SetWrapToothRect()
        {
            if (Points == null)
                return;

            Border_WrapTooth.Visibility = Visibility.Visible;
            //MoveTop.Visibility = Visibility.Visible;

            var pointses = new List<List<Point>>();
            foreach (TeethType high in Points)
            {
                var points = new List<Point>();
                foreach (PointViewModel low in high)
                {
                    var pointProperties = low.GetType().GetProperties();
                    if (pointProperties.All(p => p.Name != "X") || pointProperties.All(p => p.Name != "Y"))
                        continue;
                    var x = (double)low.GetType().GetProperty("X").GetValue(low, new object[] { });
                    var y = (double)low.GetType().GetProperty("Y").GetValue(low, new object[] { });
                    points.Add(new Point(x, y));
                }
                pointses.Add(points);
            }

            if (pointses.Count <= 1)
                return;

            DrawRect();
            DrawTeethBetweenLine(pointses);
        }

        #region Rect

        public double Top;
        public double Left;
        readonly double padding = 10;

        private void DrawRect()
        {
            Point MinPoint = Numerics.GetMinXY_Tooth(Points);
            Point MaxPoint = Numerics.GetMaxXY_Tooth(Points);

            Border_WrapTooth.Width = MaxPoint.X - MinPoint.X + padding;
            Border_WrapTooth.Height = MaxPoint.Y - MinPoint.Y + padding;

            Left = MinPoint.X - padding / 2;
            Top = MinPoint.Y - padding / 2;

            Canvas.SetTop(this, Top);
            Canvas.SetLeft(this, Left);
        }

        #endregion

        #region TeethBetweenLine

        private void DrawTeethBetweenLine(List<List<Point>> points)
        {
            List<double> listX1 = new List<double>();

            double coorX;
            foreach (var teeth in points)
            {
                coorX = Numerics.GetMaxX_Teeth(teeth).X - Left;
                listX1.Add(coorX);
            }
            List<Point> lastTeeth = points[5];  // CanineL
            coorX = Numerics.GetMinX_Teeth(lastTeeth).X - Left;
            listX1.Add(coorX);

            int i = 0;
            foreach (var l in Grid_WrapTooth.Children)
            {
                if (l is Line)
                {
                    Line line = l as Line;
                    line.X1 = listX1[i];
                    line.Y1 = 0;
                    line.X2 = listX1[i++];
                    line.Y2 = Border_WrapTooth.Height;
                }
            }
        }

        #endregion

        #region NotifyPropertyChanged

        private void RegisterCollectionItemPropertyChanged(IEnumerable collection)
        {
            if (collection == null)
                return;
            foreach (TeethType points in collection)
            {
                foreach (INotifyPropertyChanged point in points)
                    point.PropertyChanged += OnPointPropertyChanged;
            }
        }

        private void UnRegisterCollectionItemPropertyChanged(IEnumerable collection)
        {
            if (collection == null)
                return;
            foreach (TeethType points in collection)
            {
                foreach (INotifyPropertyChanged point in points)
                    point.PropertyChanged -= OnPointPropertyChanged;
            }
        }

        private void OnPointCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RegisterCollectionItemPropertyChanged(e.NewItems);
            UnRegisterCollectionItemPropertyChanged(e.OldItems);
            SetWrapToothRect();
        }

        private void OnPointPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "X" || e.PropertyName == "Y")
                SetWrapToothRect();
        }

        #endregion
    }
}
