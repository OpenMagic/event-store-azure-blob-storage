import { Log } from "../scripts/gulp/log";
import { Shell } from "../scripts/gulp/shell";

const argv = require('yargs').argv;
const git = require("git-rev-sync");
const isGitClean = require("is-git-clean");

export class Publisher {

    constructor(private shell: Shell, private log: Log) {
    }

    static publish(shell: Shell, log: Log, cb: Function): any {
        return new Publisher(shell, log).publishImpl(cb);
    }

    private publishImpl(cb: Function): any {
        this.validateGitIsClean();

        const bump = argv.bump;

        this.validateBump(bump);

        const currentBranch = this.createAndCheckoutPublishBranch()

        try {
            this.updateVersion(bump);

            try {
                this.createPackage();
                this.mergePublishBranch(currentBranch);
                this.pushRepository();
            }
            catch (err) {
                this.rollbackTag();
            }
        }
        catch (err) {
            this.rollbackPublishBranch();
        }
        return cb();
    }

    private createAndCheckoutGitBranch(branchName: string) {
        this.log.info(`Creating branch "${this.log.quote(branchName)}"`);
        throw new Error("todo");
        //this.shell.exec(`git checkout -b ${branchName}`);
    }

    private createAndCheckoutPublishBranch(): string {
        throw "todo";
        // const currentBranch = git.branch();

        // deleteGitBranchIfExists("publish")
        // createAndCheckoutGitBranch("publish");
    }

    private createPackage() {
        throw new Error("todo");
    }

    private deleteGitBranchIfExists(branchName: string) {
        throw new Error("todo");
        // if (this.shell.exec(`git show-ref --verify --quiet refs/heads/${branchName}`).code !== 0) {
        //     return;
        // }
        // this.log.info(`Deleting branch "${$.quote(branchName)}"`);
        // this.shell.exec(`git branch -d ${branchName}`);
    }

    private mergePublishBranch(branchName: string) {
        throw new Error("todo");
    }

    private pushRepository() {
        throw new Error("todo");
    }

    private rollbackPublishBranch() {
        throw new Error("todo");
    }
    
    private rollbackTag() {
        throw new Error("todo");
    }    

    // Update the version number
    private updateVersion(newVersion: string) {
        throw new Error("todo");
        // this.shell.exec(`npm version ${newVersion}`);
    }

    private validateBump(level: string) {
        const levels = "major, minor, patch, premajor, preminor, prepatch, prerelease";

        if (levels.split(", ").indexOf(level) < 0) {
            throw `Usage: gulp publish --bump <level>\n\n    where <level> is one of:\n        ${levels}`;
        }
    }

    // Check git repository is clean. Throws exception if it isn"t.
    private validateGitIsClean() {
        if (isGitClean.sync()) {
            return;
        }
        throw "Git repository must be clean before running the requested task.";
    }
}
