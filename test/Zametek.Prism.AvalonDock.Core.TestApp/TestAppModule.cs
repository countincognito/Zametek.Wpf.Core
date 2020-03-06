using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Zametek.Wpf.Core.TestApp
{
    public class TestAppModule
       : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();

            IRegion mainregion = regionManager.Regions[Constants.MainRegion];
            var bottomAnchorableView = containerProvider.Resolve<BottomAnchorableView>();
            mainregion.Add(bottomAnchorableView);

            var rightAnchorableView = containerProvider.Resolve<RightAnchorableView>();
            mainregion.Add(rightAnchorableView);

            var leftAnchorableView = containerProvider.Resolve<LeftAnchorableView>();
            mainregion.Add(leftAnchorableView);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
