using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Zametek.Wpf.Core
{
    public abstract class AbstractPropertyStateExtensions<TState, TElement, TProperty>
       : MarkupExtension
        where TState : IPersistenceState<TElement>, new()
        where TElement : IPersistenceElement<TProperty>, new()
        where TProperty : IPersistenceProperty, new()
    {
        #region Public Properties

        public object Default
        {
            get;
            set;
        }

        public BindingBase Binding
        {
            get;
            set;
        }

        #endregion

        #region Overrides

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }
            if (!(serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget provideValueTarget))
            {
                return this;
            }
            return GenericPropertyStateHelper<TState, TElement, TProperty>.ProvideValue(
               provideValueTarget.TargetObject as DependencyObject,
               provideValueTarget.TargetProperty as DependencyProperty,
               Default, Binding) ?? this;
        }

        #endregion
    }
}
