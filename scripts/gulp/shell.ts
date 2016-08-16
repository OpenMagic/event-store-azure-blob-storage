/// <reference path="../../typings/index.d.ts" />

import * as shell from "shelljs";
import { Log } from "./log";

export class Shell {
    private log: Log;

    constructor(log: Log) {
        this.log = log;
    }

    public exec(cmd: string, startingMessage: string, successfulMessage: string, failureMessage: string): void {
        this.log.info(startingMessage);

        const result = shell.exec(cmd);

        if (result.code == 0) {
            this.log.info(successfulMessage)
            return;
        }

        throw new Error(failureMessage);
    }
}
