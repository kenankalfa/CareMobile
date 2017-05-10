using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CareMobile.API.Configuration;
using System.Configuration;

namespace CareMobile.API.Host
{
    public class ConfigurationSettings : IConfigurationSettings
    {
        public ConfigurationSettings()
        {
            InitializeSettings();
        }

        public string AzureDocumentDbDatabaseId { get; set; }
        public string AzureDocumentDbEndPoint { get; set; }
        public string AzureDocumentDbAuthKey { get; set; }
        public string AzureStorageEndPoint { get; set; }
        public string AzureStorageContainerName { get; set; }
        public string AzureStorageContainerDirectoryName { get; set; }
        public string AzureEmotionApiKey { get; set; }
        public string AzureEmotionApiEndPoint { get; set; }

        public void InitializeSettings()
        {
            AzureDocumentDbDatabaseId = ConfigurationManager.AppSettings["AzureDocumentDbDatabaseId"];
            AzureDocumentDbEndPoint = ConfigurationManager.AppSettings["AzureDocumentDbEndPoint"];
            AzureDocumentDbAuthKey = ConfigurationManager.AppSettings["AzureDocumentDbAuthKey"];
            AzureStorageEndPoint = ConfigurationManager.AppSettings["AzureStorageEndPoint"];
            AzureStorageContainerName = ConfigurationManager.AppSettings["AzureStorageContainerName"];
            AzureStorageContainerDirectoryName = ConfigurationManager.AppSettings["AzureStorageContainerDirectoryName"];
            AzureEmotionApiKey = ConfigurationManager.AppSettings["AzureEmotionApiKey"];
            AzureEmotionApiEndPoint = ConfigurationManager.AppSettings["AzureEmotionApiEndPoint"];
        }
    }
}