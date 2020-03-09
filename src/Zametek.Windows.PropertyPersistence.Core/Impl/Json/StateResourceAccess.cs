using Newtonsoft.Json;
using System.IO;

namespace Zametek.Wpf.Core.Impl.Json
{
    public class StateResourceAccess
        : IStateResourceAccess<PersistenceState>
    {
        #region Fields

        private readonly string m_JsonFileName;

        #endregion

        #region Ctors

        public StateResourceAccess(string jsonFileName)
        {
            m_JsonFileName = jsonFileName;
        }

        #endregion

        #region IAccessStateResource<T> Members

        public PersistenceState Load()
        {
            if (File.Exists(m_JsonFileName))
            {
                using StreamReader reader = File.OpenText(m_JsonFileName);
                var jsonSerializer = new JsonSerializer();
                return jsonSerializer.Deserialize(reader, typeof(PersistenceState)) as PersistenceState;
            }
            return new PersistenceState();
        }

        public void Save(PersistenceState state)
        {
            using StreamWriter writer = File.CreateText(m_JsonFileName);
            var jsonSerializer = new JsonSerializer
            {
                Formatting = Formatting.Indented,
            };
            jsonSerializer.Serialize(writer, state, typeof(PersistenceState));
        }

        #endregion
    }
}
