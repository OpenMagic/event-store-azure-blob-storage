/// <reference path="typings/index.d.ts" />

import { Gulpclass, Task, SequenceTask } from "gulpclass/Decorators";
import { Config } from "./config";
import { Log } from "./tasks/utils/log";
import { Shell } from "./tasks/utils/shell";

import * as gulp from "gulp";
import * as globby from "globby";
import * as path from "path";
import * as postinstall from "./tasks/postinstall";
import * as taskListing from "gulp-task-listing";

// todo: create typings
const nuget = require("npm-nuget");

const config = new Config();
const log = new Log();
const shell = new Shell(log);

@Gulpclass()
export class Gulpfile {
    @Task("default", ["help"])
    default() {
    }

    /**
     * Lists all tasks in this Gulpfile.
     */
    @Task()
    help(cb: Function) {
        const excludeTasks = ["default", "Gulpfile"];
        taskListing.withFilters(null, taskName => excludeTasks.indexOf(taskName) > -1)(cb);
    }

    /**
     * Install dependencies not handled by `npm install`.
     */
    @Task("postinstall", ["postinstall_createUnitTestClassesSpecFlowFeatureFiles", "postinstall_installSolutionNuGetPackages"])
    postinstall() {
    }

    /**
     * Create unit test class for SpecFlow's .feature files.
     */
    @Task("postinstall_createUnitTestClassesSpecFlowFeatureFiles", ["postinstall_restoreNuGetPackages"])
    postinstall_createUnitTestClassesSpecFlowFeatureFiles(cb: Function) {
        log.info(`Generating unit test classes for SpecFlow projects '${log.quote(config.specflow.projects)}'...`);

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

    /**
     * Install NuGet packages required by the solution but not be a specific project.
     */
    @Task()
    postinstall_installSolutionNuGetPackages(cb: Function) {
        log.info(`Installing solution level NuGet packages '${log.quote(config.nuget.solutionPackages.map(i => i.name))}'...`);

        config.nuget.solutionPackages.forEach(function (pkg) {
            const excludeVersion = pkg.excludeVersion ? "-ExcludeVersion" : "";
            const cmd = `install ${pkg.name} -Version ${pkg.version} -OutputDirectory ${config.nuget.outputDirectory} ${excludeVersion}`;
            nuget.exec(cmd);
        });

        return cb();
    }

    /**
     * Restore NuGet packages defined & required by projects. 
     */
    @Task()
    postinstall_restoreNuGetPackages(cb: Function) {
        nuget.exec(`restore`);
        cb();
    }
}
