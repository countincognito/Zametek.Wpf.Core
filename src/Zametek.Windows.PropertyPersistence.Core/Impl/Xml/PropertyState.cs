//using System;
//using System.ComponentModel;
//using System.IO;
//using System.Windows;
//using System.Xml.Serialization;

//namespace Zametek.Wpf.Core.Impl.Xml
//{
//    public class PropertyState
//        : AbstractPropertyState<PersistenceState, PersistenceElement, PersistenceProperty>
//    {
//        internal PropertyState(DependencyObject element)
//            : base(element)
//        {
//        }

//        #region Overrides

//        protected override string Serialize(DependencyProperty property, object value)
//        {
//            Type valueType = DependencyPropertyDescriptor.FromProperty(property, Type).PropertyType;
//            var serializer = new XmlSerializer(valueType);
//            using var stringWriter = new StringWriter();
//            serializer.Serialize(stringWriter, value);
//            return stringWriter.ToString();
//        }

//        protected override object Deserialize(DependencyProperty property, string stringValue)
//        {
//            Type valueType = DependencyPropertyDescriptor.FromProperty(property, Type).PropertyType;
//            //if (valueType.IsAssignableFrom(typeof(IEnumerable)))
//            //{
//            //    valueType = typeof(List<object>);
//            //}
//            var serializer = new XmlSerializer(valueType);
//            using var stringReader = new StringReader(stringValue);
//            return serializer.Deserialize(stringReader);
//        }

//        #endregion
//    }
//}
