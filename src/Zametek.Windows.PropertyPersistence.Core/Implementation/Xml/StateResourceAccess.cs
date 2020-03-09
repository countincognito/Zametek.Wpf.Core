//using System.IO;
//using System.Xml.Serialization;

//namespace Zametek.WindowsEx.PropertyPersistence.Xml
//{
//    public class StateResourceAccess
//        : IAccessStateResource<State>
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

//        public State Load()
//        {
//            if (File.Exists(m_XmlFileName))
//            {
//                using (FileStream stream = File.Open(m_XmlFileName, FileMode.Open))
//                {
//                    var xmlSerializer = new XmlSerializer(typeof(State));
//                    return xmlSerializer.Deserialize(stream) as State;
//                }
//            }
//            return new State();
//        }

//        public void Save(State state)
//        {
//            using (FileStream stream = File.Open(m_XmlFileName, FileMode.Create))
//            {
//                var xmlSerializer = new XmlSerializer(typeof(State));
//                var namespaces = new XmlSerializerNamespaces();
//                namespaces.Add(string.Empty, string.Empty);
//                xmlSerializer.Serialize(stream, state, namespaces);
//            }
//        }

//        #endregion
//    }
//}
