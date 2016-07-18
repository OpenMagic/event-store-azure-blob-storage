import del = require('del')

export function clean() {
  return del([ 'artifacts' ]);
}
