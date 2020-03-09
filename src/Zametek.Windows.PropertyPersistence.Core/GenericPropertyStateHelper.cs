using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Zametek.Wpf.Core
{
    public static class GenericPropertyStateHelper<TState, TElement, TProperty>
        where TState : IPersistenceState<TElement>, new()
        where TElement : IPersistenceElement<TProperty>, new()
        where TProperty : IPersistenceProperty, new()
    {
        #region Fields

        private static readonly Dictionary<string, AbstractPropertyState<TState, TElement, TProperty>> s_PropertyStates =
            new Dictionary<string, AbstractPropertyState<TState, TElement, TProperty>>();

        private static readonly DependencyProperty s_LoadedHandlersProperty =
           DependencyProperty.RegisterAttached("LoadedHandlers", typeof(Dictionary<string, RoutedEventHandler>), typeof(AbstractPropertyState<TState, TElement, TProperty>));

        private static Func<DependencyObject, AbstractPropertyState<TState, TElement, TProperty>> s_AddPropertyState;

        #endregion

        #region Public Static Methods

#pragma warning disable CA1000 // Do not declare static members on generic types
        public static void Load(
            IStateResourceAccess<TState> stateResourceAccess,
            Func<DependencyObject, AbstractPropertyState<TState, TElement, TProperty>> addPropertyState)
        {
            if (stateResourceAccess == null)
            {
                throw new ArgumentNullException(nameof(stateResourceAccess));
            }
            AbstractPropertyState<TState, TElement, TProperty>.Persistence.Load(stateResourceAccess);
            s_AddPropertyState = addPropertyState ?? throw new ArgumentNullException(nameof(addPropertyState));
        }

        public static void Save(IStateResourceAccess<TState> stateResourceAccess)
        {
            if (stateResourceAccess == null)
            {
                throw new ArgumentNullException(nameof(stateResourceAccess));
            }
            AbstractPropertyState<TState, TElement, TProperty>.Persistence.Save(stateResourceAccess);
        }
#pragma warning restore CA1000 // Do not declare static members on generic types

        #endregion

        #region Internal Static Methods

        internal static object ProvideValue(
            DependencyObject target,
            DependencyProperty property,
            object defaultValue,
            BindingBase xamlBinding)
        {
            if (target == null
                || property == null)
            {
                return null;
            }
            object outputValue = defaultValue;
            if (outputValue == null
                || string.IsNullOrEmpty(outputValue.ToString()))
            {
                throw new InvalidOperationException($@"No default value provided for property ""{target}.{property.Name}""");
            }
            if (!outputValue.GetType().IsSerializable)
            {
                throw new InvalidOperationException($@"Default value provided for property ""{target}.{property.Name}"" is not serializable");
            }

            FrameworkElement visualAnchor = AbstractPropertyState<TState, TElement, TProperty>.GetVisualAnchor(target);
            if (!(target is FrameworkElement element) || visualAnchor != null)
            {
                element = visualAnchor;
            }
            var defaultString = outputValue as string;
            if (!string.IsNullOrEmpty(defaultString))
            {
                outputValue = AbstractPropertyState<TState, TElement, TProperty>.ConvertFromString(target.GetType(), property, defaultString);
            }
            var propertyMultiValueConverter = new PropertyMultiValueConverter()
            {
                Target = element,
                Property = property,
            };

            void handler(object s, RoutedEventArgs e)
            {
                if (element != null)
                {
                    element.Loaded -= handler;
                }

                if (xamlBinding != null
                   && !IsPropertyPersisted(target, property))
                {
                    // Normally the persisted state is the initial value used
                    // for an element property when the UI is rendered, but if
                    // no value is persisted (and a data bound property is present)
                    // then use the data bound value.
                    propertyMultiValueConverter.PropertyValuePreference = PropertyValuePreference.DataBound;
                }
                if (!HasPropertyValue(target, property))
                {
                    object value = AddPropertyValue(target, property, outputValue);
                    if (value == null)
                    {
                        throw new InvalidOperationException($@"The element ""{target}"" has no unique identifier for property persistence");
                    }
                }

                var multiBinding = new MultiBinding()
                {
                    Converter = propertyMultiValueConverter,
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                };
                // Add a new binding between the element property and the
                // persisted state.
                multiBinding.Bindings.Add(CreateStateBinding(target, property));
                if (xamlBinding != null)
                {
                    // If the element property was previously data bound in XAML
                    // then add that binding to the multibinding collection.
                    multiBinding.Bindings.Add(xamlBinding);
                }
                BindingOperations.SetBinding(target, property, multiBinding);

                if (element.IsLoaded)
                {
                    propertyMultiValueConverter.PropertyValuePreference = PropertyValuePreference.DataBound;
                }
            }

            // Windows are already loaded when the application starts
            // so need to invoke their loaded handlers manually.
            if (target is Window)
            {
                handler(null, null);
            }
            // Keep track of the loaded handlers in case they need to
            // activated manually later.
            AddPropertyLoadedHandler(target, property, handler);
            if (element != null)
            {
                element.Loaded += handler;
            }
            if (HasPropertyValue(target, property))
            {
                return GetPropertyValue(target, property);
            }
            return outputValue;
        }

        internal static string GetUidWithNamespace(DependencyObject element)
        {
            return $@"{AbstractPropertyState<TState, TElement, TProperty>.GetNamespace(element)}{AbstractPropertyState<TState, TElement, TProperty>.GetUid(element)}";
        }

        #endregion

        #region Private Static Methods

        private static bool HasPropertyValue(
            DependencyObject element,
            DependencyProperty property)
        {
            string uidWithNamespace = GetUidWithNamespace(element);
            if (string.IsNullOrEmpty(uidWithNamespace)
               || !s_PropertyStates.TryGetValue(uidWithNamespace, out AbstractPropertyState<TState, TElement, TProperty> state))
            {
                return false;
            }
            return state.HasValue(property);
        }

        private static object GetPropertyValue(
            DependencyObject element,
            DependencyProperty property)
        {
            string uidWithNamespace = GetUidWithNamespace(element);
            if (string.IsNullOrEmpty(uidWithNamespace)
               || !s_PropertyStates.TryGetValue(uidWithNamespace, out AbstractPropertyState<TState, TElement, TProperty> state))
            {
                throw new InvalidOperationException($@"The property ""{element}.{property.Name}"" is not in state");
            }
            else if (!state.HasValue(property))
            {
                throw new InvalidOperationException($@"The property ""{element}.{property.Name}"" has no value");
            }
            return state.GetValue(property);
        }

        private static object AddPropertyValue(
            DependencyObject element,
            DependencyProperty property,
            object value)
        {
            string uidWithNamespace = GetUidWithNamespace(element);
            if (string.IsNullOrEmpty(uidWithNamespace))
            {
                return null;
            }
            if (!s_PropertyStates.TryGetValue(uidWithNamespace, out AbstractPropertyState<TState, TElement, TProperty> state))
            {
                state = s_AddPropertyState(element);
                s_PropertyStates.Add(uidWithNamespace, state);
            }
            return state.AddValue(property, value);
        }

        private static void UpdatePropertyValue(
            DependencyObject element,
            DependencyProperty property,
            object value)
        {
            string uidWithNamespace = GetUidWithNamespace(element);
            if (string.IsNullOrEmpty(uidWithNamespace)
               || !s_PropertyStates.TryGetValue(uidWithNamespace, out AbstractPropertyState<TState, TElement, TProperty> state))
            {
                return;
            }
            state.UpdateValue(property, value);
        }

        private static void AddPropertyLoadedHandler(
            DependencyObject element,
            DependencyProperty property,
            RoutedEventHandler value)
        {
            Dictionary<string, RoutedEventHandler> dictionary = GetPrivateLoadedHandlers(element);
            if (dictionary == null)
            {
                dictionary = new Dictionary<string, RoutedEventHandler>();
                SetPrivateLoadedHandlers(element, dictionary);
            }
            dictionary.Add(property.Name, value);
        }

        private static bool IsPropertyPersisted(
            DependencyObject element,
            DependencyProperty property)
        {
            string uidWithNamespace = GetUidWithNamespace(element);
            return AbstractPropertyState<TState, TElement, TProperty>.Persistence.Contains(uidWithNamespace, property.Name);
        }

        private static void SetPrivateLoadedHandlers(
            DependencyObject element,
            Dictionary<string, RoutedEventHandler> value)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            element.SetValue(s_LoadedHandlersProperty, value);
        }

        private static Dictionary<string, RoutedEventHandler> GetPrivateLoadedHandlers(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            return (Dictionary<string, RoutedEventHandler>)element.GetValue(s_LoadedHandlersProperty);
        }

        private static BindingBase CreateStateBinding(
            DependencyObject element,
            DependencyProperty property)
        {
            string uidWithNamespace = GetUidWithNamespace(element);
            var output = new Binding()
            {
                Mode = BindingMode.TwoWay,
                Converter = new PropertyValueConverter()
                {
                    Target = element,
                    Property = property,
                },
                Source = s_PropertyStates,                             // Not strictly necessary.
                Path = new PropertyPath($@"[{{{uidWithNamespace}}}]"), // Not strictly necessary.
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            };
            return output;
        }

        #endregion

        #region Private Classes

        private class PropertyValueConverter
           : IValueConverter
        {
            public DependencyObject Target
            {
                get;
                set;
            }

            public DependencyProperty Property
            {
                get;
                set;
            }

            public object Convert(
                object value,
                Type targetType,
                object parameter,
                CultureInfo culture)
            {
                if (HasPropertyValue(Target, Property))
                {
                    return GetPropertyValue(Target, Property);
                }
                BindingOperations.ClearBinding(Target, Property);
                return Binding.DoNothing;
            }

            public object ConvertBack(
                object value,
                Type targetType,
                object parameter,
                CultureInfo culture)
            {
                UpdatePropertyValue(Target, Property, value);
                return Binding.DoNothing;
            }
        }

        private class PropertyMultiValueConverter
           : IMultiValueConverter
        {
            public PropertyMultiValueConverter()
            {
                PropertyValuePreference = PropertyValuePreference.PersistedState;
            }

            public PropertyValuePreference PropertyValuePreference
            {
                get;
                set;
            }

            public DependencyObject Target
            {
                get;
                set;
            }

            public DependencyProperty Property
            {
                get;
                set;
            }

            public object Convert(
                object[] values,
                Type targetType,
                object parameter,
                CultureInfo culture)
            {
                if (values == null)
                {
                    throw new ArgumentNullException(nameof(values));
                }
                if (values.Length == 1)
                {
                    object input = values[0];
                    if (targetType.IsAssignableFrom(input.GetType()))
                    {
                        return input;
                    }
                    return System.Convert.ChangeType(input, targetType, CultureInfo.InvariantCulture);
                }
                if (values.Length == 2)
                {
                    // If there are two values then one is from the persisted state and the other
                    // is from property binding. So either return the persisted state (if required)
                    // or grab the bound property value, persist it, and then return it.
                    object input;
                    object result;
                    switch (PropertyValuePreference)
                    {
                        case PropertyValuePreference.PersistedState:
                            input = values[0];
                            if (targetType.IsAssignableFrom(input.GetType()))
                            {
                                result = input;
                            }
                            else
                            {
                                result = System.Convert.ChangeType(input, targetType, CultureInfo.InvariantCulture);
                            }
                            break;
                        case PropertyValuePreference.DataBound:
                            input = values[1];
                            if (targetType.IsAssignableFrom(input.GetType()))
                            {
                                result = input;
                            }
                            else
                            {
                                result = System.Convert.ChangeType(input, targetType, CultureInfo.InvariantCulture);
                            }
                            UpdatePropertyValue(Target, Property, result);
                            break;
                        default:
                            throw new InvalidOperationException($@"Unknown PropertyValuePreference value ""{PropertyValuePreference}""");
                    }
                    return result;
                }
                throw new InvalidOperationException($@"Invalid number of input items ""{values.Length}""");
            }

            public object[] ConvertBack(
                object value,
                Type[] targetTypes,
                object parameter,
                CultureInfo culture)
            {
                if (targetTypes == null)
                {
                    throw new InvalidOperationException();
                }
                var results = new List<object>();
                foreach (Type targetType in targetTypes)
                {
                    object input = value;
                    object result;
                    if (targetType.IsAssignableFrom(input.GetType()))
                    {
                        result = input;
                    }
                    else
                    {
                        result = System.Convert.ChangeType(input, targetType, CultureInfo.InvariantCulture);
                    }
                    results.Add(result);
                }
                return results.ToArray();
            }
        }

        private enum PropertyValuePreference
        {
            PersistedState,
            DataBound,
        }

        #endregion
    }
}
