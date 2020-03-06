using System;

namespace Zametek.Wpf.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AvalonDockAnchorableAttribute
       : Attribute
    {
        public AnchorableStrategy Strategy
        {
            get;
            set;
        }

        public bool IsHidden
        {
            get;
            set;
        }
    }
}
