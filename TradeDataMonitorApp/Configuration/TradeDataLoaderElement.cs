using System.Configuration;

namespace TradeDataMonitorApp.Configuration
{
    /// <summary>
    /// An TradeDataLoaders configuration section class.
    /// </summary>
    public class TradeDataLoaderElement : ConfigurationElement
    {
        #region Properties
        /// <summary>
        /// Gets the Assembly setting.
        /// Setter implemented for unit-testing only.
        /// </summary>
        /// </summary>
        [ConfigurationProperty("assembly", IsRequired = true)]
        public string Assembly
        {
            get { return (string)base["assembly"]; }
            set { base["assembly"] = value; }
        }

        /// <summary>
        /// Gets the Class setting.
        /// Setter implemented for unit-testing only.
        /// </summary>
        [ConfigurationProperty("class", IsRequired = true)]
        public string Class
        {
            get { return (string)base["class"]; }
            set { base["class"] = value; }
        }
        #endregion
    }
}