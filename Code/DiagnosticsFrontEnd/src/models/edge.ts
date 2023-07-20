import { Node } from './node'

export class Edge{
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