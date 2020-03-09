using AvalonDock.Layout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zametek.Wpf.Core
{
    public class DockingManagerRegionAdapterLayoutStrategy
       : ILayoutUpdateStrategy
    {
        private readonly ILayoutUpdateStrategy m_WrappedStrategy = new EmptyDockingManagerRegionAdapterLayoutStrategy();

        public DockingManagerRegionAdapterLayoutStrategy()
        {
        }

        public DockingManagerRegionAdapterLayoutStrategy(ILayoutUpdateStrategy wrappedStrategy)
        {
            if (wrappedStrategy != null)
            {
                m_WrappedStrategy = wrappedStrategy;
            }
        }

        #region ILayoutUpdateStrategy Members

        public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
        {
            m_WrappedStrategy.AfterInsertAnchorable(layout, anchorableShown);
        }

        public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown)
        {
            m_WrappedStrategy.AfterInsertDocument(layout, anchorableShown);
        }

        public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
        {
            bool result = false;
            if (layout != null
               && anchorableToShow != null)
            {
                if (anchorableToShow.Root == null)
                {
                    anchorableToShow.AddToLayout(layout.Manager, GetContentAnchorableStrategy(anchorableToShow));
                    bool isHidden = GetContentAnchorableIsHidden(anchorableToShow);
                    if (isHidden)
                    {
                        anchorableToShow.CanHide = true;
                        anchorableToShow.Hide();
                    }
                    result = true;
                }
                else if (destinationContainer is LayoutAnchorablePane destPane && anchorableToShow.IsHidden)
                {
                    // Show a hidden Anchorable.
                    if (anchorableToShow.PreviousContainerIndex < 0)
                    {
                        destPane.Children.Add(anchorableToShow);
                    }
                    else
                    {
                        int insertIndex = anchorableToShow.PreviousContainerIndex;
                        if (insertIndex > destPane.ChildrenCount)
                        {
                            insertIndex = destPane.ChildrenCount;
                        }
                        destPane.Children.Insert(insertIndex, anchorableToShow);
                    }
                    result = true;
                }
            }
            return result || m_WrappedStrategy.BeforeInsertAnchorable(layout, anchorableToShow, destinationContainer);
        }

        public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer)
        {
            return m_WrappedStrategy.BeforeInsertDocument(layout, anchorableToShow, destinationContainer);
        }

        #endregion

        private static bool GetContentAnchorableIsHidden(LayoutAnchorable anchorable)
        {
            if (anchorable == null)
            {
                return false;
            }
            if (anchorable.IsAnchorable())
            {
                return anchorable.GetAnchorableIsHidden();
            }
            else if (anchorable.Content.IsAnchorable())
            {
                return anchorable.Content.GetAnchorableIsHidden();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private static AnchorableShowStrategy GetContentAnchorableStrategy(LayoutAnchorable anchorable)
        {
            var anchorableStrategy = AnchorableStrategies.Most;
            if (anchorable != null)
            {
                if (anchorable.IsAnchorable())
                {
                    anchorableStrategy = anchorable.GetAnchorableStrategy();
                }
                else if (anchorable.Content.IsAnchorable())
                {
                    anchorableStrategy = anchorable.Content.GetAnchorableStrategy();
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            AnchorableShowStrategy flag = 0;
            foreach (AnchorableStrategies strategyFlag in SplitAnchorableStrategies(anchorableStrategy))
            {
                var strategy = AnchorableShowStrategy.Most;

                strategy = strategyFlag switch
                {
                    AnchorableStrategies.Most => AnchorableShowStrategy.Most,
                    AnchorableStrategies.Left => AnchorableShowStrategy.Left,
                    AnchorableStrategies.Right => AnchorableShowStrategy.Right,
                    AnchorableStrategies.Top => AnchorableShowStrategy.Top,
                    AnchorableStrategies.Bottom => AnchorableShowStrategy.Bottom,
                    _ => throw new InvalidOperationException($@"Unknown AnchorableStrategy value {strategyFlag}"),
                };
                flag |= strategy;
            }
            if (flag == 0)
            {
                flag = AnchorableShowStrategy.Most;
            }
            return flag;
        }

        private static AnchorableStrategies[] SplitAnchorableStrategies(AnchorableStrategies strategy)
        {
            var returnArray = new List<AnchorableStrategies>();
            foreach (var value in Enum.GetValues(typeof(AnchorableStrategies)).Cast<AnchorableStrategies>())
            {
                if (strategy.HasFlag(value))
                {
                    returnArray.Add(value);
                }
            }
            return returnArray.ToArray();
        }

        #region Private Types

        private class EmptyDockingManagerRegionAdapterLayoutStrategy
           : ILayoutUpdateStrategy
        {
            public void AfterInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableShown)
            {
            }

            public void AfterInsertDocument(LayoutRoot layout, LayoutDocument anchorableShown)
            {
            }

            public bool BeforeInsertAnchorable(LayoutRoot layout, LayoutAnchorable anchorableToShow, ILayoutContainer destinationContainer)
            {
                return false;
            }

            public bool BeforeInsertDocument(LayoutRoot layout, LayoutDocument anchorableToShow, ILayoutContainer destinationContainer)
            {
                return false;
            }
        }

        #endregion
    }
}
