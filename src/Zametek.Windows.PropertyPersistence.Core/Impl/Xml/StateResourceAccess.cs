//using System.IO;
//using System.Xml.Serialization;

//namespace Zametek.Wpf.Core.Impl.Xml
//{
//    public class StateResourceAccess
//        : IStateResourceAccess<PersistenceState>
//    {
//        #region Fields

//        private string m_XmlFileName;

//        #endregion

//        #region Ctors

//        public StateResourceAccess(string xmlFileName)
//        {
//            m_XmlFileName = xmlFileName;
//        }

//        #endregion

//        #region IAccessStateResource<T> Members

//        public PersistenceState Load()
//        {
//            if (File.Exists(m_XmlFileName))
//            {
//                using FileStream stream = File.Open(m_XmlFileName, FileMode.Open);
//                var xmlSerializer = new XmlSerializer(typeof(PersistenceState));
//                return xmlSerializer.Deserialize(stream) as PersistenceState;
//            }
//            return new PersistenceState();
//        }

//        public void Save(PersistenceState state)
//        {
//            using FileStream stream = File.Open(m_XmlFileName, FileMode.Create);
//            var xmlSerializer = new XmlSerializer(typeof(PersistenceState));
//            var namespaces = new XmlSerializerNamespaces();
//            namespaces.Add(string.Empty, string.Empty);
//            xmlSerializer.Serialize(stream, state, namespaces);
//        }

//        #endregion
//    }
//}
