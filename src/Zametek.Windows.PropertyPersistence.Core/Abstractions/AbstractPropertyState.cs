using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace Zametek.Wpf.Core
{
    public abstract class AbstractPropertyState<TState, TElement, TProperty>
        where TState : IPersistenceState<TElement>, new()
        where TElement : IPersistenceElement<TProperty>, new()
        where TProperty : IPersistenceProperty, new()
    {
        #region Fields

        private readonly Dictionary<DependencyProperty, object> m_PropertyValues;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty VisualAnchorProperty =
              DependencyProperty.RegisterAttached("VisualAnchor", typeof(FrameworkElement), typeof(AbstractPropertyState<TState, TElement, TProperty>));

        public static readonly DependencyProperty UidProperty =
              DependencyProperty.RegisterAttached("Uid", typeof(string), typeof(AbstractPropertyState<TState, TElement, TProperty>), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty ModeProperty =
              DependencyProperty.RegisterAttached("Mode", typeof(PropertyStateMode), typeof(AbstractPropertyState<TState, TElement, TProperty>), new PropertyMetadata(PropertyStateMode.Persisted));

        public static readonly DependencyProperty IsNamespacingEnabledProperty =
           DependencyProperty.RegisterAttached("IsNamespacingEnabled", typeof(bool), typeof(AbstractPropertyState<TState, TElement, TProperty>), new PropertyMetadata(false));

#pragma warning disable CA1000 // Do not declare static members on generic types
        public static void SetUid(
            DependencyObject element,
            string value)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            element.SetValue(UidProperty, value);
        }

        public static string GetUid(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            return (string)element.GetValue(UidProperty);
        }

        public static void SetMode(
            DependencyObject element,
            PropertyStateMode value)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            element.SetValue(ModeProperty, value);
        }

        public static PropertyStateMode GetMode(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            return (PropertyStateMode)element.GetValue(ModeProperty);
        }

        public static void SetVisualAnchor(
            DependencyObject element,
            FrameworkElement value)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            element.SetValue(VisualAnchorProperty, value);
        }

        public static FrameworkElement GetVisualAnchor(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            return (FrameworkElement)element.GetValue(VisualAnchorProperty);
        }

        public static bool GetIsNamespacingEnabled(DependencyObject item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            return (bool)item.GetValue(IsNamespacingEnabledProperty);
        }

        public static void SetIsNamespacingEnabled(
            DependencyObject item,
            bool value)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            item.SetValue(IsNamespacingEnabledProperty, value);
        }
#pragma warning restore CA1000 // Do not declare static members on generic types

        #endregion

        #region Ctors

        internal AbstractPropertyState(DependencyObject element)
        {
            m_PropertyValues = new Dictionary<DependencyProperty, object>();
            Uid = GenericPropertyStateHelper<TState, TElement, TProperty>.GetUidWithNamespace(element);
            Mode = GetMode(element);
            Type = element.GetType();
        }

        static AbstractPropertyState()
        {
            Persistence = new Persistence<TState, TElement, TProperty>();
        }

        #endregion

        #region Internal Properties

        internal string Uid
        {
            get;
            private set;
        }

        internal PropertyStateMode Mode
        {
            get;
            private set;
        }

        internal Type Type
        {
            get;
            private set;
        }

        #endregion

        #region Internal Static Properties

        internal static Persistence<TState, TElement, TProperty> Persistence
        {
            get;
            private set;
        }

        #endregion

        #region Internal Methods

        internal bool HasValue(DependencyProperty property)
        {
            return m_PropertyValues.ContainsKey(property);
        }

        internal object GetValue(DependencyProperty property)
        {
            if (!m_PropertyValues.TryGetValue(property, out object value))
            {
                throw new InvalidOperationException($@"There is no value for property name ""{property.Name}""");
            }
            return value;
        }

        /// <summary>
        /// Adds a property value to the memory state if it does not already exist and
        /// adds it to the persisted state if necessary. Or, if the property value already
        /// exists in the persisted state then it is retrieved, added to the memory state
        /// and returned.
        /// </summary>
        internal object AddValue(DependencyProperty property, object value)
        {
            if (m_PropertyValues.ContainsKey(property))
            {
                return value;
            }
            if (Mode == PropertyStateMode.Persisted)
            {
                if (Persistence.Contains(Uid, property.Name))
                {
                    string stringValue = Persistence.GetValue(Uid, property.Name);
                    value = Deserialize(property, stringValue);
                }
                else
                {
                    Persistence.Persist(
                        Uid,
                        property.Name,
                        Serialize(property, value));
                }
            }
            m_PropertyValues.Add(property, value);
            return value;
        }

        internal void UpdateValue(DependencyProperty property, object value)
        {
            if (!m_PropertyValues.ContainsKey(property))
            {
                throw new InvalidOperationException($@"Property name ""{property.Name}"" is not in memory state");
            }
            if (Mode == PropertyStateMode.Persisted)
            {
                if (!Persistence.Contains(Uid, property.Name))
                {
                    throw new InvalidOperationException($@"Property name ""{property.Name}"" is not in persisted state");
                }
                Persistence.Persist(
                    Uid,
                    property.Name,
                    Serialize(property, value));
            }
            m_PropertyValues[property] = value;
        }


        #endregion

        #region Protected Methods

        protected abstract string Serialize(DependencyProperty prop, object value);

        protected abstract object Deserialize(DependencyProperty prop, string stringValue);

        #endregion

        #region Internal Static Methods

        internal static object ConvertFromString(Type targetType, DependencyProperty property, string stringValue)
        {
            return DependencyPropertyDescriptor
                .FromProperty(property, targetType)
                .Converter
                .ConvertFromString(stringValue);
        }

        internal static string ConvertToString(Type targetType, DependencyProperty property, object value)
        {
            return DependencyPropertyDescriptor
                .FromProperty(property, targetType)
                .Converter
                .ConvertToString(value);
        }

        internal static string GetNamespace(DependencyObject element)
        {
            FrameworkElement visualAnchor = GetVisualAnchor(element);
            if (!(element is FrameworkElement frameworkElement)
                || visualAnchor != null)
            {
                frameworkElement = visualAnchor;
                if (frameworkElement != null
                    && GetIsNamespacingEnabled(element))
                {
                    return GetNamespace(frameworkElement) + GetNamespaceName(frameworkElement) + '.';
                }
            }
            if (frameworkElement != null
                && !GetIsNamespacingEnabled(element))
            {
                frameworkElement = null;
            }
            return GetNamespace(frameworkElement);
        }

        #endregion

        #region Private Static Methods

        private static string GetNamespace(FrameworkElement element)
        {
            if (element == null)
            {
                return string.Empty;
            }
            var stringBuilder = new StringBuilder();
            FrameworkElement logicalAncestor = element.FindLogicalAncestor<FrameworkElement>();
            while (logicalAncestor != null)
            {
                stringBuilder.Insert(0, '.');
                stringBuilder.Insert(0, GetNamespaceName(logicalAncestor));
                logicalAncestor = logicalAncestor.FindLogicalAncestor<FrameworkElement>();
            }
            return stringBuilder.ToString();
        }

        private static string GetNamespaceName(FrameworkElement element)
        {
            string name = GetUid(element);
            if (string.IsNullOrEmpty(name))
            {
                name = element.GetType().Name;
            }
            return name;
        }

        #endregion
    }
}
