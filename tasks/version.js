module.exports = function (gulp, config, $) {
    gulp.task('version', function publish(cb) {
        $.log.info(`version: ${process.env.npm_package_version}`)
        $.shell.exec('npm version patch');
    });

    gulp.task('version-npm-preversion', function publish(cb) {
        $.log.info(`npm-preversion: ${process.env.npm_package_version}`)
        cb();
    });

    gulp.task('version-npm-version', function publish(cb) {
        $.log.info(`npm-version: ${process.env.npm_package_version}`)
        cb();
    });

    gulp.task('version-npm-postversion', function publish(cb) {
        $.log.info(`npm-postversion: ${process.env.npm_package_version}`)
        cb();
    });
}