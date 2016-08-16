/// <reference path="../../typings/index.d.ts" />

import * as chalk  from "chalk";
const logger = require("gulplog");

export class Log {
    private taskOperation = chalk.cyan;

    public info(message: string): void {
        logger.info(message);
    }

    public error(message: string): void {
        logger.error(message);
    }
    
    public quote(message: any): string {
        return chalk.yellow(message);
    }
}
