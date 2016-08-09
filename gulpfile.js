const config = require('./config');
const del = require('del');
const gulp = require('gulp');
const log = require('gulplog');
const msbuild = require('npm-msbuild');
const quote = require('./scripts/quote');

gulp.task('build', ['compile']);
gulp.task('clean', clean);
gulp.task('compile', ['clean'], compile);


function compile(cb) {
    log.info(`Compiling solution...`);
    console.log();
    msbuild.exec(`/property:Configuration=${config.msbuild.configuration} /verbosity:${config.msbuild.verbosity}`);
    console.log();
    return cb();
}