var fs = require('fs');
var path = require('path');

const msbuild = {
    configuration: 'Release',
    verbosity: 'Minimal'
};

const config = {
    clean: {
        directories: [
            'artifacts',
            'source/**/bin/Release',
            'source/**/obj/Release',
            'tests/**/bin/Release',
            'tests/**/obj/Release'
        ]
    },
    msbuild: msbuild,
    specflow: {
        cmd: `${__dirname}/packages/SpecFlow.2.1.0/tools/specflow.exe`,
        projects: getSpecFlowProjects()
    },
    xunit: {
        cmd: `${__dirname}/packages/xunit.runner.console/tools/xunit.console.exe`,
        assemblies: getTestAssemblies()
    }
};

function getProject(directory) {
    const files = fs.readdirSync(directory).filter(function (file) {
        return path.extname(file) === '.csproj';
    });

    if (files.length === 1) {
        return path.join(directory, files[0]);
    }

    throw `Expected to find 1 project in '${directory}', not ${files.length}.`;
}

function getSpecFlowDirectories() {
    return getDirectories(path.join(__dirname, 'tests'))
        .filter(function (directory) { return directory.endsWith('.Specifications') });
}

function getSpecFlowProjects() {
    const directories = getSpecFlowDirectories();
    const projects = directories.map(function (directory) { return getProject(directory); });

    // projects.forEach(function(project) { console.log(project); });

    return projects;
}

function getTestAssemblies() {
    throw 'todo';
}

function getDirectories(srcpath) {
    return fs.readdirSync(srcpath)
        .filter(function (file) { return fs.statSync(path.join(srcpath, file)).isDirectory(); })
        .map(function (file) { return path.join(srcpath, file); });
}

module.exports = config;