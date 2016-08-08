const config = require('../config').msbuild;
const log = require('gulplog');
const msbuild = require('npm-msbuild');

module.exports = function compileSolution() {
    log.info(`Compiling solution...`);
    console.log();
    msbuild.exec(`/property:Configuration=${config.configuration} /verbosity:${config.verbosity}`);
    console.log();
}