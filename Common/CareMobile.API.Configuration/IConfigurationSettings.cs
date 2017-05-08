namespace CareMobile.API.Configuration
{
    public interface IConfigurationSettings
    {
        string AzureDocumentDbDatabaseId { get; set; }
        string AzureDocumentDbEndPoint { get; set; }
        string AzureDocumentDbAuthKey { get; set; }
        string AzureStorageEndPoint { get; set; }
        string AzureStorageContainerName { get; set; }
        string AzureStorageContainerDirectoryName { get; set; }
        
        void InitializeSettings();
    }
}
