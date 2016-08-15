/// <reference path="typings/index.d.ts" />

import { Config } from "./config";
import { Log } from "./tasks/utils/log";
import { Shell } from "./tasks/utils/shell";

import * as gulp from "gulp";
import * as globby from "globby";
import * as path from "path";
import * as postinstall from "./tasks/postinstall";

// todo: create typings
const nuget = require("npm-nuget");

const config = new Config();
const log = new Log();
const shell = new Shell(log);

gulp.task("postinstall-create-specflow-unit-test-classes", ["postinstall-restore-nuget-packages"], cb => postinstall.createSpecFlowUnitTestClasses(cb, config, log, shell));
gulp.task("postinstall-restore-nuget-packages", cb => postinstall.)
gulp.task("postinstall", ["postinstall-create-specflow-unit-test-classes", "postinstall-install-solution-nuget-packages"], null);
