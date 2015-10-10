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
        /// </summary>
        [ConfigurationProperty("assembly", IsRequired = true)]
        public string Assembly
        {
            get { return (string)base["assembly"]; }
        }

        /// <summary>
        /// Gets the Class setting.
        /// </summary>
        [ConfigurationProperty("class", IsRequired = true)]
        public string Class
        {
            get { return (string)base["class"]; }
        }
        #endregion
    }
}