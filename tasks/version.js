module.exports = function (gulp, config, $) {
    gulp.task('version-npm-preversion', function publish(cb) {
        $.log.info(`preversion: ${env.process.npm_package_version}`)
    });

    gulp.task('version-npm-version', function publish(cb) {
        $.log.info(`version: ${env.process.npm_package_version}`)
    });

    gulp.task('version-npm-postversion', function publish(cb) {
        $.log.info(`postversion: ${env.process.npm_package_version}`)
    });
}