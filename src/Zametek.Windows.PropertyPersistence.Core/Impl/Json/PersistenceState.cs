using Newtonsoft.Json;
using System.Collections.Generic;

namespace Zametek.Wpf.Core.Impl.Json
{
    public class PersistenceState
        : IPersistenceState<PersistenceElement>
    {
        public PersistenceState()
        {
            Elements = new List<PersistenceElement>();
        }


        [JsonProperty("elements")]
        public List<PersistenceElement> Elements
        {
            get;
            private set;
        }
    }
}
