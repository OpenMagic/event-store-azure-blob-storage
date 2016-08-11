const git = require('git-rev-sync');
const isGitClean = require('is-git-clean');

module.exports = function (gulp, config, $) {
    gulp.task('publish', function publish(cb) {
        validateGitIsClean();

        const bump = config.argv.bump;
        const currentBranch = git.branch();

        deleteGitBranchIfExists('publish')
        createAndCheckoutGitBranch('publish');
        updateVersion(newVersion);
        createPackage();
        mergePublishBranch(currentBranch);

        // Push the latest commits and related tags to remote server
        //shell.exec(`git push --follow-tags`);
        pushRepository();

        return cb();
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

    // Check git repository is clean. Throws exception if it isn't.
    function validateGitIsClean() {
        if (isGitClean.sync()) {
            return;
        }
        throw 'Git repository must be clean before running the requested task.';
    }
};

