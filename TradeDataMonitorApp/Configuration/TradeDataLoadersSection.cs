using System.Configuration;

namespace TradeDataMonitorApp.Configuration
{
    /// <summary>
    /// An TradeDataLoaders configuration section class. 
    /// <remarks>http://www.codeproject.com/Articles/16466/Unraveling-the-Mysteries-of-NET-Configuration</remarks>
    /// </summary>
    public class TradeDataLoadersSection : ConfigurationSection
    {
        #region Constructors
        /// <summary>
        /// Predefines the valid properties and prepares
        /// the property collection.
        /// </summary>
        static TradeDataLoadersSection()
        {
            s_propElement = new ConfigurationProperty(
                "TradeDataLoaders",
                typeof(TradeDataLoaderElementCollection),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            s_properties = new ConfigurationPropertyCollection();

            s_properties.Add(s_propElement);
        }
        #endregion

        #region Static Fields
        private static ConfigurationProperty s_propElement;
        private static ConfigurationPropertyCollection s_properties;
        #endregion


        #region Properties
        /// <summary>
        /// Gets the TradeDataLoaderElementCollection element.
        /// </summary>
        [ConfigurationProperty("TradeDataLoaders")]
        public virtual TradeDataLoaderElementCollection TradeDataLoaders
        {
            get { return (TradeDataLoaderElementCollection)base[s_propElement]; }
        }

        /// <summary>
        /// Override the Properties collection and return our custom one.
        /// </summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get { return s_properties; }
        }
        #endregion
    }
}