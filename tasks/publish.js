const git = require('git-rev-sync');
const isGitClean = require('is-git-clean');

module.exports = function (gulp, config, $) {
    gulp.task('publish', function publish(cb) {
        validateGitIsClean();

        const bump = config.argv.bump;

        validateBump(bump);

        throw 'todo';
        // const currentBranch = git.branch();

        // deleteGitBranchIfExists('publish')
        // createAndCheckoutGitBranch('publish');
        // updateVersion(newVersion);
        // createPackage();
        // mergePublishBranch(currentBranch);

        // // Push the latest commits and related tags to remote server
        // //shell.exec(`git push --follow-tags`);
        // pushRepository();

        // return cb();
    });

    function createAndCheckoutGitBranch(branchName) {
        $.log.info(`Creating branch '${$.quote(branchName)}'`);
        $.shell.exec(`git checkout -b ${branchName}`);
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

    function validateBump() {
        
        throw `Usage: gulp publish --bump <level>\n\n    where <level> is one of:\n        major, minor, patch, premajor, preminor, prepatch, prerelease, from-git`;
    }

    // Check git repository is clean. Throws exception if it isn't.
    function validateGitIsClean() {
        if (isGitClean.sync()) {
            return;
        }
        throw 'Git repository must be clean before running the requested task.';
    }
};