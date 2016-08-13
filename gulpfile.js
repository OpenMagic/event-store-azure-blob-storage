const gulp = require('gulp');
const options = require('./config');
const plugins = {
    del: require('del'),
    dependencies: dependencies,
    glob: require('globby'),
    log: require('gulplog'),
    mkdirp: require('mkdirp'),
    msbuild: require('npm-msbuild'),
    nuget: require('npm-nuget'),
    quote: require('./scripts/quote'),
    shell: require('shelljs'),
    versions: require('./scripts/versions')
}


// Load gulp tasks in ./tasks directory
require('load-gulp-tasks')(gulp, options, plugins);

gulp.task('build', ['package']);
gulp.task('default', ['help']);
gulp.task('help', require('gulp-task-listing'));

function dependencies(list) {
    return options.argv.ignoreDependencies ? null : list;
}
