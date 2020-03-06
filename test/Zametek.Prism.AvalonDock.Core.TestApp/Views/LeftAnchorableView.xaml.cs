using Prism;
using System;

namespace Zametek.Wpf.Core.TestApp
{
    [AvalonDockAnchorable(Strategy = AnchorableStrategy.Left, IsHidden = true)]
    public partial class LeftAnchorableView
        : IActiveAware
    {
        #region Fields

        private bool m_IsActive;

        #endregion

        #region Ctors

        public LeftAnchorableView()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public LeftAnchorableViewModel ViewModel
        {
            get
            {
                return DataContext as LeftAnchorableViewModel;
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
