using Process_Page.ToothTemplate.Utils;
using Process_Page.ViewModel;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Process_Page.ToothTemplate 
{
    using TeethType = ObservableCollection<PointViewModel>;

    public partial class UpperTooth : UserControl
    {
        public UpperTooth()
        {
            InitializeComponent();

            SmileLineUp.Visibility = Visibility.Hidden;
            SmileLineDown.Visibility = Visibility.Hidden;

            UpSmile_Left.Visibility = Visibility.Hidden;
            UpSmile_Right.Visibility = Visibility.Hidden;
            DownSmile_Left.Visibility = Visibility.Hidden;
            DownSmile_Right.Visibility = Visibility.Hidden;

        }

        #region Points

        public IEnumerable Tooth_Points
        {
            get { return (IEnumerable)GetValue(ToothDataProperty); }
            set { SetValue(ToothDataProperty, value); }
        }

        public static readonly DependencyProperty ToothDataProperty
                = DependencyProperty.Register("Tooth_Points", typeof(IEnumerable), typeof(UpperTooth), new PropertyMetadata(ToothDataPropertyChanged));

        private static void ToothDataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UpperTooth upper = d as UpperTooth;
            if (upper == null) return;

            if (e.NewValue is INotifyCollectionChanged)
            {
                (e.NewValue as INotifyCollectionChanged).CollectionChanged += upper.OnPointCollectionChanged;
                upper.RegisterCollectionItemPropertyChanged(e.NewValue as IEnumerable);
            }

            if (e.OldValue is INotifyCollectionChanged)
            {
                (e.OldValue as INotifyCollectionChanged).CollectionChanged -= upper.OnPointCollectionChanged;
                upper.UnRegisterCollectionItemPropertyChanged(e.OldValue as IEnumerable);
            }

            if (e.NewValue == null) return;

            upper.DrawMover();
            upper.DrawHoHoLine();
        }       

        #region PropertyChanged

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

            if (e.OldValue is INotifyCollectionChanged)
            {
                (e.OldValue as INotifyCollectionChanged).CollectionChanged -= upper.OnPointCollectionChanged;
                upper.UnRegisterCollectionItemPropertyChanged(e.OldValue as IEnumerable);
            }

            if (e.NewValue == null) return;

            upper.DrawMover();
            upper.DrawSmileLine();
        }       

        #region NotifyPropertyChanged

        private void RegisterCollectionItemPropertyChanged(IEnumerable collection)
        {
            if (collection == null) return;
            foreach (TeethType points in collection)
                foreach (INotifyPropertyChanged point in points)
                    point.PropertyChanged += OnPointPropertyChanged;
        }

        private void UnRegisterCollectionItemPropertyChanged(IEnumerable collection)
        {
            if (collection == null) return;
            foreach (TeethType points in collection)
                foreach (INotifyPropertyChanged point in points)
                    point.PropertyChanged -= OnPointPropertyChanged;
        }

        private void OnPointCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RegisterCollectionItemPropertyChanged(e.NewItems);
            UnRegisterCollectionItemPropertyChanged(e.OldItems);
            DrawMover();
            DrawSmileLine();
        }

        private void OnPointPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "X" || e.PropertyName == "Y") { 
                DrawMover();
                DrawSmileLine();
            }
        }

        #endregion

        #endregion

        #region ShowLengths

        public static readonly DependencyProperty ShowLengthsProperty =
            DependencyProperty.Register("ShowLengths", typeof(bool), typeof(UpperTooth));

        public bool ShowLengths
        {
            get { return (bool)GetValue(ShowLengthsProperty); }
            set { SetValue(ShowLengthsProperty, value); }
        }

        #endregion

        #region Fill
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(bool), typeof(UpperTooth));

        public bool Fill
        {
            get { return (bool)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        #endregion

        private void DrawMover()
        {
            Point min = Numerics.GetMinXY_Tooth(Tooth_Points);
            Point max = Numerics.GetMaxXY_Tooth(Tooth_Points);

            double left = (min.X + max.X) / 2 - Mover.Width / 2;
            double top = (min.Y) - (-min.Y + max.Y) / 2 - Mover.Height;

            Canvas.SetLeft(Mover, left);
            Canvas.SetTop(Mover, top);
            Mover.Visibility = Visibility.Visible;
        }

        private void DrawSmileLine()
        {
            // SmileLine
            Point MaxPoint = Numerics.GetMaxXY_Tooth(Tooth_Points);
            Point MinPoint = Numerics.GetMinXY_Tooth(Tooth_Points);

            double unitDistance = (MaxPoint.Y - MinPoint.Y) / 3;

            DownPathFigure.StartPoint = new Point(MinPoint.X - 2 * unitDistance, MaxPoint.Y - unitDistance);
            DownArcSegment.Point = new Point(MaxPoint.X + 2 * unitDistance, MaxPoint.Y - unitDistance);

            UpPathFigure.StartPoint = new Point(MinPoint.X - 2 * unitDistance, MinPoint.Y - unitDistance);
            UpArcSegment.Point = new Point(MaxPoint.X + 2 * unitDistance, MinPoint.Y - unitDistance);

            double temp = 2 * Math.Abs(MinPoint.X / 2 + MaxPoint.X / 2 - DownPathFigure.StartPoint.X) / 1.7320508075688772935;//3^0.5
            DownArcSegment.Size = new Size(1.5 * temp, temp);
            UpArcSegment.Size = new Size(1.5 * temp, temp);

            // SmileControl
            //double x, y, q;
            LeftSmileControl_up.Center = UpPathFigure.StartPoint;
            //x = (UpPathFigure.StartPoint.X + UpArcSegment.Point.X) / 2;
            //q = (UpArcSegment.Point.X - UpPathFigure.StartPoint.X) / 1.5;
            //y = q / 2;
            //MidSmileControl_up.Center = new Point(x, y);
            RightSmileControl_up.Center = UpArcSegment.Point;

            LeftSmileControl_down.Center = DownPathFigure.StartPoint;
            //x = (DownPathFigure.StartPoint.X + DownArcSegment.Point.X) / 2;
            //q = (DownArcSegment.Point.X - DownPathFigure.StartPoint.X) / 1.5;
            //y = DownPathFigure.StartPoint.Y + q / 2;
            //MidSmileControl_down.Center = new Point(x, y);
            RightSmileControl_down.Center = DownArcSegment.Point;
        }
    }
}
