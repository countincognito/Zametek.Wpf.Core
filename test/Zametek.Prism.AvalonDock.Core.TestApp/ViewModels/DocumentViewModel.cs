using Prism;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace Zametek.Wpf.Core.TestApp
{
    public sealed class DocumentViewModel
       : BindableBase, INavigationAware, IActiveAware
    {
        #region Fields

        private bool m_IsActive;

        #endregion

        #region Fields

        private string m_Name;

        #endregion

        #region Ctors

        public DocumentViewModel()
        {
        }

        #endregion

        #region Properties

        public string Title
        {
            get
            {
                return Name;
            }
        }

        public string Name
        {
            get
            {
                return m_Name;
            }
            private set
            {
                m_Name = value;
                RaisePropertyChanged(nameof(Title));
                RaisePropertyChanged(nameof(Name));
            }
        }

        #endregion

        #region INavigationAware Members

        bool INavigationAware.IsNavigationTarget(NavigationContext navigationContext)
        {
            if (navigationContext == null)
            {
                throw new ArgumentNullException(nameof(navigationContext));
            }
            var name = navigationContext.Parameters[Properties.Resources.Name] as string;
            if (!string.IsNullOrEmpty(name))
            {
                return string.Compare(name, Name, StringComparison.OrdinalIgnoreCase) == 0;
            }
            return false;
        }

        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext == null)
            {
                throw new ArgumentNullException(nameof(navigationContext));
            }
            Name = navigationContext.Parameters[Properties.Resources.Name] as string;
        }

        #endregion

        #region IActiveAware Members

        public event EventHandler IsActiveChanged;

        public bool IsActive
        {
            get
            {
                return m_IsActive;
            }
            set
            {
                if (m_IsActive != value)
                {
                    m_IsActive = value;
                    IsActiveChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        #endregion
    }
}
