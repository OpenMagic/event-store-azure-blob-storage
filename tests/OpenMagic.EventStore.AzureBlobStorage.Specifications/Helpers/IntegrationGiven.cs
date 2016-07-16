using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers.Dummies;
using OpenMagic.EventStore.AzureBlobStorage.Specifications.Settings;

namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Helpers
{
    public class IntegrationGiven
    {
        public IntegrationGiven(AzureStorageSettings azureStorageSettings, IBlobNamer blobNamer)
        {
            BlobNamer = blobNamer;
            AggregateType = typeof(DummyAggregate);
            AggregateId = Guid.NewGuid().ToString();

            ConnectionString = azureStorageSettings.ConnectionString;
            ContainerName = azureStorageSettings.ContainerName;
        }


        public Type AggregateType { get; }
        public string AggregateId { get; }
        public Blob Blob => (Blob)BlobContainer.GetBlobAsync(AggregateType, AggregateId).Result;
        public BlobContainer BlobContainer => BlobContainer.GetAsync(ConnectionString, ContainerName).Result;
        public string BlobName => BlobNamer.GetBlobName(AggregateType, AggregateId);
        public IBlobNamer BlobNamer { get; set; }
        public CloudAppendBlob CloudAppendBlob => CloudBlobContainer.GetAppendBlobReference(BlobName);
        public CloudBlobContainer CloudBlobContainer => GetCloudBlobContainer();
        public string ConnectionString { get; }
        public string ContainerName { get; set; }
        public IEnumerable<object> Events { get; set; }

        private CloudBlobContainer GetCloudBlobContainer()
        {
            var storageAccount = CloudStorageAccount.Parse(ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(ContainerName);

            return blobContainer;
        }
    }
}