using System.Collections.Generic;

namespace Zametek.Wpf.Core
{
    public interface IPersistenceElement<TProperty>
    {
        List<TProperty> Properties { get; }

        string Uid { get; set; }
    }
}
