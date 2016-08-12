module.exports = function (gulp, config, $) {
    gulp.task('version', function publish(cb) {
        $.log.info(`version: ${process.env.npm_package_version}`)
    });

    gulp.task('version-npm-preversion', function publish(cb) {
        $.log.info(`preversion: ${process.env.npm_package_version}`)
    });

    gulp.task('version-npm-version', function publish(cb) {
        $.log.info(`version: ${process.env.npm_package_version}`)
    });

    gulp.task('version-npm-postversion', function publish(cb) {
        $.log.info(`postversion: ${process.env.npm_package_version}`)
    });
}