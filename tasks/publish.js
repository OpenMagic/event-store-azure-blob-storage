const isGitClean = require('is-git-clean');

module.exports = function (gulp, config, $) {
    gulp.task('publish', function publish(cb) {
        validateGitIsClean();

        const newVersion = getNewVersionArgument($);
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

    // Check git repository is clean. Throws exception if it isn't.
    function validateGitIsClean() {
        if (isGitClean.sync()) {
            return;
        }
        throw 'Git repository must be clean before running the requested task.';
    }
};

