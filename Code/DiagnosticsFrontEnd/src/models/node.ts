import { ConstraintType } from "./edge";

export class Node{
    public id: string;
    public name: string;
    public childNodes: Node[]

    public parentRelationshipType: ConstraintType;

    constructor(
        id: string,
        name: string
    ) {
        this.id = id;
        this.name = name;
        this.childNodes = [];
        this.parentRelationshipType = ConstraintType.Mandatory;
    }

    public addChildNodes(children: Node[]){
        this.childNodes = this.childNodes.concat(children);
    }

    public setParentRelType(type: ConstraintType){
        this.parentRelationshipType = type;
    }
}