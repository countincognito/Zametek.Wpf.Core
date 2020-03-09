using Newtonsoft.Json;
using System.Collections.Generic;

namespace Zametek.Wpf.Core.Impl.Json
{
    public class PersistenceElement
        : IPersistenceElement<PersistenceProperty>
    {
        public PersistenceElement()
        {
            Properties = new List<PersistenceProperty>();
        }

        [JsonProperty("properties")]
        public List<PersistenceProperty> Properties
        {
            get;
            private set;
        }

        [JsonProperty("uid")]
        public string Uid
        {
            get;
            set;
        }
    }
}
