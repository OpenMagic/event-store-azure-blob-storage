'use strict';

const solutionName = "OpenMagic.EventStore.AzureBlobStorage"
const configuration = 'Release';

const config = {
    artifacts: './artifacts',
    configuration: configuration,
    solution: {
        name: solutionName
    },
    specflow: {
        cmd: `"${__dirname}/packages/SpecFlow.2.1.0/tools/specflow.exe"`,
        project: `"${__dirname}/tests/${solutionName}.Specifications/${solutionName}.Specifications.csproj"`
    },
    xunit: {
        cmd: `"${__dirname}/packages/xunit.runner.console/tools/xunit.console.exe"`,
        assemblies: `"${__dirname}/tests/${solutionName}.Specifications/bin/${configuration}/${solutionName}.Specifications.dll"`
    }
}

require('shelljs/make');

const mkdirp = require('mkdirp');
const msbuild = require('npm-msbuild');
const npmPackage = require('./package.json');
const nuget = require('npm-nuget');
const replace = require("replace");
const rimraf = require('rimraf');
const shell = require('shelljs');

// Clean the environment
target.clean = function () {
    runningTask('clean');
    rimraf.sync(config.artifacts);
    mkdirp.sync(config.artifacts);
    completedTask('clean');
}

// Compile the solution
target.compile = function () {
    target.clean();
    runningTask('compile');
    msbuild.exec(`/property:Configuration=${config.configuration} /verbosity:minimal`);
    completedTask('compile');
}

// Install dependencies not handled by `npm install` 
target.postinstall = function () {
    nuget.exec(`restore`);
    nuget.exec(`install xunit.runner.console -OutputDirectory ./packages -ExcludeVersion -Version 2.1`);

    console.log('Generating SpecFlow unit test classes...');
    shell.exec(`${config.specflow.cmd} generateall ${config.specflow.project}`);
    console.log('Successfully generated SpecFlow unit test classes.');
}

// Run all tests
target.test = function (args) {
    target.compile();
    runningTask('test');
    shell.exec(`${config.xunit.cmd} ${config.xunit.assemblies}`);
    completedTask('test');
}

// Log a task has completed
function completedTask(name) {
    console.log();
    console.log(`Successfully completed ${name} task.`);
    console.log();
}

// Log a task is starting
function runningTask(name) {
    console.log(`Starting ${name} task...`);
    console.log();
}
