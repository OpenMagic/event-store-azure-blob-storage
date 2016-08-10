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

const git = require('git-rev-sync');
const isGitClean = require('is-git-clean');
const mkdirp = require('mkdirp');
const msbuild = require('npm-msbuild');
const npmPackage = require('./package.json');
const nuget = require('npm-nuget');
const replace = require("replace");
const rimraf = require('rimraf');
const shell = require('shelljs');

// Publish the package
target.publish = function (args) {
    runningTask('publish');

    validateGitIsClean();

    const newVersion = getNewVersionArgument(args);
    const currentBranch = git.branch();

    deleteGitBranchIfExists('publish')
    createAndCheckoutGitBranch('publish');
    updateVersion(newVersion);
    createPackage();
    mergePublishBranch(currentBranch);

    // Push the latest commits and related tags to remote server
    //shell.exec(`git push --follow-tags`);
    pushRepository();

    completedTask('publish');
}

function createAndCheckoutGitBranch(branchName) {
    console.log(`Creating branch '${branchName}'`);
    shell.exec(`git checkout -b ${branchName}`);
}

function deleteGitBranchIfExists(branchName) {
    if (shell.exec(`git show-ref --verify --quiet refs/heads/${branchName}`).code !== 0){
        return;
    }
    console.log(`Deleting branch '${branchName}'`);
    shell.exec(`git branch -d ${branchName}`);
}

// Get npm version argument
function getNewVersionArgument(args) {
    if (args == null || args.length !== 1) {
        console.log(args);
        writePublishUsage();
        process.exit(1);
    }
    return args[0];
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

// Update the version number
function updateVersion(newVersion) {


    // npm version will:
    //      - Run tests
    //      - Update version number in package.json and Constants.cs
    //      - Stage the version number changes to the Git repository
    //      - Create the nuget package
    //       
    // See tasks 'npm_preversion, npm_version, npm_postversion' for more details 
    //shell.exec(`npm version ${newVersion}`);   
}

// Write how to use publish command 
function writePublishUsage() {
    console.log();
    console.log(`Usage: publish <newversion> or node tasks publish <newversion>`);
    console.log();
    console.log(`where <newversion> is one of:`);
    console.log(`    major, minor, patch, premajor, preminor, prepatch, prerelease, from-git`);
    console.log();
}

// Check git repository is clean. Exits with 1 if it isn't.
function validateGitIsClean() {
    if (isGitClean.sync()) {
        return;
    }
    console.log('Git repository must be clean before running the requested task.')
    process.exit(1);
}
