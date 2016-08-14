// Install dependencies not handled by `npm install` 

/// <reference path="../typings/index.d.ts" />

import { config } from "../config";
import { log } from "./log";
import * as globby from "globby";

const nuget = require('npm-nuget');
const path = require('path');
const quote = require('./quote');
const shell = require('shelljs');

function createSpecFlowUnitTestClasses() {
    const task = 'generate unit test classes for all SpecFlow projects'

    log.startingTask(task);

    console.log(`Generating unit test classes for SpecFlow projects '${quote(config.specflow.projects)}'...`);

    const projects = globby.sync(config.specflow.projects);
    projects.forEach(function (project) {
        console.log(`Generating unit test classes for SpecFlow project '${quote(path.basename(project))}'...`);
        shell.exec(`${config.specflow.cmd} generateall ${project}`);
    });

    log.finishedTask(task);
}

function restoreNuGetPackages() {
    const task = 'restore NuGet packages';

    log.startingTask(task);

    nuget.exec(`restore`);
    nuget.exec(`install xunit.runner.console -OutputDirectory ./packages -ExcludeVersion -Version ${config.xunit.version}`);

    log.finishedTask(task);
}

restoreNuGetPackages();
createSpecFlowUnitTestClasses();