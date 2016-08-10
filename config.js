var path = require('path');

const msbuild = {
    configuration: 'Release',
    verbosity: 'Minimal'
};

const testsDirectory = path.join(__dirname, 'tests');

const config = {
    clean: {
        directories: [
            'artifacts',
            `source/**/bin/${msbuild.configuration}`,
            `source/**/obj/${msbuild.configuration}`,
            `tests/**/bin/${msbuild.configuration}`,
            `tests/**/obj/${msbuild.configuration}`
        ]
    },
    msbuild: msbuild,
    specflow: {
        cmd: `${__dirname}/packages/SpecFlow.2.1.0/tools/specflow.exe`,
        projects: [
            'tests/**/*.Specifications.csproj'
        ]
    },
    xunit: {
        cmd: `${__dirname}/packages/xunit.runner.console/tools/xunit.console.exe`,
        assemblies: [
            `tests/**/bin/${msbuild.configuration}/*.Specifications.dll`,
            `tests/**/bin/${msbuild.configuration}/*.Tests.dll`
        ]
    }
};

module.exports = config;