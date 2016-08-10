module.exports = function (gulp, config, $) {
    gulp.task('package', [/*todo 'test'*/], function package(cb) {
        $.log.info(`Creating NuGet package...`);
        
        return cb();
    });
};