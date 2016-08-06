var gulp = require('gulp');
var taskListing = require('gulp-task-listing');
gulp.task('default', ['help']);
gulp.task('help', taskListing);
require('gulp-load-tasks')();
