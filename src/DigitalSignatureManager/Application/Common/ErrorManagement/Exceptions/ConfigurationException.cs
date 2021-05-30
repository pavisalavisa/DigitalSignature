using System;

namespace Application.Common.ErrorManagement.Exceptions
{
    public class ConfigurationException : Exception
    {
        public ConfigurationException(): base()
        {

        }

        public ConfigurationException(string missingConfigurationKey) : base()
        {
            GetMissingConfigurationKey = missingConfigurationKey;
        }

        public string GetMissingConfigurationKey { get; }
    }
}