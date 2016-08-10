module.exports = function (gulp, config, $) {
    gulp.task('compile', function compile(cb) {
        $.log.info(`Compiling solution...`);
        console.log();

        $.msbuild.exec(`/property:Configuration=${config.msbuild.configuration} /verbosity:${config.msbuild.verbosity}`);

        console.log();
        
        return cb();
    });
};