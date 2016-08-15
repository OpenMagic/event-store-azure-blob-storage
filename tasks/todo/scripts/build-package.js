module.exports = function (gulp, config, $) {
    gulp.task('build-package', $.dependencies(['test']), function package(cb) {

        $.log.info(`Creating NuGet packages for '${$.quote(config.nuget.nuspecs)}'`);

        $.mkdirp.sync(config.artifacts);

        const nuspecs = $.glob.sync(config.nuget.nuspecs);

        nuspecs.forEach(function (nuspec) {
            $.log.info(`Creating NuGet package for '${$.quote(nuspec)}'`);
            $.nuget.exec(`pack ${nuspec} -OutputDirectory ${config.artifacts} -Version ${$.versions.getNuGetVersion()} -Symbols -Properties "Configuration=${config.msbuild.configuration}"`);
        });

        return cb();
    });
};