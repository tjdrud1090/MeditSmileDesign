using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Process_Page.Util
{
    public class KeyBoardBehaviors
    {
        //키보드 비헤이비어
        //지금은 KeyDown만, 나중에 확장이 있을 수 있는 부분이라(단축키 등등)
        //새로운 cs를 새로 팜.
        public static readonly DependencyProperty KeyboardDownCommandProperty =
            DependencyProperty.RegisterAttached("KeyboardDownCommand", typeof(ICommand), typeof(KeyBoardBehaviors),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(KeyboardDownCommandChanged)));

        private static void KeyboardDownCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = d as FrameworkElement;
            element.KeyDown += element_KeyboardDown;
        }

        static void element_KeyboardDown(object sender, KeyboardEventArgs e)
        {
            FrameworkElement element = sender as FrameworkElement;
            ICommand command = GetKeyboardDownCommand(element);
            command.Execute(e);
        }

        public static void SetKeyboardDownCommand(UIElement element, ICommand value)
        {
            element.SetValue(KeyboardDownCommandProperty, value);
        }

        public static ICommand GetKeyboardDownCommand(UIElement element)
        {
            return (ICommand)element.GetValue(KeyboardDownCommandProperty);
        }
    }
}
