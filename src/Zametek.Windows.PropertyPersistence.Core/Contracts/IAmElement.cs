using System.Collections.Generic;

namespace Zametek.Wpf.Core
{
    public interface IAmElement<TProperty>
    {
        List<TProperty> Properties { get; }

        string Uid { get; set; }
    }
}
