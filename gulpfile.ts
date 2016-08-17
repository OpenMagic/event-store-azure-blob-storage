/// <reference path="typings/index.d.ts" />

import { Gulpclass, Task, SequenceTask } from "gulpclass/Decorators";
import { Config } from "./config";
import { Log } from "./scripts/gulp/log";
import { Shell } from "./scripts/gulp/shell";

import * as del from "del";
import * as globby from "globby";
import * as gulp from "gulp";
import * as gulpShell from "gulp-shell";
import * as mkdirp from "mkdirp"
import * as path from "path";
import * as taskListing from "gulp-task-listing";
import * as versions from "./scripts/versions";

// todo: create typings
const argv = require('yargs').argv;
const git = require('git-rev-sync');
const isGitClean = require('is-git-clean');
const msbuild = require("npm-msbuild");
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
        const mainTasks = ["build", "help", "publish", "test", "watch"]
        const excludeTasks = ["default", "Gulpfile", "postinstall"];

        taskListing
            .withFilters(
                taskName => mainTasks.indexOf(taskName) < 0, 
                taskName => excludeTasks.filter(value => value === taskName || taskName.indexOf(`${value}_`) === 0).length > 0)
            (cb);
    }

    /**
     * Compile, test & create packages
     */
    @Task("build", dependencies(["package"]))
    build(cb: Function) {
        cb();
    }

    /**
     * Clean output directories
     */
    @Task()
    clean(cb: Function) {
        log.info(`Deleting directories '${log.quote(config.clean.directories)}'...`)
        return del(config.clean.directories)
    }

    /**
     * Compile the solution.
     */
    @Task("compile", dependencies(["clean"]))
    compile(cb: Function) {
        log.info(`Compiling solution...`);

        console.log();
        msbuild.exec(`/property:Configuration=${config.msbuild.configuration} /verbosity:${config.msbuild.verbosity}`);
        console.log();

        return cb();
    }

    /**
     * Create nuget packages.
     */
    @Task("package", dependencies(["test"]))
    package() {
        const src = config.nuget.nuspecs;
        log.info(`Creating NuGet packages for '${src}'`);

        mkdirp.sync(config.artifacts);

        // todo: gulp typings is incorrect
        // const srcOption 
        const srcOptions: any = { read: false };

        const cmd = `${nuget.path()} pack <%= file.path %> -OutputDirectory ${config.artifacts} -Version ${versions.getNuGetVersion()} -Symbols -Properties "Configuration=${config.msbuild.configuration}"`;

        return gulp
            .src(src, srcOptions)
            .pipe(gulpShell(cmd));
    }

    /**
     * Install dependencies not handled by `npm install`.
     */
    @Task("postinstall", dependencies(["postinstall_createUnitTestClassesSpecFlowFeatureFiles", "postinstall_installSolutionNuGetPackages"]))
    postinstall() {
    }

    /**
     * Create unit test class for SpecFlow's .feature files.
     */
    @Task("postinstall_createUnitTestClassesSpecFlowFeatureFiles", dependencies(["postinstall_restoreNuGetPackages"]))
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

    /** 
     * Bump version number and create NuGet packages
     */
    @SequenceTask()
    publish() {
        const tasks = [
            "test",
            "publish_validateGitIsClean"
        ];

        return argv.ignoreDependencies ? tasks.slice(1) : tasks;
    }

    /**
     * Throws error if git repository is dirty
     */
    @Task()
    publish_validateGitIsClean(cb: Function) {
        if (isGitClean.sync()) {
            return cb();
        }
        throw new Error("Git repository must be clean before running the requested task.");
    }
    
    /** 
     * Run all tests.
     */
    @Task(null, dependencies(["compile"]))
    test() {
        const src = config.xunit.assemblies;
        log.info(`Running tests for '${log.quote(src)}'...`);

        // todo: gulp typings is incorrect
        // const srcOption { read: false }
        const srcOptions: any = null;

        return gulp
            .src(src, srcOptions)
            .pipe(gulpShell(`${config.xunit.cmd} <%= file.path %>`));
    }
}

/**
 * Ignore dependencies if --ignoreDependencies argument passed to gulp.
 * e.g. gulp test --ignoreDependencies
 */
function dependencies(value: string[]): string[] {
    return argv.ignoreDependencies ? null : value;
}
