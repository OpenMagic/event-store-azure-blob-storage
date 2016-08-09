const gulp = require('gulp');
const options = require('./config');
const plugins = {
    del: require('del'),
    log: require('gulplog'),
    msbuild: require('npm-msbuild'),
    quote: require('./scripts/quote')
}

// Load gulp tasks in ./tasks directory
require('load-gulp-tasks')(gulp, options, plugins);

gulp.task('default', ['help']);
gulp.task('help', require('gulp-task-listing'));
