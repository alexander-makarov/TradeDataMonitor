using System.Collections.Specialized;
using System.Configuration;

namespace TradeDataMonitorApp
{
    /// <summary>
    /// Wrapper around System.Configuration.ConfigurationManager
    /// </summary>
    public class ConfigurationManagerWrapper : IConfigurationManager
    {
        public object GetSection(string sectionName)
        {
            return ConfigurationManager.GetSection(sectionName);
        }

        public NameValueCollection AppSettings 
        {
            get { return ConfigurationManager.AppSettings; }
        }
    }
}