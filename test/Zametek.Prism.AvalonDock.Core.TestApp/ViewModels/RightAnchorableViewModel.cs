using Prism.Mvvm;
using Prism.Regions;

namespace Zametek.Wpf.Core.TestApp
{
    public sealed class RightAnchorableViewModel
       : BindableBase, INavigationAware
    {
        #region Ctors

        public RightAnchorableViewModel()
        {
            Title = "Right Anchorable ViewModel";
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
