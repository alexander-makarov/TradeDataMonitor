using System.Collections.Specialized;

namespace TradeDataMonitorApp
{
    /// <summary>
    /// Interface to mock System.Configuration.ConfigurationManager
    /// </summary>
    public interface IConfigurationManager
    {
        /// <summary>
        /// Retrieves a specified configuration section for the current application's default configuration.
        /// </summary>
        /// 
        /// <returns>
        /// The specified <see cref="T:System.Configuration.ConfigurationSection"/> object, or null if the section does not exist.
        /// </returns>
        /// <param name="sectionName">The configuration section path and name.</param><exception cref="T:System.Configuration.ConfigurationErrorsException">A configuration file could not be loaded.</exception>
        object GetSection(string sectionName);

        /// <summary>
        /// Gets the <see cref="T:System.Configuration.AppSettingsSection"/> data for the current application's default configuration.
        /// </summary>
        /// 
        /// <returns>
        /// Returns a <see cref="T:System.Collections.Specialized.NameValueCollection"/> object that contains the contents of the <see cref="T:System.Configuration.AppSettingsSection"/> object for the current application's default configuration.
        /// </returns>
        /// <exception cref="T:System.Configuration.ConfigurationErrorsException">Could not retrieve a <see cref="T:System.Collections.Specialized.NameValueCollection"/> object with the application settings data.</exception>
        NameValueCollection AppSettings { get; }
    }
}