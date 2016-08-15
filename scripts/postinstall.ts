// Install dependencies not handled by `npm install` 

/// <reference path="../typings/index.d.ts" />

import { config } from "../config";
import { log } from "./library/log";
import * as globby from "globby";
import * as path from "path";
import * as shell from "./library/shell";

// todo: create typings
const nuget = require("npm-nuget");

main();

function main() {
    restoreNuGetPackages();
    installXunitPackage();
    createSpecFlowUnitTestClasses();
}

function createSpecFlowUnitTestClasses() {
    const task = "generate unit test classes for all SpecFlow projects"

    log.startingTask(task);

    console.log(`Generating unit test classes for SpecFlow projects '${log.quote(config.specflow.projects)}'...`);

    const projects = globby.sync(config.specflow.projects);
    projects.forEach(function (project) {
        shell.exec(
            `${config.specflow.cmd} generateall ${project}`,
            `Generating unit test classes for SpecFlow project '${log.quote(path.basename(project))}'...`,
            `Successfully generated unit test classes for SpecFlow project '${log.quote(path.basename(project))}'...`,
            `Failed to generate unit test classes for SpecFlow project '${log.quote(path.basename(project))}'...`
        );
    });

    log.finishedTask(task);
}

function restoreNuGetPackages() {
    const task = "restore NuGet packages";

    log.startingTask(task);

    nuget.exec(`restore`);
    nuget.exec(`install ${package.name} -OutputDirectory ./packages -ExcludeVersion -Version ${package.version}`);

    log.finishedTask(task);
}
