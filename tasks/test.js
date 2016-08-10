module.exports = function (gulp, config, $) {
    gulp.task('test', ['compile'], function compile(cb) {
        $.log.info(`Running tests for '${$.quote(config.xunit.assemblies)}'`);

        config.xunit.assemblies.forEach(function (assemblyPattern) {
            $.log.info(`Running tests for '${$.quote(assemblyPattern)}'`);

            const assemblies = $.glob.sync(assemblyPattern);

            assemblies.forEach(function (assembly) {
                $.log.info(`Running tests for '${$.quote(assembly)}'`);
                $.shell.exec(`${config.xunit.cmd} ${assembly}`);
            });
        });

        return cb();
    });
};