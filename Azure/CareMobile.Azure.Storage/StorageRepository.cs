using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

using CareMobile.API.Configuration;

namespace CareMobile.Azure.Storage
{
    public class StorageRepository : IStorageRepository
    {
        private IConfigurationSettings _settings;
        private CloudStorageAccount storageAccount;
        private CloudBlobClient blobClient;
        private CloudBlobContainer container;
        private CloudBlobDirectory directory;

        public StorageRepository(IConfigurationSettings settings)
        {
            _settings = settings;
        }

        public void DeleteFile(string fileName)
        {
            Initialize();

            var blockBlob = directory.GetBlockBlobReference(fileName);
            blockBlob.Delete(DeleteSnapshotsOption.IncludeSnapshots);
        }

        public byte[] GetByteArray(string fileName)
        {
            Initialize();

            var blockBlob = directory.GetBlockBlobReference(fileName);
            var ms = new MemoryStream();
            
            blockBlob.DownloadToStream(ms);

            var returnValue = ms.ToArray();
            ms.Close();

            return returnValue;
        }

        public string UploadFile(string fileName, Stream stream)
        {
            Initialize();

            var blockBlob = directory.GetBlockBlobReference(fileName);
            blockBlob.UploadFromStream(stream);

            blockBlob = directory.GetBlockBlobReference(fileName);
            return blockBlob.Uri.ToString();
        }

        private void Initialize()
        {
            storageAccount = CloudStorageAccount.Parse(_settings.AzureStorageEndPoint);
            blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference(_settings.AzureStorageContainerName);
            directory = container.GetDirectoryReference(_settings.AzureStorageContainerDirectoryName);
        }
    }
}
