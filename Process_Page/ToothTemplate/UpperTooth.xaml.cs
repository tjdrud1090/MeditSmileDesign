using Process_Page.ToothTemplate.Utils;
using Process_Page.ViewModel;
using System;
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
    using TeethType = ObservableCollection<PointViewModel>;

    public partial class UpperTooth : UserControl
    {
        public UpperTooth()
        {
            InitializeComponent();
        }

        #region Points

        public IEnumerable Tooth_Points
        {
            get { return (IEnumerable)GetValue(ToothDataProperty); }
            set { SetValue(ToothDataProperty, value); }
        }

        public static readonly DependencyProperty ToothDataProperty
                = DependencyProperty.Register("Tooth_Points", typeof(IEnumerable), typeof(UpperTooth), new PropertyMetadata(null, ToothDataPropertyChanged));

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
        }

        private void OnPointCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RegisterCollectionItemPropertyChanged(e.NewItems);
            UnRegisterCollectionItemPropertyChanged(e.OldItems);

            DrawMover();
        }

        private void OnPointPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "X" || e.PropertyName == "Y")
                DrawMover();
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

            double left = (min.X + max.X) / 2 - MoveTop.Width / 2;
            double top = (min.Y) - (-min.Y + max.Y) / 2 - MoveTop.Height;

            Canvas.SetLeft(MoveTop, left);
            Canvas.SetTop(MoveTop, top);
            MoveTop.Visibility = Visibility.Visible;
        }
    }
}
