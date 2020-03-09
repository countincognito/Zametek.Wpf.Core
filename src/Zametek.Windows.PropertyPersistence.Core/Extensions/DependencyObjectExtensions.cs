using System;
using System.Windows;
using System.Windows.Media;

namespace Zametek.Wpf.Core
{
    public static class DependencyObjectExtensions
    {
        public static T FindVisualAncestor<T>(this DependencyObject item)
            where T : DependencyObject
        {
            while (item != null && !(item is T))
            {
                item = VisualTreeHelper.GetParent(item);
            }
            return item as T;
        }

        public static T FindVisualAncestor<T>(
            this DependencyObject item,
            Predicate<T> predicate)
            where T : DependencyObject
        {
            while (item != null && (!(item is T) || !predicate((T)item)))
            {
                item = VisualTreeHelper.GetParent(item);
            }
            return item as T;
        }

        public static T FindVisualDescendant<T>(
            this DependencyObject item,
            string childName)
            where T : DependencyObject
        {
            T result = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(item);
            for (int childIndex = 0; childIndex < childrenCount; ++childIndex)
            {
                DependencyObject child = VisualTreeHelper.GetChild(item, childIndex);
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

        public static T FindLogicalAncestor<T>(this DependencyObject item)
            where T : DependencyObject
        {
            item = LogicalTreeHelper.GetParent(item);
            while (item != null && !(item is T))
            {
                item = LogicalTreeHelper.GetParent(item);
            }
            return item as T;
        }

        public static T FindLogicalAncestor<T>(
            this DependencyObject item,
            Predicate<T> predicate)
            where T : DependencyObject
        {
            item = LogicalTreeHelper.GetParent(item);
            while (item != null && (!(item is T) || !predicate((T)item)))
            {
                item = LogicalTreeHelper.GetParent(item);
            }
            return item as T;
        }
    }
}
