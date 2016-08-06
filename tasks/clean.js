const log = require('gulplog');
const config = require('../config');
const del = require('del');
const chalk = require('chalk');

module.exports = function() {
  log.info(`Deleting '${chalk.cyan(config.clean.directories)}'`);
  return del(config.clean.directories);
}
