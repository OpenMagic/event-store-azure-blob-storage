/// <reference path="../../typings/index.d.ts" />

import * as chalk  from "chalk";

export class Log {
    private logger = require("gulplog");

    public debug(message: string): void {
        this.logger.debug(chalk.gray(message));
    }

    public error(message: string): void {
        this.logger.error(chalk.red(message));
    }
    
    public info(message: string): void {
        this.logger.info(message);
    }

    public quote(message: any): string {
        return chalk.yellow(message);
    }

    public warn(message: string): void {
        this.logger.warn(chalk.yellow(message));
    }
}
