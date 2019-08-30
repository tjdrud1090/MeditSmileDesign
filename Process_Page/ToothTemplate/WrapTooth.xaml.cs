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
          //DrawSmileLine(pointses);
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

            //MoveTop.Margin = new Thickness(Left + Border_WrapTooth.Width/2, Top - 40, 0, 0);
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

        #region SmileLine 
        private void DrawJoseLine(List<List<Point>> all)
        {
            Point MaxPoint = Numerics.GetMaxXY_Tooth(Points);
            Point MinPoint = Numerics.GetMinXY_Tooth(Points);
            double unitDistance = (MaxPoint.Y - MinPoint.Y) / 2;
            Point StartPoint =new Point(MinPoint.X-unitDistance,MaxPoint.Y/2+MinPoint.Y/2);
            Point EndPoint =new Point(MaxPoint.X+unitDistance,MaxPoint.Y/2+MinPoint.Y/2);

            double temp = (MinPoint.X / 2 + MaxPoint.X / 2 - StartPoint.X) / 1.7320508075688772935;//3^0.5
            Size size = new Size(2 * temp, 2 * temp);
            ArcSegment arcSegment = new ArcSegment(EndPoint,size,0,false,SweepDirection.Counterclockwise,true);

        }

        private void DrawSmileLine(List<List<Point>> all)
        {



            Point Left2 = Numerics.GetMinX_Teeth(all[5]);
            Point Left1 = Numerics.GetMaxY_Teeth(all[4]);

            Point Mid = Numerics.GetMaxXY_Tooth(Points);

            Point Right1 = Numerics.GetMaxY_Teeth(all[1]);
            Point Right2 = Numerics.GetMaxX_Teeth(all[2]);

            double padding = 30;
            Point LeftCont = new Point(Left2.X - Left - padding, Border_WrapTooth.Height / 2);
            Point MidCont = new Point(Border_WrapTooth.Width / 2, Mid.Y - Top + 5);
            Point RightCont = new Point(Right2.X - Left + padding, Border_WrapTooth.Height / 2);

            // Make a list of Control Points.
            List<Point> list = new List<Point>();
            list.Add(LeftCont);
            list.Add(MidCont);
            list.Add(RightCont);

            // Draw Control Points
            int adj = 7;
            Canvas.SetLeft(LeftSmileControl, LeftCont.X - adj);
            Canvas.SetTop(LeftSmileControl, LeftCont.Y - adj);
            LeftSmileControl.Visibility = Visibility.Visible;

            MidSmileControl.Visibility = Visibility.Visible;
            Canvas.SetLeft(MidSmileControl, MidCont.X - adj);
            Canvas.SetTop(MidSmileControl, MidCont.Y - adj);

            RightSmileControl.Visibility = Visibility.Visible;
            Canvas.SetLeft(RightSmileControl, RightCont.X - adj);
            Canvas.SetTop(RightSmileControl, RightCont.Y - adj);

            // Draw SmileLine.
            var smile_geometry = new PathGeometry();
            var smile_pathfigureCollection = new PathFigureCollection();
            var path_figure = new PathFigure();
            path_figure.StartPoint = LeftCont;
            var path_segmentCollection = new PathSegmentCollection();

            int pad = 20;
            PointCollection pc = new PointCollection();
            pc.Add(new Point(Left1.X - Left, Left1.Y - Top + pad));
            pc.Add(MidCont);
            pc.Add(new Point(Right1.X - Left, Right1.Y - Top + pad));
            pc.Add(RightCont);
            var segment = new PolyQuadraticBezierSegment()
            {
                Points = pc
            };
            path_segmentCollection.Add(segment);
            path_figure.Segments = path_segmentCollection;
            smile_pathfigureCollection.Add(path_figure);
            smile_geometry.Figures = smile_pathfigureCollection;
            SmileLine.Data = smile_geometry;
        }

        #endregion
    }
}
