module.exports = {
    clean: {
        directories: [
            'artifacts',
            'source/**/bin/Release',
            'source/**/obj/Release',
            'tests/**/bin/Release',
            'tests/**/obj/Release'
        ]
    },
    msbuild: {
        configuration: 'Release',
        verbosity: 'Minimal'
    },
    specflow: {
        cmd: __dirname + '/packages/SpecFlow.2.1.0/tools/specflow.exe',
        projects: [
            __dirname + '/tests/OpenMagic.EventStore.AzureBlobStorage.Specifications/OpenMagic.EventStore.AzureBlobStorage.Specifications.csproj'
        ]
    }
};