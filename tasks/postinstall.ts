// Install dependencies not handled by `npm install` 

/// <reference path="../typings/index.d.ts" />

import { Config } from "../config";
import { Log } from "./utils/log";
import { Shell } from "./utils/shell";

import * as gulp from "gulp";
import * as globby from "globby";
import * as path from "path";

// todo: create typings
const nuget = require("npm-nuget");

export function createSpecFlowUnitTestClasses(cb: Function, config: Config, log: Log, shell: Shell): gulp.TaskFunction {
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

    return cb();
}

export function restoreNuGetPackages(cb: Function): gulp. {
    nuget.exec(`restore`);
}
