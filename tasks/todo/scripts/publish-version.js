module.exports = function (gulp, config, $) {
    gulp.task('publish-version', function version(cb) {
        $.log.info(`version: ${process.env.npm_package_version}`)
        $.shell.exec('npm version patch');
        cb();
    });
}