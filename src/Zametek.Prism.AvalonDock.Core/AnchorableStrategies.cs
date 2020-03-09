using System;

namespace Zametek.Wpf.Core
{
    [Flags]
    public enum AnchorableStrategies
    {
        Most = 1 << 0,
        Left = 1 << 1,
        Right = 1 << 2,
        Top = 1 << 3,
        Bottom = 1 << 4,
    }
}
