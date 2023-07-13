export class FmGraphResult{
    public nodes: Node[];
    public edges: Edge[];

    constructor(nodes: Node[], edges: Edge[]) {
        this.nodes = nodes;
        this.edges = edges;
    }
}

export class Node{
    public name: string;
    public id: string;

    constructor(
        name: string,
        id: string
    ) {
        this.name = name;
        this.id = id;
    }
}

export class Edge{
    public from: string;
    public to: string;
    public relType: ConstraintType;

    constructor(from: string, to: string, type: ConstraintType) {
        this.from = from;
        this.to = to;
        this.relType = type;
    }
}    

export enum ConstraintType{
    Mandatory,
    Optional,
    Alternative,
    Or,

    Excludes,
    Requires,
}
