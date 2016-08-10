var path = require('path');

const artifactsDirectory = 'artifacts';
const msbuild = {
    configuration: 'Release',
    verbosity: 'Minimal'
};

const config = {
    artifacts: artifactsDirectory,
    clean: {
        directories: [
            artifactsDirectory,
            `source/**/bin/${msbuild.configuration}`,
            `source/**/obj/${msbuild.configuration}`,
            `tests/**/bin/${msbuild.configuration}`,
            `tests/**/obj/${msbuild.configuration}`
        ]
    },
    msbuild: msbuild,
    nuget: {
        nuspecs: [
            '*.nuspec'
        ]
    },
    specflow: {
        cmd: `${__dirname}/packages/SpecFlow.2.1.0/tools/specflow.exe`,
        projects: [
            'tests/**/*.Specifications.csproj'
        ]
    },
    versions: require('./scripts/versions'),
    xunit: {
        cmd: `${__dirname}/packages/xunit.runner.console/tools/xunit.console.exe`,
        assemblies: [
            `tests/**/bin/${msbuild.configuration}/*.Specifications.dll`,
            `tests/**/bin/${msbuild.configuration}/*.Tests.dll`
        ]
    }
};

module.exports = config;