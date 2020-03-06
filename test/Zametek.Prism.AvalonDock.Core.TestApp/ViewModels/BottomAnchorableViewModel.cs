using Prism.Mvvm;
using Prism.Regions;

namespace Zametek.Wpf.Core.TestApp
{
    public sealed class BottomAnchorableViewModel
       : BindableBase, INavigationAware
    {
        #region Ctors

        public BottomAnchorableViewModel()
        {
            Title = "Bottom Anchorable ViewModel";
        }

        #endregion

        #region Properties

        public string Title
        {
            get;
            private set;
        }

        #endregion

        #region INavigationAware Members

        bool INavigationAware.IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        void INavigationAware.OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        void INavigationAware.OnNavigatedTo(NavigationContext navigationContext)
        {
        }

        #endregion
    }
}
