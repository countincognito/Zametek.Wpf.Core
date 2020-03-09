using Newtonsoft.Json;

namespace Zametek.Wpf.Core.Impl.Json
{
    public class PersistenceProperty
        : IPersistenceProperty
    {
        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }

        [JsonProperty("value")]
        public string Value
        {
            get;
            set;
        }
    }
}
