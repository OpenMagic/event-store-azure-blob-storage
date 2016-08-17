const path = require('path');

export function getAssemblyVersion() {
    const version = getNpmVersion();
    const hyphen = version.indexOf('-');

    if (hyphen === -1) {
        return version + '.0';
    }

    const preReleaseVersion = version.substring(hyphen + 1);

    return `${version.substring(0, hyphen)}.${preReleaseVersion}`;
}

export function getNpmVersion() {
    const packageObj = require(path.join(__dirname, '../package.json'));
    const version = packageObj.version;

    return version;
}

export function getNuGetVersion() {
    const version = getNpmVersion();
    const hyphen = version.indexOf('-');

    if (hyphen === -1) {
        return version + '.0';
    }

    const preReleaseVersion = version.substring(hyphen + 1);

    return `${version.substring(0, hyphen)}-pre${preReleaseVersion}`;
}
