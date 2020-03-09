using AvalonDock;
using AvalonDock.Controls;
using AvalonDock.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Zametek.Utility;

namespace Zametek.Wpf.Core
{
    public static class DockingManagerExtensions
    {
        #region Public Methods

        #region Documents

        public static IEnumerable<LayoutDocument> GetAllDocuments(this DockingManager dockingManager)
        {
            if (dockingManager == null)
            {
                throw new ArgumentNullException(nameof(dockingManager));
            }
            return dockingManager.GetAllLayoutContents<LayoutDocument>();
        }

        public static LayoutDocument FindDocument(this DockingManager dockingManager, object content)
        {
            if (dockingManager == null)
            {
                throw new ArgumentNullException(nameof(dockingManager));
            }
            return dockingManager.FindLayoutContent<LayoutDocument>(content);
        }

        public static bool FloatDocument(this DockingManager dockingManager, object content)
        {
            if (dockingManager == null)
            {
                throw new ArgumentNullException(nameof(dockingManager));
            }
            LayoutDocument document = dockingManager.FindDocument(content);
            LayoutItem item = dockingManager.GetLayoutItemFromModel(document);
            if (item != null && !document.IsFloating)
            {
                item.FloatCommand.Execute(true);
            }
            return document.IsFloating;
        }

        /// <summary>
        /// Cleanly close a visible Document.
        /// </summary>
        /// <param name="dockingManager">The Root item DockingManager.</param>
        /// <param name="content">Either a View, ViewModel or <see cref="Xceed.Wpf.AvalonDock.Layout.LayoutDocument"/>
        /// contained in the layout of the DockingManager as a Document.</param>
        /// <returns>True is successfully closed, otherwise false.</returns>
        public static bool CloseDocument(this DockingManager dockingManager, object content)
        {
            if (dockingManager == null)
            {
                throw new ArgumentNullException(nameof(dockingManager));
            }
            if (content == null)
            {
                return true;
            }
            LayoutDocument document = dockingManager.FindDocument(content);
            LayoutItem item = dockingManager.GetLayoutItemFromModel(document);
            if (item != null)
            {
                item.CloseCommand.Execute(null);
            }
            if (document != null
               && dockingManager.GetAllDocuments().Contains(document))
            {
                // This is where you would end up when cancelling closing a document.
                return false;
            }
            return true;
        }

        public static bool CloseAllDocuments(this DockingManager dockingManager)
        {
            if (dockingManager == null)
            {
                throw new ArgumentNullException(nameof(dockingManager));
            }
            IEnumerable<LayoutDocument> documents = dockingManager.GetAllDocuments();
            // Separate list needed because calling Close removes the item from the IEnumerable.
            IList<LayoutDocument> documentList = documents.ToList();
            foreach (LayoutDocument document in documentList)
            {
                LayoutItem item = dockingManager.GetLayoutItemFromModel(document);
                if (item != null)
                {
                    item.CloseCommand.Execute(null);
                    if (documents.Contains(document))
                    {
                        // This is where you would end up if when cancelling closing a document.
                        break;
                    }
                }
            }
            return !documents.Any();
        }

        #endregion

        #region Anchorables

        public static IEnumerable<LayoutAnchorable> GetAllAnchorables(this DockingManager dockingManager)
        {
            if (dockingManager == null)
            {
                throw new ArgumentNullException(nameof(dockingManager));
            }
            return dockingManager.GetAllLayoutContents<LayoutAnchorable>();
        }

        public static LayoutAnchorable FindAnchorable(this DockingManager dockingManager, object content)
        {
            if (dockingManager == null)
            {
                throw new ArgumentNullException(nameof(dockingManager));
            }
            return dockingManager.FindLayoutContent<LayoutAnchorable>(content);
        }

        public static bool FloatAnchorable(this DockingManager dockingManager, object content)
        {
            if (dockingManager == null)
            {
                throw new ArgumentNullException(nameof(dockingManager));
            }
            return dockingManager.FloatAnchorable(content, true);
        }

        public static bool FloatAnchorable(this DockingManager dockingManager, object content, bool setAsActiveContent)
        {
            if (dockingManager == null)
            {
                throw new ArgumentNullException(nameof(dockingManager));
            }
            LayoutAnchorable anchorable = dockingManager.FindAnchorable(content);
            bool shown = dockingManager.ShowAnchorable(anchorable, setAsActiveContent);
            LayoutItem item = dockingManager.GetLayoutItemFromModel(anchorable);
            if (shown && !anchorable.IsFloating && item != null)
            {
                item.FloatCommand.Execute(true);
            }
            return anchorable.IsFloating;
        }

        /// <summary>
        /// Cleanly hide a visible Anchorable.
        /// </summary>
        /// <param name="dockingManager">The Root item DockingManager.</param>
        /// <param name="content">Either a View, ViewModel or <see cref="Xceed.Wpf.AvalonDock.Layout.LayoutAnchorable"/>
        /// contained in the layout of the DockingManager as an Anchorable.</param>
        /// <returns>True is successfully hidden, otherwise false.</returns>
        public static bool HideAnchorable(this DockingManager dockingManager, object content)
        {
            if (dockingManager == null)
            {
                throw new ArgumentNullException(nameof(dockingManager));
            }
            return dockingManager.HideAnchorable(content, true);
        }

