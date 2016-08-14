export class Build {
    public execute(cb: Function) : void {
        console.log('Build');
        cb();
    }
}