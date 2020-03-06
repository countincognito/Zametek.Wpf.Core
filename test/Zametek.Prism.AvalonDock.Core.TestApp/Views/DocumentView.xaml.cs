using Prism;
using System;

namespace Zametek.Wpf.Core.TestApp
{
    public partial class DocumentView
        : IActiveAware
    {
        #region Fields

        private bool m_IsActive;

        #endregion

        #region Ctors

        public DocumentView()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public DocumentViewModel ViewModel
        {
            get
            {
                return DataContext as DocumentViewModel;
            }
            set
            {
                if (ViewModel != value)
                {
                    DataContext = value;
                }
            }
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
