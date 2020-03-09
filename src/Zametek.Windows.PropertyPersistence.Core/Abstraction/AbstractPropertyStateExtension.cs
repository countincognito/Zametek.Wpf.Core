using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Zametek.Wpf.Core
{
    public abstract class AbstractPropertyStateExtension<TState, TElement, TProperty>
       : MarkupExtension
        where TState : IAmState<TElement>, new()
        where TElement : IAmElement<TProperty>, new()
        where TProperty : IAmProperty, new()
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
