using System.Collections.Generic;

namespace Zametek.Wpf.Core
{
    public class State
        : IAmState<Element>
    {
        public State()
        {
            Elements = new List<Element>();
        }

        public List<Element> Elements
        {
            get;
            private set;
        }
    }
}
