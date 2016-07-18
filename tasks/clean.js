"use strict";
var del = require('del');
function clean() {
    return del(['artifacts']);
}
exports.clean = clean;
