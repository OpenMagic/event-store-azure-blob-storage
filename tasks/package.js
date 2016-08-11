module.exports = function (gulp, config, $) {
    gulp.task('package', $.dependencies(['test']), function package(cb) {

        $.log.info(`Creating NuGet packages for '${$.quote(config.nuget.nuspecs)}'`);

        $.mkdirp.sync(config.artifacts);

        config.nuget.nuspecs.forEach(function (nuspecPattern) {
            if (config.nuget.nuspecs.length > 1) {
                $.log.info(`Creating NuGet packages for '${$.quote(nuspecPattern)}'`);
            }

            const nuspecs = $.glob.sync(nuspecPattern);

            nuspecs.forEach(function (nuspec) {
                $.log.info(`Creating NuGet package for '${$.quote(nuspec)}'`);
                $.nuget.exec(`pack ${nuspec} -OutputDirectory ${config.artifacts} -Version ${$.versions.getNuGetVersion()} -Symbols -Properties "Configuration=${config.msbuild.configuration}"`);
            });
        });

        return cb();
    });
};