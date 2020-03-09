using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Zametek.Wpf.Core
{
    public class PropertyState
        : AbstractPropertyState<State, Element, Property>
    {
        internal PropertyState(DependencyObject element)
            : base(element)
        {
        }

        #region Overrides

        protected override string Serialize(DependencyProperty property, object value)
        {
            Type valueType = DependencyPropertyDescriptor.FromProperty(property, Type).PropertyType;
            if (valueType.IsAssignableFrom(typeof(IEnumerable)))
            {
                valueType = typeof(List<object>);
            }
            return JsonConvert.SerializeObject(value, valueType, null);
        }

        protected override object Deserialize(DependencyProperty property, string stringValue)
        {
            Type valueType = DependencyPropertyDescriptor.FromProperty(property, Type).PropertyType;
            if (valueType.IsAssignableFrom(typeof(IEnumerable)))
            {
                valueType = typeof(List<object>);
            }
            return JsonConvert.DeserializeObject(stringValue, valueType);
        }

        #endregion
    }
}
