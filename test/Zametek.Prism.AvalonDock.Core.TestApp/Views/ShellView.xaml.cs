using System;
using System.ComponentModel;
using System.Windows;

namespace Zametek.Wpf.Core.TestApp
{
    public partial class ShellView
    {
        #region Fields

        private readonly LeftAnchorableView m_LeftAnchorableView;
        private readonly BottomAnchorableView m_BottomAnchorableView;
        private readonly RightAnchorableView m_RightAnchorableView;

        #endregion

        #region Ctors

        public ShellView(
            LeftAnchorableView leftAnchorableView,
            BottomAnchorableView bottomAnchorableView,
            RightAnchorableView rightAnchorableView,
            ShellViewModel viewModel)
        {
            m_LeftAnchorableView = leftAnchorableView ?? throw new ArgumentNullException(nameof(leftAnchorableView));
            m_BottomAnchorableView = bottomAnchorableView ?? throw new ArgumentNullException(nameof(bottomAnchorableView));
            m_RightAnchorableView = rightAnchorableView ?? throw new ArgumentNullException(nameof(rightAnchorableView));
            ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            InitializeComponent();
        }

        #endregion

        #region Properties

        public ShellViewModel ViewModel
        {
            get
            {
                return DataContext as ShellViewModel;
            }
            set
            {
                DataContext = value;
            }
        }

        #endregion

        #region LeftAnchorable

        private void ShowLeftAnchorableButtonView_Click(object sender, RoutedEventArgs e)
        {
            ShowLeftAnchorableView();
        }

        private void ShowLeftAnchorableView()
        {
            DockManager.ShowAnchorable(m_LeftAnchorableView, setAsActiveContent: true);
        }

        private void ShowLeftAnchorableButtonViewModel_Click(object sender, RoutedEventArgs e)
        {
            ShowLeftAnchorableViewModel();
        }

        private void ShowLeftAnchorableViewModel()
        {
            DockManager.ShowAnchorable(m_LeftAnchorableView.ViewModel, setAsActiveContent: true);
        }


        private void HideLeftAnchorableButtonView_Click(object sender, RoutedEventArgs e)
        {
            HideLeftAnchorableView();
        }

        private void HideLeftAnchorableView()
        {
            DockManager.HideAnchorable(m_LeftAnchorableView, removeAsActiveContent: true);
        }

        private void HideLeftAnchorableButtonViewModel_Click(object sender, RoutedEventArgs e)
        {
            HideLeftAnchorableViewModel();
        }

        private void HideLeftAnchorableViewModel()
        {
            DockManager.HideAnchorable(m_LeftAnchorableView.ViewModel, removeAsActiveContent: true);
        }

        #endregion

        #region BottomAnchorable

        private void ShowBottomAnchorableButtonView_Click(object sender, RoutedEventArgs e)
        {
            ShowBottomAnchorableView();
        }

        private void ShowBottomAnchorableView()
        {
            DockManager.ShowAnchorable(m_BottomAnchorableView, setAsActiveContent: true);
        }

        private void ShowBottomAnchorableButtonViewModel_Click(object sender, RoutedEventArgs e)
        {
            ShowBottomAnchorableViewModel();
        }

        private void ShowBottomAnchorableViewModel()
        {
            DockManager.ShowAnchorable(m_BottomAnchorableView.ViewModel, setAsActiveContent: true);
        }


        private void HideBottomAnchorableButtonView_Click(object sender, RoutedEventArgs e)
        {
            HideBottomAnchorableView();
        }

        private void HideBottomAnchorableView()
        {
            DockManager.HideAnchorable(m_BottomAnchorableView, removeAsActiveContent: true);
        }

        private void HideBottomAnchorableButtonViewModel_Click(object sender, RoutedEventArgs e)
        {
            HideBottomAnchorableViewModel();
        }

        private void HideBottomAnchorableViewModel()
        {
            DockManager.HideAnchorable(m_BottomAnchorableView.ViewModel, removeAsActiveContent: true);
        }

        #endregion

        #region RightAnchorable

        private void ShowRightAnchorableButtonView_Click(object sender, RoutedEventArgs e)
        {
            ShowRightAnchorableView();
        }

        private void ShowRightAnchorableView()
        {
            DockManager.ShowAnchorable(m_RightAnchorableView, setAsActiveContent: true);
        }

        private void ShowRightAnchorableButtonViewModel_Click(object sender, RoutedEventArgs e)
        {
            ShowRightAnchorableViewModel();
        }

        private void ShowRightAnchorableViewModel()
        {
            DockManager.ShowAnchorable(m_RightAnchorableView.ViewModel, setAsActiveContent: true);
        }

        private void HideRightAnchorableButtonView_Click(object sender, RoutedEventArgs e)
        {
            HideRightAnchorableView();
        }

        private void HideRightAnchorableView()
        {
            DockManager.HideAnchorable(m_RightAnchorableView, removeAsActiveContent: true);
        }

        private void HideRightAnchorableButtonViewModel_Click(object sender, RoutedEventArgs e)
        {
            HideRightAnchorableViewModel();
        }

        private void HideRightAnchorableViewModel()
        {
            DockManager.HideAnchorable(m_RightAnchorableView.ViewModel, removeAsActiveContent: true);
        }

        #endregion

        #region CloseAllDocuments

        private void CloseAllDocuments_Click(object sender, RoutedEventArgs e)
        {
            DockManager.CloseAllDocuments();
        }

        #endregion

        #region Overrides

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            bool allClosed = DockManager.CloseAllDocuments();

            // Perhaps one of the documents isn't ready to close yet.
            e.Cancel = !allClosed;
        }

        #endregion
    }
}
