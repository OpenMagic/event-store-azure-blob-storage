/// <reference path="../../typings/index.d.ts" />

import * as chalk  from "chalk";

class Log {
    private taskOperation = chalk.cyan;

    public finishedTask(taskName: string): void {
        console.log(`Finished '${this.taskOperation(taskName)}'.`);
    }

    public info(message: string): void {
        console.log(message);
    }

    public startingTask(taskName: string): void {
        console.log(`Starting '${this.taskOperation(taskName)}'...`);
    }

    public quote(message: any): string {
        return chalk.yellow(message);
    }
}

export const log = new Log();