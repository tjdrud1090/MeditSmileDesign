using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Process_Page.Util
{
    /// <summary>
    /// NumericUpDown.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public NumericUpDown()
        {
            InitializeComponent();
            Measurement.Text = Value.ToString();
        }

        public readonly static DependencyProperty MaximumProperty;
        public readonly static DependencyProperty MinimumProperty;
        public readonly static DependencyProperty ValueProperty;
        public readonly static DependencyProperty StepProperty;

        static NumericUpDown()
        {
            MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(NumericUpDown));
            MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(NumericUpDown));
            StepProperty = DependencyProperty.Register("StepValue", typeof(double), typeof(NumericUpDown));
            ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnTextChangePropertyChanged)));
        }

        private static void OnTextChangePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown uservaluecontrol = d as NumericUpDown;
            uservaluecontrol.Measurement.Text = ((double)e.NewValue).ToString("N2");
        }

        #region DpAccessior
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetCurrentValue(ValueProperty, value); }
        }
        public double StepValue
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        #endregion

        void _DownButton_Click(object sender, RoutedEventArgs e)
        {
            if (Value > Minimum)
            {
                Value -= StepValue;
                if (Value < Minimum)
                    Value = Minimum;
            }
        }

        void _UpButton_Click(object sender, RoutedEventArgs e)
        {
            if (Value < Maximum)
            {
                Value += StepValue;
                if (Value > Maximum)
                    Value = Maximum;
            }
        }

        private void Measurement_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            string value = textBox.Text;
            double result = 0;

            if (double.TryParse(value, out result) == false)
            {
                Measurement.Text = Value.ToString("N2");
                return;
            }
            if(result < Maximum && result > Minimum)
            {
                value = result.ToString("N2");
                result = double.Parse(value);

                Value = result;
            }
            else
            {
                Measurement.Text = Value.ToString("N2");
            }
        }

        private void Numeric_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                FocusManager.SetFocusedElement(FocusManager.GetFocusScope(Measurement), null);
        }
    }
}
