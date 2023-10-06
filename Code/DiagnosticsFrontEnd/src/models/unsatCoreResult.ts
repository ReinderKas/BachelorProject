import { Node } from "./node";
import { Edge } from "./edge";

export class UnsatCoreResult{
    public unsatisfiableCore: string[];
    public nodes: Node[];    
    public edges: Edge[];

    constructor(
        unsatCore: string[],
        nodes: Node[],
        edges: Edge[]
    ) {
        this.unsatisfiableCore = unsatCore;
        this.nodes = nodes;
        this.edges = edges;
    }
}