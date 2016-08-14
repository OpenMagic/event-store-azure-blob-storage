module.exports = function (gulp, config, $) {
    gulp.task('build-clean', function clean() {
        $.log.info(`Deleting '${$.quote(config.clean.directories)}'`);
        
        return $.del(config.clean.directories);
    });
};