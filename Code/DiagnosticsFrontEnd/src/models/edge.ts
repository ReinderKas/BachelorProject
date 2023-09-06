import { Node } from './node'

export class Edge {
    public from: Node;
    public to: Node[];
    public relType: ConstraintType;

    public isNonXTree: boolean;

    constructor(from: Node, to: Node[], type: ConstraintType) {
        this.from = from;
        this.to = to;
        this.relType = type;

        this.isNonXTree = this.relType == ConstraintType.Mandatory
                        || this.relType == ConstraintType.Optional
                        || this.relType == ConstraintType.Alternative
                        || this.relType == ConstraintType.Or;

        if (this.isNonXTree){
            from.addChildNodes(to);

            to.forEach(n => {
                n.setParentRelType(type);
            });
        }
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

export class GraphEdge{
    public fromId: string;
    public toId: string;
    public relType: number;

    constructor(
        fromId: string,
        toId: string,
        relType: number
    ) {
        this.fromId = fromId;
        this.toId = toId;
        this.relType = relType;
    }
}