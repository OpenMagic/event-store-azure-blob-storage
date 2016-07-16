namespace OpenMagic.EventStore.AzureBlobStorage.Specifications.Settings
{
    public class AzureStorageSettings : AppSettings
    {
        public AzureStorageSettings()
            : base("Azure_Storage")
        {
        }

        public string ConnectionString => GetString("ConnectionString");
        public string ContainerName => GetString("ContainerName");
    }
}