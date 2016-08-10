var files = require('./scripts/files');
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
            `source/**/bin/${msbuild.configurations}`,
            `source/**/obj/${msbuild.configurations}`,
            `tests/**/bin/${msbuild.configurations}`,
            `tests/**/obj/${msbuild.configurations}`
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
        assemblies: files[
            `tests/**/bin/${msbuild.configurations}/*.Specifications.dll`,
            `tests/**/bin/${msbuild.configurations}/*.Tests.dll`
        ]
    }
};

module.exports = config;