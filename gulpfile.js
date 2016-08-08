var gulp = require('gulp');
var gulpSequence = require('gulp-sequence');

// Load gulp tasks in ./tasks
require('gulp-load-tasks')();

gulp.task('build', gulpSequence('clean', 'compile'));