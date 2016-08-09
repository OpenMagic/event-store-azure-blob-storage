module.exports = function (gulp, config, $) {
    gulp.task('test', ['compile'], function compile(cb) {
        $.shell.exec(`${config.xunit.cmd} ${config.xunit.assemblies}`);
        return cb();
    });
};