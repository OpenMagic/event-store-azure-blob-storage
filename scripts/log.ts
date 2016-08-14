/// <reference path="../typings/index.d.ts" />

import * as chalk  from "chalk";

class Log {
    private taskOperation = chalk.cyan;

    public finishedTask(taskName): void {
        console.log(`Finished '${this.taskOperation(taskName)}'.`);
    }

    public startingTask(taskName): void {
        console.log(`Starting '${this.taskOperation(taskName)}'...`);
    }
}

export const log = new Log();