        /// <summary>
        /// Cleanly hide a visible Anchorable.
        /// </summary>
        /// <param name="dockingManager">The Root item DockingManager.</param>
        /// <param name="content">Either a View, ViewModel or <see cref="Xceed.Wpf.AvalonDock.Layout.LayoutAnchorable"/>
        /// contained in the layout of the DockingManager as an Anchorable.</param>
        /// <param name="removeAsActiveContent">If the content is currently active, clear the Active Content of the
        /// DockingManager when hidden.</param>
        /// <returns>True is successfully hidden, otherwise false.</returns>
        public static bool HideAnchorable(this DockingManager dockingManager, object content, bool removeAsActiveContent)
        {
            if (dockingManager == null)
            {
                throw new ArgumentNullException(nameof(dockingManager));
            }
            if (content == null)
            {
                return true;
            }
            LayoutAnchorable anchorable = dockingManager.FindAnchorable(content);
            if (anchorable == null)
            {
                return false;
            }
            if (anchorable.IsHidden
                || dockingManager.IsActuallyHidden(anchorable))
            {
                return true;
            }
            if (dockingManager.GetLayoutItemFromModel(anchorable) is LayoutAnchorableItem item)
            {
                item.HideCommand.Execute(true);
            }
            if (removeAsActiveContent && dockingManager.ActiveContent == content)
            {
                dockingManager.ActiveContent = null;
            }
            return dockingManager.IsActuallyHidden(anchorable);
        }

        /// <summary>
        /// Cleanly show a hidden Anchorable.
        /// </summary>
        /// <param name="dockingManager">The Root item DockingManager.</param>
        /// <param name="content">Either a View, ViewModel or <see cref="Xceed.Wpf.AvalonDock.Layout.LayoutAnchorable"/>
        /// contained in the layout of the DockingManager as an Anchorable.</param>
        /// <returns>True is successfully shown, otherwise false.</returns>
        public static bool ShowAnchorable(this DockingManager dockingManager, object content)
        {
            if (dockingManager == null)
            {
                throw new ArgumentNullException(nameof(dockingManager));
            }
            return dockingManager.ShowAnchorable(content, true);
        }

        /// <summary>
        /// Cleanly show a hidden Anchorable.
        /// </summary>
        /// <param name="dockingManager">The Root item DockingManager.</param>
        /// <param name="content">Either a View, ViewModel or <see cref="Xceed.Wpf.AvalonDock.Layout.LayoutAnchorable"/>
        /// contained in the layout of the DockingManager as an Anchorable.</param>
        /// <param name="setAsActiveContent">Set the content as the Active Content of the DockingManager when shown.</param>
        /// <returns>True is successfully shown, otherwise false.</returns>
        public static bool ShowAnchorable(this DockingManager dockingManager, object content, bool setAsActiveContent)
        {
            if (dockingManager == null)
            {
                throw new ArgumentNullException(nameof(dockingManager));
            }
            if (content == null)
            {
                return true;
            }
            LayoutAnchorable anchorable = dockingManager.FindAnchorable(content);
            if (anchorable == null)
            {
                return false;
            }
            if (!anchorable.IsHidden
                || !dockingManager.IsActuallyHidden(anchorable))
            {
                return true;
            }
            anchorable.Show();
            if (setAsActiveContent)
            {
                dockingManager.ActiveContent = content;
            }
            return !dockingManager.IsActuallyHidden(anchorable);
        }

        #endregion

        #endregion

        #region Private Methods

        private static bool IsActuallyHidden<T>(this DockingManager dockingManager, T content)
            where T : LayoutContent
        {
            if (dockingManager == null)
            {
                throw new ArgumentNullException(nameof(dockingManager));
            }
            return dockingManager.Layout.Hidden.OfType<T>().Contains(content);
        }

        private static T FindLayoutContent<T>(this DockingManager dockingManager, object content)
            where T : LayoutContent
        {
            if (dockingManager == null)
            {
                throw new ArgumentNullException(nameof(dockingManager));
            }
            if (dockingManager.Layout == null)
            {
                throw new InvalidOperationException(Properties.Resources.DockingManagerLayoutPropertyIsNull);
            }
            if (content == null)
            {
                return null;
            }
            T layoutContent = null;
            content.TypeSwitchOn()
               .Case<T>(x =>
               {
                   layoutContent = x;
               })
               .Case<FrameworkElement>(x =>
               {
                   layoutContent = dockingManager.Layout.Descendents().OfType<T>().FirstOrDefault(y => y.Content == x)
                      ?? dockingManager.Layout.Descendents().OfType<T>().FirstOrDefault(y => y.Content == x.DataContext);
               })
               .Default(x =>
               {
                   layoutContent = dockingManager.Layout.Descendents().OfType<T>().FirstOrDefault(y => (y.Content as FrameworkElement)?.DataContext == x);
               });
            return layoutContent;
        }

        private static IEnumerable<T> GetAllLayoutContents<T>(this DockingManager dockingManager)
            where T : LayoutContent
        {
            if (dockingManager == null)
            {
                throw new ArgumentNullException(nameof(dockingManager));
            }
            if (dockingManager.Layout == null)
            {
                throw new InvalidOperationException(Properties.Resources.DockingManagerLayoutPropertyIsNull);
            }
            return dockingManager.Layout.Descendents().OfType<T>();
        }

        #endregion
    }
}