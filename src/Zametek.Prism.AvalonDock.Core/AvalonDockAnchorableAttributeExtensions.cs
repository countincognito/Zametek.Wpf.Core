using System;
using System.Linq;

namespace Zametek.Wpf.Core
{
    public static class AvalonDockAnchorableAttributeExtensions
    {
        public static bool IsAnchorable(this object item)
        {
            return item?.GetAvalonDockAnchorableAttribute() != null;
        }

        public static AnchorableStrategy GetAnchorableStrategy(this object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            var avalonDockAnchorableAttribute = item.GetAvalonDockAnchorableAttribute();
            if (avalonDockAnchorableAttribute == null)
            {
                throw new InvalidOperationException();
            }
            return avalonDockAnchorableAttribute.Strategy;
        }

        public static bool GetAnchorableIsHidden(this object item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            var avalonDockAnchorableAttribute = item.GetAvalonDockAnchorableAttribute();
            if (avalonDockAnchorableAttribute == null)
            {
                throw new InvalidOperationException();
            }
            return avalonDockAnchorableAttribute.IsHidden;
        }

        public static AvalonDockAnchorableAttribute GetAvalonDockAnchorableAttribute(this object item)
        {
            return item?.GetType().GetCustomAttributes(typeof(AvalonDockAnchorableAttribute), true).FirstOrDefault() as AvalonDockAnchorableAttribute;
        }
    }
}
