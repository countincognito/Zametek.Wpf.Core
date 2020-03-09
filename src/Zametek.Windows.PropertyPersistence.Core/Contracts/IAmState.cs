using System.Collections.Generic;

namespace Zametek.Wpf.Core
{
    public interface IAmState<TElement>
    {
        List<TElement> Elements { get; }
    }
}
