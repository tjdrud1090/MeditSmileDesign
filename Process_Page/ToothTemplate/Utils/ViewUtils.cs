using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Process_Page.ToothTemplate.Utils
{
    public static class ViewUtils
    {
        // do you have any parent?
        public static bool AnyParent(DependencyObject item, Func<DependencyObject, bool> condition)
        {
            if (item == null)
                return false;

            var logicalParent = LogicalTreeHelper.GetParent(item);
            var visualParent = VisualTreeHelper.GetParent(item);

            return condition(item) || AnyParent(visualParent, condition);
        }

        // Get Above Parent
        public static DependencyObject GetParent(DependencyObject item, Func<DependencyObject, bool> condition)
        {
            if (item == null)
                return null;

            var logicalParent = LogicalTreeHelper.GetParent(item);
            var visualParent = VisualTreeHelper.GetParent(item);

            return condition(item) ? item : GetParent(visualParent, condition);
        }

        public static DependencyObject GetVisualChild(DependencyObject item, Func<DependencyObject, bool> condition)
        {
            if (item == null)
                return null;

            var q = new Queue<DependencyObject>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(item); i++)
            {
                var child = VisualTreeHelper.GetChild(item, i);
                if (condition(child))
                    return child;
                q.Enqueue(child);       // condition(child) == false인 Child들을 모음
            }

            // condition(t) == false였던 Child들을 다시 각자 파고들어서 다시 검색
            while (q.Count > 0)
            {
                var subChild = q.Dequeue();
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(subChild); i++)
                {
                    var childofchild = VisualTreeHelper.GetChild(subChild, i);
                    if (condition(childofchild))
                        return childofchild;
                    q.Enqueue(childofchild);
                }
            }

            // 두단계나 검색했는데 없으면 Child가 없는 것으로 리턴
            return null;
        }

        public static DependencyObject GetLogicalChild(DependencyObject item, Func<DependencyObject, bool> condition)
        {
            if (item == null)
                return null;

            var q = new Queue<DependencyObject>();
            foreach (var child in LogicalTreeHelper.GetChildren(item))
            {
                var dependencyChild = child as DependencyObject;
                if (condition(dependencyChild))
                    return dependencyChild;
                q.Enqueue(dependencyChild);
            }

            while (q.Count > 0)
            {
                var subChild = q.Dequeue();
                foreach (var childofchild in LogicalTreeHelper.GetChildren(subChild))
                {
                    var dependencyChildofChild = childofchild as DependencyObject;
                    if (condition(dependencyChildofChild))
                        return dependencyChildofChild;
                    q.Enqueue(dependencyChildofChild);
                }
            }

            return null;
        }


        public static FrameworkElement FindParent(FrameworkElement child, Type parentType)
        {
            FrameworkElement fr = child.Parent as FrameworkElement;
            while (fr.GetType() != parentType && !(fr is Window))
            {
                fr = fr.Parent as FrameworkElement;
            }
            var parent = !(fr is Window) ? fr : null;
            return parent;
        }

        public static FrameworkElement FindChild(Panel parent, Type childType)
        {
            foreach (Control fr in parent.Children)
                if (fr.GetType() == childType)
                    return fr;
            return null;
        }
    }
}
