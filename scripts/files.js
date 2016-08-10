var fs = require('fs');
var path = require('path');

module.exports.getSpecFlowProjects = function (testsDirectory) {
    const directories = getSpecFlowDirectories(testsDirectory);
    const projects = directories.map(directory => getProject(directory));

    // projects.forEach(project => console.log(project));

    return projects;
}

module.exports.getTestAssemblies = function (testsDirectory, configuration) {
    const assemblies = getDirectories(testsDirectory)
        .map(directory => getTestAssembly(directory, configuration));

    // assemblies.forEach(project => console.log(project));

    return assemblies;
}

function getDirectories(directory) {
    return fs.readdirSync(directory)
        .filter(file => fs.statSync(path.join(directory, file)).isDirectory())
        .map(file => path.join(directory, file));
}

function getProject(directory) {
    const files = fs.readdirSync(directory).filter(file => path.extname(file) === '.csproj');

    if (files.length === 1) {
        return path.join(directory, files[0]);
    }

    throw `Expected to find 1 project in '${directory}', not ${files.length}.`;
}

function getSpecFlowDirectories(testsDirectory) {
    return getDirectories(testsDirectory)
        .filter(directory => directory.endsWith('.Specifications'));
}

function getTestAssembly(directory, configuration) {
    return path.join(directory, `bin/${configuration}/${path.basename(directory)}.dll`);
}