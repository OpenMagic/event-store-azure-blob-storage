var lib = require('./library');
var log = require('gulplog');
var config = require('../config');
var del = require('del');
var chalk = require('chalk');

module.exports = function() {
  log.info(`Deleting '${chalk.cyan(config.clean.directories)}'`);
  return del(config.clean.directories);
}
