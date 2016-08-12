// Install dependencies not handled by `npm install` 

const taskOperation = require('chalk').cyan;
const config = require('../config');
const glob = require('globby');
const path = require('path');
const quote = require('./quote');
const replace = require("replace");
const shell = require('shelljs');
const versions = require('./versions');

function finished(message) {
    console.log(`Finished '${taskOperation(message)}'.`);
}

function starting(message) {
    console.log(`Starting '${taskOperation(message)}'...`);
}

function updateConstants() {
    const task = `update constants`;

    starting(task);

    const version = versions.getAssemblyVersion();
    const constants = glob.sync(config.constants);

    console.log(`Updating assembly version in '${quote(constants)}' with '${quote(version)}'`);

    replace({
        regex: /public const string Version = \"\d+\.\d+\.\d+\.\d\";/,
        replacement: `public const string Version = \"${version}\";`,
        paths: constants,
        recursive: false,
        silent: true,
    });

    // const constants = glob.sync(config.constants);
    // console.log(`constants ${constants}`);
    // constants.forEach(function (constant) {
    //     console.log(`Updating assembly version in '${quote(constant)}'`);
    //     replace({
    //         regex: /public const string Version = \"\d+\.\d+\.\d+\.\d\";/,
    //         replacement: `public const string Version = \"${version}\";`,
    //         paths: constant,
    //         recursive: true,
    //         silent: false,
    //     });
    // });

    finished(task);
}

updateConstants();