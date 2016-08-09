module.exports = function (gulp, options, plugins) {
    gulp.task('clean', function clean() {
        log.info(`Deleting '${quote(config.clean.directories)}'`);
        return del(config.clean.directories);
    });
};