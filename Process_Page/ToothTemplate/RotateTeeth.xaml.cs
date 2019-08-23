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
    /// RotateTeeth.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RotateTeeth : UserControl
    {
        public RotateTeeth()
        {
            InitializeComponent();
        }

        #region Points

        public IEnumerable Points
        {
            get { return (IEnumerable)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(IEnumerable), typeof(RotateTeeth), new PropertyMetadata(null, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rotate = d as RotateTeeth;
            if (rotate == null) return;

            if (e.NewValue is INotifyCollectionChanged)
            {
                (e.NewValue as INotifyCollectionChanged).CollectionChanged += rotate.OnPointCollectionChanged;
                rotate.RegisterCollectionItemPropertyChanged(e.NewValue as IEnumerable);
            }

            if (e.OldValue is INotifyCollectionChanged)
            {
                (e.OldValue as INotifyCollectionChanged).CollectionChanged -= rotate.OnPointCollectionChanged;
                rotate.UnRegisterCollectionItemPropertyChanged(e.OldValue as IEnumerable);
            }

            if (e.NewValue != null)
                rotate.SetPathData();
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

        private void SetPathData()
        {
            if (Points == null)
                return;

            var points = new List<Point>();
            foreach (var point in Points)
            {
                var pointProperties = point.GetType().GetProperties();
                if (pointProperties.All(p => p.Name != "X") || pointProperties.All(p => p.Name != "Y"))
                    continue;
                var x = (double)point.GetType().GetProperty("X").GetValue(point, new object[] { });
                var y = (double)point.GetType().GetProperty("Y").GetValue(point, new object[] { });
                points.Add(new Point(x, y));
            }

            if (points.Count <= 1)
                return;

            SetPin(points);
        }

        double Left, Top;
        private void SetPin(List<Point> points)
        {
            Point min = new Point(Numerics.GetMinX_Teeth(points).X, Numerics.GetMinY_Teeth(points).Y);
            Point max = new Point(Numerics.GetMaxX_Teeth(points).X, Numerics.GetMaxY_Teeth(points).Y);

            Left = (max.X + min.X) / 2;
            Top = min.Y - ((max.Y - min.Y) / 4);
            Canvas.SetLeft(RotatePin, Left);
            Canvas.SetTop(RotatePin, Top);
        }
    }
}
