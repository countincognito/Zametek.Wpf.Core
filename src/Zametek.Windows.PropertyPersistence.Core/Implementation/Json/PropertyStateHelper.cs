using System;

namespace Zametek.Wpf.Core
{
    public static class PropertyStateHelper
    {
        public static void Load(IAccessStateResource<State> stateResourceAccess)
        {
            if (stateResourceAccess == null)
            {
                throw new ArgumentNullException(nameof(stateResourceAccess));
            }
            GenericPropertyStateHelper<State, Element, Property>.Load(
                stateResourceAccess,
                x => new PropertyState(x));
        }

        public static void Save(IAccessStateResource<State> stateResourceAccess)
        {
            if (stateResourceAccess == null)
            {
                throw new ArgumentNullException(nameof(stateResourceAccess));
            }
            GenericPropertyStateHelper<State, Element, Property>.Save(stateResourceAccess);
        }
    }
}
