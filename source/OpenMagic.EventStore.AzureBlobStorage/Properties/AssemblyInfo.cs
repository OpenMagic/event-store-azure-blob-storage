using System.Reflection;
using System.Runtime.CompilerServices;
using Anotar.LibLog;
using OpenMagic.EventStore.AzureBlobStorage;

[assembly: AssemblyTitle(Constants.Assembly.Title)]
[assembly: AssemblyDescription(".NET Event Store using Azure Blob storage")]
[assembly: AssemblyProduct(Constants.Assembly.Product)]
[assembly: AssemblyCopyright(Constants.Assembly.Copyright)]
[assembly: AssemblyVersion(Constants.Assembly.Version)]
[assembly: AssemblyFileVersion(Constants.Assembly.FileVersion)]
[assembly: InternalsVisibleTo("OpenMagic.EventStore.AzureBlobStorage.Specifications")]
[assembly: LogMinimalMessage]