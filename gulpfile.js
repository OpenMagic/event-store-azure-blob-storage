//eval(require("typescript").transpile(require("fs").readFileSync("./gulpfile.ts").toString()));
const files = [
    "./tasks/clean.ts"
];
files.forEach(function(file) { eval(require("typescript").transpile(require("fs").readFileSync(file).toString())) });