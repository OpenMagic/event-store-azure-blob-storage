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
            'source/**/bin/Release',
            'source/**/obj/Release',
            'tests/**/bin/Release',
            'tests/**/obj/Release'
        ]
    },
    msbuild: msbuild,
    specflow: {
        cmd: `${__dirname}/packages/SpecFlow.2.1.0/tools/specflow.exe`,
        projects: files.getSpecFlowProjects(testsDirectory)
    },
    xunit: {
        cmd: `${__dirname}/packages/xunit.runner.console/tools/xunit.console.exe`,
        assemblies: files.getTestAssemblies(testsDirectory, msbuild.configuration)
    }
};

module.exports = config;