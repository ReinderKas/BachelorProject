export class Node{
    public id: string;
    public name: string;
    public childNodes: Node[]

    constructor(
        id: string,
        name: string
    ) {
        this.id = id;
        this.name = name;
        this.childNodes = [];
    }

    public addChildNodes(children: Node[]){
        this.childNodes = this.childNodes.concat(children);
    }
}