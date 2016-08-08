const config = require('../config');
const del = require('del');
const log = require('gulplog');
const quote = require('../scripts/quote');

module.exports = function clean() {
    log.info(`Deleting '${quote(config.clean.directories)}'`);
    return del(config.clean.directories);
}
