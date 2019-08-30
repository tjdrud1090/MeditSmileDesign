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
using Process_Page.ToothTemplate.Utils;
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
        }

        #region image property
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

        //Transform Center
        public static readonly DependencyProperty CenterProperty
        = DependencyProperty.Register("TransformCenter", typeof(Point), typeof(ImageCanvas));

        public Point TransformCenter
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
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
        #endregion

        #region Mouth Points Property

        #region Mouth Points
        public IEnumerable MouthPoint
        {
            get { return (IEnumerable)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("MouthPoint", typeof(IEnumerable), typeof(ImageCanvas), new PropertyMetadata(null, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var draw = d as ImageCanvas;
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
            DependencyProperty.Register("PathColor", typeof(Brush), typeof(ImageCanvas), new PropertyMetadata(Brushes.MidnightBlue));

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
        #endregion
    }
}
