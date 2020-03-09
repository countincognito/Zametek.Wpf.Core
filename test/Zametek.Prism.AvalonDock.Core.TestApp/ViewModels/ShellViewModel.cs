using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace Zametek.Wpf.Core.TestApp
{
    public class ShellViewModel
        : BindableBase
    {
        #region Fields

        private readonly IRegionManager m_RegionManager;
        private string m_DisplayName;
        private string m_NewDocumentName;

        #endregion

        #region Ctors

        public ShellViewModel(IRegionManager regionManager)
        {
            m_RegionManager = regionManager ?? throw new ArgumentNullException(nameof(regionManager));
            InitializeCommands();
            SubscribeToEvents();
        }

        #endregion

        #region Properties

        public string DisplayName
        {
            get
            {
                return m_DisplayName;
            }
            private set
            {
                if (m_DisplayName != value)
                {
                    m_DisplayName = value;
                    RaisePropertyChanged(nameof(DisplayName));
                }
            }
        }

        public string NewDocumentName
        {
            get
            {
                return m_NewDocumentName;
            }
            set
            {
                m_NewDocumentName = value;
                RaisePropertyChanged(nameof(NewDocumentName));
                RaiseCanExecuteChangedAllCommands();
            }
        }

        #endregion

        #region Commands

        private void InitializeCommands()
        {
            AddDocumentCommand = new DelegateCommand(AddDocument, CanAddDocument);
        }

        private void RaiseCanExecuteChangedAllCommands()
        {
            AddDocumentCommand.RaiseCanExecuteChanged();
        }

        public DelegateCommand AddDocumentCommand
        {
            get;
            private set;
        }

        #region AddDocument

        private void AddDocument()
        {
            string docName = NewDocumentName;
            if (!string.IsNullOrEmpty(docName))
            {
                var navigationParameters = new NavigationParameters
                {
                    { Properties.Resources.Name, docName }
                };
                m_RegionManager.RequestNavigate(
                    Constants.MainRegion,
                    new Uri($@"{Properties.Resources.DocumentView}{navigationParameters}",
                    UriKind.Relative));
                NewDocumentName = string.Empty;
            }
        }

        private bool CanAddDocument()
        {
            return !string.IsNullOrEmpty(NewDocumentName);
        }

        #endregion

        #endregion

        #region Private Methods

        private void SubscribeToEvents()
        {
        }

        #endregion
    }
}
