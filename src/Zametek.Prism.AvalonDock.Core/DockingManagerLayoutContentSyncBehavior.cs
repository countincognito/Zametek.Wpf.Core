using AvalonDock;
using AvalonDock.Layout;
using Prism.Regions;
using Prism.Regions.Behaviors;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Zametek.Wpf.Core
{
    public class DockingManagerLayoutContentSyncBehavior
       : RegionBehavior, IHostAwareRegionBehavior
    {
        public static readonly string BehaviorKey = "DockingManagerLayoutContentSyncBehavior";

        private bool m_UpdatingActiveViewsInManagerActiveContentChanged;
        private DockingManager m_DockingManager;

        private readonly ObservableCollection<object> m_Documents = new ObservableCollection<object>();
        private ReadOnlyObservableCollection<object> m_ReadonlyDocumentsList = null;

        private readonly ObservableCollection<object> m_Anchorables = new ObservableCollection<object>();
        private ReadOnlyObservableCollection<object> m_ReadonlyAnchorablesList = null;

        public DockingManagerLayoutContentSyncBehavior(DockingManager dockingManager)
        {
            m_DockingManager = dockingManager ?? throw new ArgumentNullException(nameof(dockingManager));
        }

        public DependencyObject HostControl
        {
            get
            {
                return m_DockingManager;
            }
            set
            {
                m_DockingManager = value as DockingManager;
            }
        }

        public ReadOnlyObservableCollection<object> Documents
        {
            get
            {
                if (m_ReadonlyDocumentsList == null)
                {
                    m_ReadonlyDocumentsList = new ReadOnlyObservableCollection<object>(m_Documents);
                }
                return m_ReadonlyDocumentsList;
            }
        }

        public ReadOnlyObservableCollection<object> Anchorables
        {
            get
            {
                if (m_ReadonlyAnchorablesList == null)
                {
                    m_ReadonlyAnchorablesList = new ReadOnlyObservableCollection<object>(m_Anchorables);
                }
                return m_ReadonlyAnchorablesList;
            }
        }

        /// <summary>
        /// Starts to monitor the <see cref="IRegion"/> to keep it in synch with the items of the <see cref="HostControl"/>.
        /// </summary>
        protected override void OnAttach()
        {
            if (m_DockingManager == null)
            {
                throw new InvalidOperationException(Properties.Resources.DockingManagerIsNotDefined);
            }
            if (m_DockingManager.DocumentsSource != null)
            {
                throw new InvalidOperationException(Properties.Resources.DocumentsSourceMustBeNull);
            }
            if (m_DockingManager.AnchorablesSource != null)
            {
                throw new InvalidOperationException(Properties.Resources.AnchorablesSourceMustBeNull);
            }
            if (BindingOperations.GetBinding(m_DockingManager, DockingManager.DocumentsSourceProperty) != null)
            {
                throw new InvalidOperationException(Properties.Resources.DocumentsSourceMustNotBeBoundToAnything);
            }
            if (BindingOperations.GetBinding(m_DockingManager, DockingManager.AnchorablesSourceProperty) != null)
            {
                throw new InvalidOperationException(Properties.Resources.AnchorablesSourceMustNotBeBoundToAnything);
            }

            SynchronizeItems();

            m_DockingManager.ActiveContentChanged += DockingManager_ActiveContentChanged;
            Region.ActiveViews.CollectionChanged += Region_ActiveViews_CollectionChanged;
            Region.Views.CollectionChanged += Region_Views_CollectionChanged;
            m_DockingManager.DocumentClosed += DockingManager_DocumentClosed;
        }

        /// <summary>
        /// Binds the Documents Source and Anchorables Source of the Docking Manager
        /// and synchronizes them with the existing contents of the Region.
        /// </summary>
        private void SynchronizeItems()
        {
            BindingOperations.SetBinding(
                HostControl,
                DockingManager.DocumentsSourceProperty,
                new Binding(Properties.Resources.Documents)
                {
                    Source = this
                });
            BindingOperations.SetBinding(
               HostControl,
               DockingManager.AnchorablesSourceProperty,
               new Binding(Properties.Resources.Anchorables)
               {
                   Source = this
               });

            foreach (object view in Region.Views)
            {
                if (view.IsAnchorable())
                {
                    m_Anchorables.Add(view);
                }
                else
                {
                    m_Documents.Add(view);
                }
            }
        }

        /// <summary>
        /// Activates the appropriate Views in the Region whenever the Active Content
        /// changes in the Docking Manager.
        /// </summary>
        private void DockingManager_ActiveContentChanged(object sender, EventArgs e)
        {
            try
            {
                m_UpdatingActiveViewsInManagerActiveContentChanged = true;
                if (m_DockingManager == sender)
                {
                    var activeContent = m_DockingManager.ActiveContent;
                    foreach (var item in Region.ActiveViews.Where(it => it != activeContent))
                    {
                        if (item != null
                           && Region.Views.Contains(item)
                           && Region.ActiveViews.Contains(item))
                        {
                            Region.Deactivate(item);
                        }
                    }

                    if (activeContent != null
                       && Region.Views.Contains(activeContent)
                       && !Region.ActiveViews.Contains(activeContent))
                    {
                        Region.Activate(activeContent);
                    }
                }
            }
            finally
            {
                m_UpdatingActiveViewsInManagerActiveContentChanged = false;
            }
        }

        /// <summary>
        /// Updates the Active Content of the Docking Manager whenever the Active Views
        /// change in the Region.
        /// </summary>
        private void Region_ActiveViews_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (m_UpdatingActiveViewsInManagerActiveContentChanged
               || e.NewItems == null)
            {
                return;
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (m_DockingManager.ActiveContent != null
                   && m_DockingManager.ActiveContent != e.NewItems[0]
                   && Region.ActiveViews.Contains(m_DockingManager.ActiveContent))
                {
                    Region.Deactivate(m_DockingManager.ActiveContent);
                }
                m_DockingManager.ActiveContent = e.NewItems[0];
            }
            else
            {
                if (e.Action != NotifyCollectionChangedAction.Remove
                   || !e.OldItems.Contains(m_DockingManager.ActiveContent))
                {
                    return;
                }
                m_DockingManager.ActiveContent = null;
            }
        }

        /// <summary>
        /// Updates the collections bound to the Documents Source and Anchorables Source
        /// of the Docking Manager whenever the Views change in the Region.
        /// </summary>
        private void Region_Views_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (object newItem in e.NewItems)
                {
                    if (newItem.IsAnchorable())
                    {
                        m_Anchorables.Add(newItem);
                    }
                    else
                    {
                        m_Documents.Add(newItem);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object oldItem in e.OldItems)
                {
                    if (oldItem.IsAnchorable())
                    {
                        m_Anchorables.Remove(oldItem);
                    }
                    else
                    {
                        m_Documents.Remove(oldItem);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the Views collection in the Region whenever a Document is closed.
        /// </summary>
        private void DockingManager_DocumentClosed(object sender, DocumentClosedEventArgs e)
        {
            LayoutDocument layoutDocument = e.Document;
            if (layoutDocument == null)
            {
                return;
            }
            if (Region.Views.Contains(layoutDocument))
            {
                Region.Remove(layoutDocument);
            }
            if (Region.Views.Contains(layoutDocument.Content))
            {
                Region.Remove(layoutDocument.Content);
            }
            if (!(layoutDocument.Content is FrameworkElement fElement))
            {
                return;
            }
            if (Region.Views.Contains(fElement))
            {
                Region.Remove(fElement);
            }
            if (Region.Views.Contains(fElement.DataContext))
            {
                Region.Remove(fElement.DataContext);
            }
        }
    }
}
