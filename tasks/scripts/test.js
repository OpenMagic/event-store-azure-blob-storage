module.exports = function (gulp, config, $) {
    gulp.task('test', $.dependencies(['compile']), function test(cb) {
        $.log.info(`Running tests for '${$.quote(config.xunit.assemblies)}'`);

        const assemblies = $.glob.sync(config.xunit.assemblies);

        assemblies.forEach(function (assembly) {
            $.log.info(`Running tests for '${$.quote(assembly)}'`);
            $.shell.exec(`${config.xunit.cmd} ${assembly}`);
        });

        return cb();
    });
};