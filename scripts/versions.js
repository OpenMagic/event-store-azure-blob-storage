const path = require('path');

module.exports.getNpmVersion = getNpmVersion;
module.exports.getNuGetVersion = getNuGetVersion;

function getNpmVersion() {
    const package = require(path.join(__dirname, '../package.json'));
    const version = package.version;

    return version;
}

function getNuGetVersion() {
    const version = getNpmVersion();
    const hyphen = version.indexOf('-');

    if (hyphen === -1) {
        return version + '.0';
    }

    const preReleaseVersion = version.substring(hyphen + 1);

    return `${version.substring(0, hyphen)}-pre${preReleaseVersion}`;
}
