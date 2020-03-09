using System;

namespace Zametek.Wpf.Core.Impl.Json
{
    public static class PropertyStateHelper
    {
        public static void Load(IStateResourceAccess<PersistenceState> stateResourceAccess)
        {
            if (stateResourceAccess == null)
            {
                throw new ArgumentNullException(nameof(stateResourceAccess));
            }
            GenericPropertyStateHelper<PersistenceState, PersistenceElement, PersistenceProperty>.Load(
                stateResourceAccess,
                x => new PropertyState(x));
        }

        public static void Save(IStateResourceAccess<PersistenceState> stateResourceAccess)
        {
            if (stateResourceAccess == null)
            {
                throw new ArgumentNullException(nameof(stateResourceAccess));
            }
            GenericPropertyStateHelper<PersistenceState, PersistenceElement, PersistenceProperty>.Save(stateResourceAccess);
        }
    }
}
