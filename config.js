module.exports = {
    clean: {
        directories: [
            'artifacts',
            'source/**/bin/Release',
            'source/**/obj/Release',
            'tests/**/bin/Release',
            'tests/**/obj/Release'
        ]
    },
    msbuild: {
        configuration: 'Release',
        verbosity: 'Minimal'
    }
};