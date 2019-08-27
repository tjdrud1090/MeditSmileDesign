using Process_Page.ToothTemplate.Utils;
using Process_Page.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Process_Page.Cards;
using System.Collections.ObjectModel;

namespace Process_Page.ToothTemplate.AttachedProperties
{
    using TeethType = ObservableCollection<PointViewModel>;
    class DragInsideCanvasBehavior
    {
        const int MouseTimeDif = 20;

        #region DragInsideCanvas

        public static bool GetDragInsideCanvas(DependencyObject obj)
        {
            return (bool)obj.GetValue(DragInsideCanvasProperty);
        }

        public static void SetDragInsideCanvas(DependencyObject obj, bool value)
        {
            obj.SetValue(DragInsideCanvasProperty, value);
        }

        // Using a DependencyProperty as the backing store for DragInsideCanvasBehavior.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DragInsideCanvasProperty =
            DependencyProperty.RegisterAttached("DragInsideCanvas", typeof(bool),
                                                typeof(DragInsideCanvasBehavior), new PropertyMetadata(false, OnPropertyChanged));

        #region RegistredElements

        private static Dictionary<FrameworkElement, MyHandlersData> _registredElements;
        private static Dictionary<FrameworkElement, MyHandlersData> RegistredElements
        {
            get { return _registredElements ?? (_registredElements = new Dictionary<FrameworkElement, MyHandlersData>()); }
            set { _registredElements = value; }
        }

        #endregion

        // 각 point에 대한 Changed 함수
        private static void OnPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var frameworkElement = dependencyObject as FrameworkElement;
            if (frameworkElement == null)
                return;

            var canvas = ViewUtils.GetParent(frameworkElement, (t) => t is Canvas) as Canvas;

            //var canvas = ((SmileDesign_Page)(Application.Current.MainWindow.Content)).Image_Canvas as Canvas;
            if (canvas == null)
                return;

            var mousePosition = Mouse.GetPosition(canvas);
            int lastMouseMoveTime = Environment.TickCount;
            bool itemIsClicked = false;

            double lastMousePosX = -1;
            double lastMousePosY = -1;
            double x = 0;
            double y = 0;

            RoutedEventHandler mouseDown = RegistredElements.ContainsKey(frameworkElement) ? RegistredElements[frameworkElement].MouseDown : null;
            RoutedEventHandler mouseUp = RegistredElements.ContainsKey(frameworkElement) ? RegistredElements[frameworkElement].MouseUp : null;
            RoutedEventHandler mouseMove = RegistredElements.ContainsKey(frameworkElement) ? RegistredElements[frameworkElement].MouseMove : null;

