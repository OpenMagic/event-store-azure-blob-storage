const config = {
    clean: {
        directories: [
            'artifacts',
            'source/**/bin/Release',
            'source/**/obj/Release',
            'tests/**/bin/Release',
            'tests/**/obj/Release'
        ]
    }
};

module.exports = config;