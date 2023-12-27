import { Node } from "./node";
import { Edge } from "./edge";

export class UnsatCoreResult{
    public unsatisfiableCore: string[];
    public nodes: Node[];    
    public edges: Edge[];
    public toRemove: ToRemoveResult[];

    constructor(
        unsatCore: string[],
        nodes: Node[],
        edges: Edge[],
        toRemove: ToRemoveResult[]
    ) {
        this.unsatisfiableCore = unsatCore;
        this.nodes = nodes;
        this.edges = edges;
        this.toRemove = toRemove;
    }

    public remove(name: string) : boolean {
        console.log("Remove", this.toRemove)
        for(let i = 0; i < this.toRemove.length; i++){
            if (this.toRemove[i].nodes.find(n => n.name === name))
            {
                return true;   
            }
        }

        return false;
    }
}

export class ToRemoveResult{
    public relationshipType: string;
    public nodes: Node[];

    constructor(
        relationshipType: string,
        nodes: Node[],
    ) {
        this.relationshipType = relationshipType;
        this.nodes = nodes;
    }
}