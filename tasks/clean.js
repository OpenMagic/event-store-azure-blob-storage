var config = require('../config');
var del = require('del');
var log = require('gulplog');
var quote = require('../scripts/quote');

module.exports = function() {
  log.info(`Deleting '${quote(config.clean.directories)}'`);
  return del(config.clean.directories);
}
