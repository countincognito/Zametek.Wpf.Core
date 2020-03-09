using Newtonsoft.Json;
using System.IO;

namespace Zametek.Wpf.Core
{
    public class StateResourceAccess
        : IAccessStateResource<State>
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

        public State Load()
        {
            if (File.Exists(m_JsonFileName))
            {
                using StreamReader reader = File.OpenText(m_JsonFileName);
                var jsonSerializer = new JsonSerializer();
                return jsonSerializer.Deserialize(reader, typeof(State)) as State;
            }
            return new State();
        }

        public void Save(State state)
        {
            using StreamWriter writer = File.CreateText(m_JsonFileName);
            var jsonSerializer = new JsonSerializer
            {
                Formatting = Formatting.Indented,
            };
            jsonSerializer.Serialize(writer, state, typeof(State));
        }

        #endregion
    }
}
