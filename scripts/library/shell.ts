/// <reference path="../../typings/index.d.ts" />

import * as shell from "shelljs";
import { log } from "./log";

export function exec(cmd: string, startingMessage: string, successfulMessage: string, failureMessage: string) : void {
    log.info(startingMessage);
    
    const result = shell.exec(cmd);

    if (result.code == 0) {
        log.info(successfulMessage)
        return;
    }

    throw new Error(failureMessage);
}