const argv = require('yargs').argv;
const git = require("git-rev-sync");
const isGitClean = require("is-git-clean");

export class Publisher {

    publish(cb: Function): any {
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

    private createAndCheckoutGitBranch(branchName) {
        $.log.info(`Creating branch "${$.quote(branchName)}"`);
        $.shell.exec(`git checkout -b ${branchName}`);
    }

    private createAndCheckoutPublishBranch() {
        throw "todo";
        // const currentBranch = git.branch();

        // deleteGitBranchIfExists("publish")
        // createAndCheckoutGitBranch("publish");
    }

    private create
    private deleteGitBranchIfExists(branchName) {
        if ($.shell.exec(`git show-ref --verify --quiet refs/heads/${branchName}`).code !== 0) {
            return;
        }
        $.log.info(`Deleting branch "${$.quote(branchName)}"`);
        $.shell.exec(`git branch -d ${branchName}`);
    }

    // Update the version number
    private updateVersion(newVersion) {
        shell.exec(`npm version ${newVersion}`);
    }

    private validateBump(level) {
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
