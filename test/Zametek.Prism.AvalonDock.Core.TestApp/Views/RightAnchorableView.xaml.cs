using Prism;
using System;

namespace Zametek.Wpf.Core.TestApp
{
    [AvalonDockAnchorable(Strategy = AnchorableStrategy.Right, IsHidden = true)]
    public partial class RightAnchorableView
        : IActiveAware
    {
        #region Fields

        private bool m_IsActive;

        #endregion

        #region Ctors

        public RightAnchorableView()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public RightAnchorableViewModel ViewModel
        {
            get
            {
                return DataContext as RightAnchorableViewModel;
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
