using Prism;
using System;

namespace Zametek.Wpf.Core.TestApp
{
    [AvalonDockAnchorable(Strategy = AnchorableStrategies.Bottom, IsHidden = true)]
    public partial class BottomAnchorableView
        : IActiveAware
    {
        #region Fields

        private bool m_IsActive;

        #endregion

        #region Ctors

        public BottomAnchorableView()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        public BottomAnchorableViewModel ViewModel
        {
            get
            {
                return DataContext as BottomAnchorableViewModel;
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