            if ((bool)dependencyPropertyChangedEventArgs.NewValue)
            {

                if (!RegistredElements.ContainsKey(frameworkElement))
                {
                    RegistredElements.Add(frameworkElement, new MyHandlersData());
                    RegistredElements[frameworkElement].MouseDown = (_, __) =>
                    {
                        var mv = ((SmileDesign_Page)(Application.Current.MainWindow.Content)) as SmileDesign_Page;
                        if (mv.ToothControl.EditPoints.IsChecked == true)
                        {
                            // Remove
                            if (Mouse.RightButton == MouseButtonState.Pressed)
                            {
                                var listcanvas = VisualTreeHelper.GetParent(frameworkElement as DependencyObject) as Canvas;
                                var listbox = VisualTreeHelper.GetParent(listcanvas) as ListBox;
                                var target_teeth = listbox.ItemsSource as TeethType;

                                var delete_point = (frameworkElement as ListBoxItem).Content as PointViewModel;
                                if (target_teeth.Count > 3)
                                    target_teeth.Remove(delete_point);

                                int i = 0;
                                foreach (var point in target_teeth)
                                    point.I = i++;
                            }
                        }
                        // Move
                        itemIsClicked = true;

                        //always mouse down (start dragging), update the x,y 
                        x = (double)frameworkElement.GetValue(Canvas.LeftProperty);
                        y = (double)frameworkElement.GetValue(Canvas.TopProperty);

                        x = double.IsNaN(x) ? 0 : x;
                        y = double.IsNaN(y) ? 0 : y;

                        mousePosition = Mouse.GetPosition(canvas);
                        lastMousePosX = mousePosition.X;
                        lastMousePosY = mousePosition.Y;
                        Mouse.Capture(frameworkElement);
                    };

                    RegistredElements[frameworkElement].MouseUp = (_, __) =>
                    {
                        itemIsClicked = false;
                        Mouse.Capture(null);
                    };

                    RegistredElements[frameworkElement].MouseMove = (_, __) =>
                    {
                        if (itemIsClicked && ((Environment.TickCount - lastMouseMoveTime) > MouseTimeDif))
                        {
                            mousePosition = Mouse.GetPosition(canvas);
                            var containerHeight = (double)canvas.GetValue(FrameworkElement.ActualHeightProperty);
                            var containerWidth = (double)canvas.GetValue(FrameworkElement.ActualWidthProperty);
                            var mouseDiffX = mousePosition.X - lastMousePosX;
                            var mouseDiffY = mousePosition.Y - lastMousePosY;

                            Console.WriteLine("original framework element : " + x + ", " + y);

                            var main = ((SmileDesign_Page)(Application.Current.MainWindow.Content)) as SmileDesign_Page;
                            //x + mouseDiffX >= 0 && mousePosition.X >= 0 && 
                            //y + mouseDiffY >= 0 && mousePosition.Y >= 0 &&

                            if ((containerWidth <= 0 || (x + mouseDiffX <= containerWidth) && (mousePosition.X <= containerWidth)))
                            {
                                x = x + mouseDiffX;
                                frameworkElement.SetValue(Canvas.LeftProperty, x);

                                if (main.ToothControl.mirror.IsChecked == true)
                                {
                                    ListBoxItem sys = FindSymmetryPoint(frameworkElement);
                                    PointViewModel p_sys = sys.Content as PointViewModel;

                                    var mouseDiffX2 = -mouseDiffX;
                                    sys.SetValue(Canvas.LeftProperty, p_sys.X + mouseDiffX2);
                                }
                            }
                            if ( (containerHeight <= 0 || (y + mouseDiffY <= containerHeight && mousePosition.Y <= containerHeight)))
                            {
                                y = y + mouseDiffY;
                                frameworkElement.SetValue(Canvas.TopProperty, y);

                                if (main.ToothControl.mirror.IsChecked == true)
                                {
                                    ListBoxItem sys = FindSymmetryPoint(frameworkElement);
                                    PointViewModel p_sys = sys.Content as PointViewModel;
                                    sys.SetValue(Canvas.TopProperty, p_sys.Y + mouseDiffY);
                                }
                            }

                            Console.WriteLine("framework element : " + x + ", " + y);
                            Console.WriteLine("mouse Diff : " + mouseDiffX + ", " + mouseDiffY);
                            Console.WriteLine("Mouse Poisition : " + mousePosition);
                            Console.WriteLine();

                            lastMouseMoveTime = Environment.TickCount;
                            lastMousePosX = mousePosition.X;
                            lastMousePosY = mousePosition.Y;
                        }

                    };

                    frameworkElement.AddHandler(Mouse.MouseDownEvent, RegistredElements[frameworkElement].MouseDown, true);
                    frameworkElement.AddHandler(Mouse.MouseUpEvent, RegistredElements[frameworkElement].MouseUp, true);
                    frameworkElement.AddHandler(Mouse.MouseMoveEvent, RegistredElements[frameworkElement].MouseMove, false);
                }

                if (mouseDown != null)
                    frameworkElement.RemoveHandler(Mouse.MouseDownEvent, mouseDown);
                if (mouseUp != null)
                    frameworkElement.RemoveHandler(Mouse.MouseUpEvent, mouseUp);
                if (mouseMove != null)
                    frameworkElement.RemoveHandler(Mouse.MouseMoveEvent, mouseMove);
                if (RegistredElements.ContainsKey(frameworkElement))
                    RegistredElements.Remove(frameworkElement);
            }
        }

        private static ListBoxItem FindSymmetryPoint(FrameworkElement frameworkElement)
        {
            PointViewModel dot = ((frameworkElement as ListBoxItem).Content) as PointViewModel;
            var listcanvas = VisualTreeHelper.GetParent(frameworkElement as DependencyObject) as Canvas;
            var listbox_me = VisualTreeHelper.GetParent(listcanvas) as ListBox;
            Teeth me = ViewUtils.FindParent(listbox_me, Type.GetType("Process_Page.ToothTemplate.Teeth")) as Teeth;

            var main = ((SmileDesign_Page)(Application.Current.MainWindow.Content)) as SmileDesign_Page;
            int idx_me = main.ToothControl.dic[me.Name];
            int idx_you = idx_me + (idx_me >= 0 && idx_me < 3 ? +3 : -3);

            var parent = me.Parent as Grid;
            var myKey = main.ToothControl.dic.FirstOrDefault(p => p.Value == idx_you).Key;
            Teeth you = parent.FindName(myKey) as Teeth;
            ListBox listBox_you = you.FindName("list") as ListBox;

            return (ListBoxItem)(listBox_you.ItemContainerGenerator.ContainerFromIndex(dot.I));
        }

        #endregion

        private class MyHandlersData
        {
            public RoutedEventHandler MouseDown { get; set; }
            public RoutedEventHandler MouseUp { get; set; }
            public RoutedEventHandler MouseMove { get; set; }
        }
    }
}
