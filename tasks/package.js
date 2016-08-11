module.exports = function (gulp, config, $) {
    gulp.task('package', ['test'], function package(cb) {

        throw 'todo: copy bin to artifacts'

        $.log.info(`Creating NuGet packages for '${$.quote(config.nuget.nuspecs)}'`);

        $.mkdirp.sync(config.artifacts);
        
        config.nuget.nuspecs.forEach(function (nuspecPattern) {
            $.log.info(`Creating NuGet packages for '${$.quote(nuspecPattern)}'`);

            const nuspecs = $.glob.sync(nuspecPattern);

            nuspecs.forEach(function (nuspec) {
                $.log.info(`Creating NuGet package for '${$.quote(nuspec)}'`);
                $.nuget.exec(`pack ${nuspec} -OutputDirectory ${config.artifacts} -Version ${config.versions.getNuGetVersion()} -Symbols`);
            });
        });

        return cb();
    });
};