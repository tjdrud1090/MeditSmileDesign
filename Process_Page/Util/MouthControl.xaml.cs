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

namespace Process_Page.Util
{
    /// <summary>
    /// MouthControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MouthControl : UserControl
    {
        public MouthControl()
        {
            InitializeComponent();
        }

        #region Mouth Points
        public IEnumerable MouthPoint
        {
            get { return (IEnumerable)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("MouthPoint", typeof(IEnumerable), typeof(MouthControl), new PropertyMetadata(null, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var draw = d as MouthControl;
            if (draw == null) return;

            if (e.NewValue is INotifyCollectionChanged)
            {
                (e.NewValue as INotifyCollectionChanged).CollectionChanged += draw.OnPointCollectionChanged;
                draw.RegisterCollectionItemPropertyChanged(e.NewValue as IEnumerable);
            }

            if (e.OldValue is INotifyCollectionChanged)
            {
                (e.OldValue as INotifyCollectionChanged).CollectionChanged -= draw.OnPointCollectionChanged;
                draw.UnRegisterCollectionItemPropertyChanged(e.OldValue as IEnumerable);
            }

            if (e.NewValue != null)
                draw.SetPathData();
        }
        #endregion

        #region PathColor
        public Brush PathColor
        {
            get { return (Brush)GetValue(PathColorProperty); }
            set { SetValue(PathColorProperty, value); }
        }

        public static readonly DependencyProperty PathColorProperty =
            DependencyProperty.Register("PathColor", typeof(Brush), typeof(MouthControl), new PropertyMetadata(Brushes.MidnightBlue));

        #endregion

        // Draw BezierCurve
        private void SetPathData()
        {
            if (MouthPoint == null)
                return;
            var points = new List<Point>();
            foreach (var point in MouthPoint)
            {
                var pointProperties = point.GetType().GetProperties();
                if (pointProperties.All(p => p.Name != "X") || pointProperties.All(p => p.Name != "Y"))
                    continue;
                var x = (double)point.GetType().GetProperty("X").GetValue(point, new object[] { });
                var y = (double)point.GetType().GetProperty("Y").GetValue(point, new object[] { });
                points.Add(new Point(x, y));
            }

            if (points.Count <= 1) return;

            var Mouth_PathFigure = new PathFigure { StartPoint = points.FirstOrDefault() };
            var Mouth_SegmentCollection = new PathSegmentCollection();
            var bezierSegments = InterpolationUtils.InterpolatePointWithBezierCurves(points, true);
            if (bezierSegments == null || bezierSegments.Count < 1)
            {
                foreach (var point in points.GetRange(1, points.Count - 1))
                {
                    var lineSegment = new LineSegment { Point = point };
                    Mouth_SegmentCollection.Add(lineSegment);
                }
            }
            else
            {
                foreach (var curveSeg in bezierSegments)
                {
                    var segment = new BezierSegment
                    {
                        Point1 = curveSeg.FirstControlPoint,
                        Point2 = curveSeg.SecondControlPoint,
                        Point3 = curveSeg.EndPoint
                    };
                    Mouth_SegmentCollection.Add(segment);
                }
            }

            Mouth_PathFigure.Segments = Mouth_SegmentCollection;
            var Mouth_PathFigureCollection = new PathFigureCollection { Mouth_PathFigure };
            var Mouth_PathGeometry = new PathGeometry { Figures = Mouth_PathFigureCollection };

            DrawLine.Data = Mouth_PathGeometry;
        }

        #region PropertyChanged

        private void RegisterCollectionItemPropertyChanged(IEnumerable collection)
        {
            if (collection == null) return;
            foreach (INotifyPropertyChanged point in collection)
                point.PropertyChanged += OnPointPropertyChanged;
        }

        private void UnRegisterCollectionItemPropertyChanged(IEnumerable collection)
        {
            if (collection == null) return;
            foreach (INotifyPropertyChanged point in collection)
                point.PropertyChanged -= OnPointPropertyChanged;
        }

        private void OnPointCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RegisterCollectionItemPropertyChanged(e.NewItems);
            UnRegisterCollectionItemPropertyChanged(e.OldItems);
            SetPathData();
        }

        private void OnPointPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "X" || e.PropertyName == "Y")
                SetPathData();
        }

        #endregion  
    }
}
