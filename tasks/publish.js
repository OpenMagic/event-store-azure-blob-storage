const git = require('git-rev-sync');
const isGitClean = require('is-git-clean');

module.exports = function (gulp, config, $) {
    gulp.task('publish', function publish(cb) {
        validateGitIsClean();

        const bump = config.argv.bump;

        validateBump(bump);

        const currentBranch = createAndCheckoutPublishBranch()

        try {
            updateVersion(bump);

            try {
                createPackage();
                mergePublishBranch(currentBranch);
                pushRepository();
            }
            catch (err) {
                rollbackTag();
            }
        }
        catch (err) {
            rollbackPublishBranch();
        }
        return cb();
    });

    function createAndCheckoutGitBranch(branchName) {
        $.log.info(`Creating branch '${$.quote(branchName)}'`);
        $.shell.exec(`git checkout -b ${branchName}`);
    }

    function createAndCheckoutPublishBranch() {
        throw 'todo';
        // const currentBranch = git.branch();

        // deleteGitBranchIfExists('publish')
        // createAndCheckoutGitBranch('publish');
    }

    function deleteGitBranchIfExists(branchName) {
        if ($.shell.exec(`git show-ref --verify --quiet refs/heads/${branchName}`).code !== 0) {
            return;
        }
        $.log.info(`Deleting branch '${$.quote(branchName)}'`);
        $.shell.exec(`git branch -d ${branchName}`);
    }

    // Update the version number
    function updateVersion(newVersion) {
        shell.exec(`npm version ${newVersion}`);
    }

    function validateBump(level) {
        const levels = 'major, minor, patch, premajor, preminor, prepatch, prerelease';

        if (levels.split(', ').indexOf(level) < 0) {
            throw `Usage: gulp publish --bump <level>\n\n    where <level> is one of:\n        ${levels}`;
        }
    }

    // Check git repository is clean. Throws exception if it isn't.
    function validateGitIsClean() {
        if (isGitClean.sync()) {
            return;
        }
        throw 'Git repository must be clean before running the requested task.';
    }
};