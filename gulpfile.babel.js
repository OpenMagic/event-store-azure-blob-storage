// import gulp from 'gulp';
// import HubRegistry from 'gulp-hub';

// var hub = new HubRegistry(['tasks/*.js']);

// gulp.registry(hub);

'use strict';

var gulp = require('gulp');
var HubRegistry = require('gulp-hub');

/* load some files into the registry */
var hub = new HubRegistry(['tasks/*.js']);

/* tell gulp to use the tasks just loaded */
gulp.registry(hub);