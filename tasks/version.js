module.exports = function (gulp, config, $) {
    gulp.task('version', function version(cb) {
        $.log.info(`version: ${process.env.npm_package_version}`)
        $.shell.exec('npm version patch');
        cb();
    });

    gulp.task('version-npm-preversion', function version_npm_preversion(cb) {
        console.log(`npm-preversion: ${process.env.npm_package_version}`)
        $.log.info('ddddddddddd');
        $.log.info(`npm-preversion: ${process.env.npm_package_version}`)
        cb();
    });

    gulp.task('version-npm-version', function version_npm_version(cb) {
        $.log.info(`npm-version: ${process.env.npm_package_version}`)
        cb();
    });

    gulp.task('version-npm-postversion', function version_npm_postversion(cb) {
        $.log.info(`npm-postversion: ${process.env.npm_package_version}`)
        cb();
    });
}