using System.Configuration;

namespace TradeDataMonitorApp.Configuration
{
    /// <summary>
    /// TradeDataLoaderElementCollection configuration element collection class. 
    /// <remarks>http://www.codeproject.com/Articles/16466/Unraveling-the-Mysteries-of-NET-Configuration</remarks>
    /// </summary>
    [ConfigurationCollection(typeof(TradeDataLoaderElement),
        CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class TradeDataLoaderElementCollection : ConfigurationElementCollection
    {
        #region Constructors
        static TradeDataLoaderElementCollection()
        {
            m_properties = new ConfigurationPropertyCollection();
        }

        public TradeDataLoaderElementCollection()
        {
        }
        #endregion

        #region Fields
        private static ConfigurationPropertyCollection m_properties;
        #endregion

        #region Properties
        protected override ConfigurationPropertyCollection Properties
        {
            get { return m_properties; }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }
        #endregion

        #region Indexers
        public TradeDataLoaderElement this[int index]
        {
            get { return (TradeDataLoaderElement)base.BaseGet(index); }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                base.BaseAdd(index, value);
            }
        }

        public TradeDataLoaderElement this[string name]
        {
            get { return (TradeDataLoaderElement)base.BaseGet(name); }
        }
        #endregion

        #region Overrides
        protected override ConfigurationElement CreateNewElement()
        {
            return new TradeDataLoaderElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as TradeDataLoaderElement).Class;
        }

        /// <summary>
        /// Method implemented for unit-testing only
        /// </summary>
        public void Add(TradeDataLoaderElement element)
        {
            LockItem = false;  // the workaround
            BaseAdd(element);
        }
        
        #endregion
    }
}