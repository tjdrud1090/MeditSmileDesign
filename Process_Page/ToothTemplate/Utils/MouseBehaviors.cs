using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Process_Page.ToothTemplate.Utils
{
    class MouseBehaviors
    {
        #region MouseUp

        public static readonly DependencyProperty MouseUpCommandProperty =
            DependencyProperty.RegisterAttached("MouseUpCommand", typeof(ICommand), typeof(MouseBehaviors), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseUpCommandChanged)));

        private static void MouseUpCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;
            element.MouseUp += element_MouseUp;
        }

        static void element_MouseUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            ICommand command = GetMouseUpCommand(element);
            command.Execute(e);
        }

        public static void SetMouseUpCommand(UIElement element, ICommand value)
        {
            element.SetValue(MouseUpCommandProperty, value);
        }

        public static ICommand GetMouseUpCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseUpCommandProperty);
        }

        #endregion

        #region MouseDown

        public static readonly DependencyProperty MouseDownCommandProperty =
            DependencyProperty.RegisterAttached("MouseDownCommand", typeof(ICommand), typeof(MouseBehaviors),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseDownCommandChanged)));

        private static void MouseDownCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;
            element.MouseDown += element_MouseDown;
        }

        static void element_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            ICommand command = GetMouseDownCommand(element);
            command.Execute(e);
        }

        public static void SetMouseDownCommand(UIElement element, ICommand value)
        {
            element.SetValue(MouseDownCommandProperty, value);
        }

        public static ICommand GetMouseDownCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseDownCommandProperty);
        }

        #endregion

        #region MouseLeave

        public static readonly DependencyProperty MouseLeaveCommandProperty =
            DependencyProperty.RegisterAttached("MouseLeaveCommand", typeof(ICommand), typeof(MouseBehaviors), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseLeaveCommandChanged)));

        private static void MouseLeaveCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;
            element.MouseLeave += new MouseEventHandler(element_MouseLeave);
        }

        static void element_MouseLeave(object sender, MouseEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            ICommand command = GetMouseLeaveCommand(element);
            command.Execute(e);
        }

        public static void SetMouseLeaveCommand(UIElement element, ICommand value)
        {
            element.SetValue(MouseLeaveCommandProperty, value);
        }

        public static ICommand GetMouseLeaveCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseLeaveCommandProperty);
        }

        #endregion

        #region MouseLeftButtonDown

        public static ICommand GetMouseLeftButtonDownCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseLeftButtonDownCommandProperty);
        }

        public static void SetMouseLeftButtonDownCommand(UIElement element, ICommand value)
        {
            element.SetValue(MouseLeftButtonDownCommandProperty, value);
        }

        public static readonly DependencyProperty MouseLeftButtonDownCommandProperty =
            DependencyProperty.RegisterAttached("MouseLeftButtonDownCommand", typeof(ICommand), typeof(MouseBehaviors), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseLeftButtonDownCommandChanged)));

        private static void MouseLeftButtonDownCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;
            element.MouseLeftButtonDown += element_MouseLeftButtonDown;
        }

        static void element_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            ICommand command = GetMouseLeftButtonDownCommand(element);
            command.Execute(e);
        }

        #endregion

        #region MouseLeftButtonUp

        public static readonly DependencyProperty MouseLeftButtonUpCommandProperty =
            DependencyProperty.RegisterAttached("MouseLeftButtonUpCommand", typeof(ICommand), typeof(MouseBehaviors), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseLeftButtonUpCommandChanged)));

        private static void MouseLeftButtonUpCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;
            element.MouseLeftButtonUp += element_MouseLeftButtonUp;
        }

        static void element_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            ICommand command = GetMouseLeftButtonUpCommand(element);
            command.Execute(e);
        }

        public static void SetMouseLeftButtonUpCommand(UIElement element, ICommand value)
        {
            element.SetValue(MouseLeftButtonUpCommandProperty, value);
        }

        public static ICommand GetMouseLeftButtonUpCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseLeftButtonUpCommandProperty);
        }

        #endregion

        #region MouseMove

        public static readonly DependencyProperty MouseMoveCommandProperty =
            DependencyProperty.RegisterAttached("MouseMoveCommand", typeof(ICommand), typeof(MouseBehaviors), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseMoveCommandChanged)));

        private static void MouseMoveCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;
            element.MouseMove += new MouseEventHandler(element_MouseMove);
        }

        static void element_MouseMove(object sender, MouseEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            ICommand command = GetMouseMoveCommand(element);
            command.Execute(e);
        }

        public static void SetMouseMoveCommand(UIElement element, ICommand value)
        {
            element.SetValue(MouseMoveCommandProperty, value);
        }

        public static ICommand GetMouseMoveCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseMoveCommandProperty);
        }

        #endregion

        #region MouseRightButtonDown

        public static readonly DependencyProperty MouseRightButtonDownCommandProperty =
            DependencyProperty.RegisterAttached("MouseRightButtonDownCommand", typeof(ICommand), typeof(MouseBehaviors), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseRightButtonDownCommandChanged)));

        private static void MouseRightButtonDownCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;
            element.MouseRightButtonDown += element_MouseRightButtonDown;
        }

        static void element_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            ICommand command = GetMouseRightButtonDownCommand(element);
            command.Execute(e);
        }

        public static void SetMouseRightButtonDownCommand(UIElement element, ICommand value)
        {
            element.SetValue(MouseRightButtonDownCommandProperty, value);
        }

        public static ICommand GetMouseRightButtonDownCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseRightButtonDownCommandProperty);
        }

        #endregion

        #region MouseRightButtonUp

        public static readonly DependencyProperty MouseRightButtonUpCommandProperty =
            DependencyProperty.RegisterAttached("MouseRightButtonUpCommand", typeof(ICommand), typeof(MouseBehaviors), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseRightButtonUpCommandChanged)));

        private static void MouseRightButtonUpCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;
            element.MouseRightButtonUp += element_MouseRightButtonUp;
        }

        static void element_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            ICommand command = GetMouseRightButtonUpCommand(element);
            command.Execute(e);
        }

        public static void SetMouseRightButtonUpCommand(UIElement element, ICommand value)
        {
            element.SetValue(MouseRightButtonUpCommandProperty, value);
        }

        public static ICommand GetMouseRightButtonUpCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseRightButtonUpCommandProperty);
        }

        #endregion

        #region MouseWheel

        public static readonly DependencyProperty MouseWheelCommandProperty =
            DependencyProperty.RegisterAttached("MouseWheelCommand", typeof(ICommand), typeof(MouseBehaviors), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseWheelCommandChanged)));

        private static void MouseWheelCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;

            element.MouseWheel += new MouseWheelEventHandler(element_MouseWheel);
        }

        static void element_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;

            ICommand command = GetMouseWheelCommand(element);

            command.Execute(e);
        }

        public static void SetMouseWheelCommand(UIElement element, ICommand value)
        {
            element.SetValue(MouseWheelCommandProperty, value);
        }

        public static ICommand GetMouseWheelCommand(UIElement element)
        {
            return (ICommand)element.GetValue(MouseWheelCommandProperty);
        }

        #endregion

        #region MouseLeftButtonDoubleDown

        //public static readonly DependencyProperty MouseLeftButtonDoubleDownProperty =
        //    DependencyProperty.RegisterAttached("MouseLeftButtonDoubleDownProperty", typeof(ICommand), typeof(MouseBehaviors), new FrameworkPropertyMetadata(new PropertyChangedCallback(MouseLeftButtonDoubleDownCommandChanged)));

        //private static void MouseLeftButtonDownCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    UIElement element = d as UIElement;
        //    element.
        //}

        #endregion

    }
}
