/// <reference path="typings/index.d.ts" />

import * as path from "path";

export class Config {
    // todo: make readonly
    artifacts: string;
    clean: {
        directories: string[]
    };
    constants: string[];
    msbuild: {
        configuration: string,
        verbosity: string
    };
    nuget: {
        nuspecs: string[],
        outputDirectory: string,
        solutionPackages: [
            {
                name: string,
                version: string,
                excludeVersion: boolean
            }]
    };
    specflow: {
        cmd: string,
        projects: string[]
    };
    xunit: {
        cmd: string,
        assemblies: string[]
    };

    constructor() {

        const artifactsDirectory = "artifacts";
        const msbuild = {
            configuration: "Release",
            verbosity: "Minimal"
        };

        this.artifacts = artifactsDirectory;

        this.clean = {
            directories: [
                artifactsDirectory,
                `source/**/bin/${msbuild.configuration}`,
                `source/**/obj/${msbuild.configuration}`,
                `tests/**/bin/${msbuild.configuration}`,
                `tests/**/obj/${msbuild.configuration}`
            ]
        };

        this.constants = [
            "source/**/Constants.cs"
        ];

        this.msbuild = msbuild;

        this.nuget = {
            nuspecs: [
                "*.nuspec"
            ],
            outputDirectory: "packages",
            solutionPackages: [
                {
                    name: "xunit.runner.console",
                    version: "2.1",
                    excludeVersion: true
                }
            ]
        };

        this.specflow = {
            cmd: `${__dirname}/packages/SpecFlow.2.1.0/tools/specflow.exe`,
            projects: [
                "tests/**/*.Specifications.csproj"
            ]
        };

        this.xunit = {
            cmd: `${__dirname}/packages/xunit.runner.console/tools/xunit.console.exe`,
            assemblies: [
                `tests/**/bin/${msbuild.configuration}/*.Specifications.dll`,
                `tests/**/bin/${msbuild.configuration}/*.Tests.dll`
            ]
        };
    };
}
