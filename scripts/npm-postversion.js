// Install dependencies not handled by `npm install` 

const taskOperation = require('chalk').cyan;
const config = require('../config');
const path = require('path');
const quote = require('./quote');
const replace = require("replace");
const shell = require('shelljs');

function finished(message) {
    console.log(`Finished '${taskOperation(message)}'.`);
}

function starting(message) {
    console.log(`Starting '${taskOperation(message)}'...`);
}

function updateConstants() {
    const task = `update config.constants`;

    starting(task);

    const version = npmVersionToAssemblyVersion(process.env.npm_package_version);

    replace({
        regex: /public const string Version = \"\d+\.\d+\.\d+\.\d\";/,
        replacement: `public const string Version = \"${version}\";`,
        paths: config.constants,
        recursive: true,
        silent: false,
    });

    finished(task);
}

updateConstants();