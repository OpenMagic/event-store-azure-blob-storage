/// <reference path="../../typings/index.d.ts" />

import * as chalk  from "chalk";

export class Log {
    private taskOperation = chalk.cyan;

    public info(message: string): void {
        console.log(message);
    }

    public error(message: string): void {
        console.log(chalk.red(message));
    }
    
    public quote(message: any): string {
        return chalk.yellow(message);
    }
}
