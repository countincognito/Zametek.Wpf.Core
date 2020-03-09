using System.Collections.Generic;

namespace Zametek.Wpf.Core
{
    public class Element
        : IAmElement<Property>
    {
        public Element()
        {
            Properties = new List<Property>();
        }

        public List<Property> Properties
        {
            get;
            private set;
        }

        public string Uid
        {
            get;
            set;
        }
    }
}
