// Install dependencies not handled by `npm install` 

const taskOperation = require('chalk').cyan;
const config = require('../config');
const glob = require('glob');
const nuget = require('npm-nuget');
const path = require('path');
const quote = require('./quote');
const shell = require('shelljs');

function createSpecFlowUnitTestClasses() {
    const task = 'generate unit test classes for all SpecFlow projects'

    starting(task);

    config.specflow.projects.forEach(function (projectsPattern) {
        console.log(`Generating unit test classes for SpecFlow '${quote(projectsPattern)}'...`);
        const projects = glob.sync(projectsPattern);
        projects.forEach(function (project) {
            console.log(`Generating unit test classes for SpecFlow '${quote(path.basename(project))}'...`);
            shell.exec(`${config.specflow.cmd} generateall ${project}`);
        })
    });

    finished(task);
}

function finished(message) {
    console.log(`Finished '${taskOperation(message)}'.`);
}

function restoreNuGetPackages() {
    const task = 'restore NuGet packages';

    starting(task);

    nuget.exec(`restore`);
    nuget.exec(`install xunit.runner.console -OutputDirectory ./packages -ExcludeVersion -Version 2.1`);

    finished(task);
}

function starting(message) {
    console.log(`Starting '${taskOperation(message)}'...`);
}

restoreNuGetPackages();
createSpecFlowUnitTestClasses();