using System;
using System.Windows;
using System.Windows.Media;

namespace Zametek.Wpf.Core
{
    public static class DependencyObjectExtensions
    {
        public static T FindVisualAncestor<T>(this DependencyObject obj)
            where T : DependencyObject
        {
            while (obj != null && !(obj is T))
            {
                obj = VisualTreeHelper.GetParent(obj);
            }
            return obj as T;
        }

        public static T FindVisualAncestor<T>(this DependencyObject obj, Predicate<T> predicate)
            where T : DependencyObject
        {
            while (obj != null && (!(obj is T) || !predicate((T)obj)))
            {
                obj = VisualTreeHelper.GetParent(obj);
            }
            return obj as T;
        }

        public static T FindVisualDescendant<T>(this DependencyObject obj, string childName)
            where T : DependencyObject
        {
            T result = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(obj);
            for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, childIndex);
                if ((child as T) == null)
                {
                    result = child.FindVisualDescendant<T>(childName);
                    if (result != null)
                    {
                        break;
                    }
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    if (child is FrameworkElement frameworkElement
                        && string.Equals(frameworkElement.Name, childName, StringComparison.Ordinal))
                    {
                        result = child as T;
                        break;
                    }
                }
                else
                {
                    result = child as T;
                    break;
                }
            }
            return result;
        }

        public static T FindLogicalAncestor<T>(this DependencyObject obj)
            where T : DependencyObject
        {
            obj = LogicalTreeHelper.GetParent(obj);
            while (obj != null && !(obj is T))
            {
                obj = LogicalTreeHelper.GetParent(obj);
            }
            return obj as T;
        }

        public static T FindLogicalAncestor<T>(this DependencyObject obj, Predicate<T> predicate)
            where T : DependencyObject
        {
            obj = LogicalTreeHelper.GetParent(obj);
            while (obj != null && (!(obj is T) || !predicate((T)obj)))
            {
                obj = LogicalTreeHelper.GetParent(obj);
            }
            return obj as T;
        }
    }
}
