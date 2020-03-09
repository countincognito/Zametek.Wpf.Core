using System.Collections.Generic;

namespace Zametek.Wpf.Core
{
    public interface IPersistenceState<TElement>
    {
        List<TElement> Elements { get; }
    }
}
