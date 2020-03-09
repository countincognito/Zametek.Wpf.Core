using System;
using System.Collections.Generic;
using System.Linq;

namespace Zametek.Wpf.Core
{
    internal class Persistence<TState, TElement, TProperty>
        where TState : IAmState<TElement>, new()
        where TElement : IAmElement<TProperty>, new()
        where TProperty : IAmProperty, new()
    {
        #region Fields

        private TState m_State;
        private readonly Dictionary<string, TElement> m_StateElementsLookup;

        #endregion

        #region Ctors

        internal Persistence()
        {
            m_State = new TState();
            m_StateElementsLookup = new Dictionary<string, TElement>();
        }

        #endregion

        #region Internal Methods

        internal void Load(IAccessStateResource<TState> stateResourceAccess)
        {
            if (stateResourceAccess == null)
            {
                throw new ArgumentNullException("stateResourceAccess");
            }
            try
            {
                m_State = stateResourceAccess.Load();
            }
            finally
            {
                InitializeLookupTables();
            }
        }

        internal void Save(IAccessStateResource<TState> stateResourceAccess)
        {
            if (stateResourceAccess == null)
            {
                throw new ArgumentNullException("stateResourceAccess");
            }
            stateResourceAccess.Save(m_State);
        }

        internal void Persist(string uid, string propertyName, string value)
        {
            if (m_StateElementsLookup.ContainsKey(uid))
            {
                Update(uid, propertyName, value);
            }
            else
            {
                Add(uid, propertyName, value);
            }
        }

        internal bool Contains(string uid, string propertyName)
        {
            return m_StateElementsLookup.ContainsKey(uid) && Contains(m_StateElementsLookup[uid], propertyName);
        }

        internal string GetValue(string uid, string propertyName)
        {
            return GetValue(m_StateElementsLookup[uid], propertyName);
        }

        #endregion

        #region Private Methods

        private void InitializeLookupTables()
        {
            foreach (TElement element in m_State.Elements)
            {
                m_StateElementsLookup.Add(element.Uid, element);
            }
        }

        private void Add(string uid, string propertyName, string value)
        {
            if (m_StateElementsLookup.ContainsKey(uid))
            {
                throw new InvalidOperationException(string.Format("Property name {0} is already in persisted state", propertyName));
            }
            var element = new TElement
            {
                Uid = uid
            };
            var property = new TProperty
            {
                Name = propertyName,
                Value = value
            };
            element.Properties.Add(property);
            m_State.Elements.Add(element);
            m_StateElementsLookup.Add(element.Uid, element);
        }

        private void Update(string uid, string propertyName, string value)
        {
            TElement element;
            if (!m_StateElementsLookup.TryGetValue(uid, out element))
            {
                throw new InvalidOperationException(string.Format("Property name {0} is not in persisted state", propertyName));
            }
            if (Contains(element, propertyName))
            {
                GetProperty(element, propertyName).Value = value;
                return;
            }
            var property = new TProperty
            {
                Name = propertyName,
                Value = value
            };
            element.Properties.Add(property);
        }

        #endregion

        #region Private Static Methods

        private static bool Contains(TElement element, string propertyName)
        {
            return element.Properties.Any(x => x.Name == propertyName);
        }

        private static string GetValue(TElement element, string propertyName)
        {
            TProperty property = element.Properties.FirstOrDefault(x => x.Name == propertyName);
            if (property == null)
            {
                return string.Empty;
            }
            return property.Value;
        }

        private static TProperty GetProperty(TElement element, string propertyName)
        {
            return element.Properties.FirstOrDefault(x => x.Name == propertyName);
        }

        #endregion
    }
}
