import { ConstraintType, Edge, GraphEdge } from "./edge";
import { GraphNode, Node } from "./node";

export class FmGraphResult{
    public root: Node | undefined;
    public nodes: Node[] = [];
    public edges: Edge[] = [];

    private nodesById: { [key: string]: Node } = {};

    private hierarchyRelationshipTypes: Set<number>;

    constructor(nodes: any[], edges: any[]) {
        this.initializeNodes(nodes);
        this.initializeEdges(edges);
        this.root = this.nodes.find(n => !this.edges.some(e => e.to.some(to => to == n)));
        this.hierarchyRelationshipTypes = new Set<number>([0, 1, 2, 3]);
    }

    private initializeNodes(nodes: any[]){
        for(let i = 0; i < nodes.length; i++){
            let node = new Node(nodes[i].id, nodes[i].name)

            this.nodes.push(node)
            this.nodesById[node.id] = node;
        }
    }

    private initializeEdges(edges: any[]){
        for(let i = 0; i < edges.length; i++){
            let toNodeIds: string[] = edges[i].toNodes;

            let toNodes = toNodeIds.map(id => this.nodesById[id])
            let fromNode = this.nodesById[edges[i].fromNode];

            this.edges.push(new Edge(fromNode, toNodes, edges[i].type))
        }
    }

    public getNodeById(id: string){
        return this.nodesById[id];
    }

    public getGraphNodes(){
        return this.nodes.map<GraphNode>(n => new GraphNode(n.id, n.name));
    }

    public getEdgesHierarchy(){
        let result: GraphEdge[] = new Array();

        // Currently only assigns ID's
        // Could easily be made into assigning entire Node.
        this.edges.forEach(edge => {
            if (this.hierarchyRelationshipTypes.has(edge.relType)){
                edge.to.forEach(to => {
                    result.push(new GraphEdge(edge.from.id, to.id, edge.relType))
                });
            }
        });

        return result;
    }
    
    public getEdgesCrossTree(){
        let result: GraphEdge[] = new Array();
        console.log("Inside getEdgesCrossTree", this.edges)

        // Currently only assigns ID's
        // Could easily be made into assigning entire Node.
        this.edges.forEach(edge => {
            if (!this.hierarchyRelationshipTypes.has(edge.relType)){
                edge.to.forEach(to => {
                    result.push(new GraphEdge(edge.from.id, to.id, edge.relType))
                });
            }
        });

        return result;
    }

    public getNodeStructure(){
        if (!this.root){
            throw new Error("Node does not exist.");
        }

        return this.buildRecursively(this.root);
    }

    private buildRecursively(node: Node): NodeData{  
        const children: NodeData[] = [];
            
        // Recursively build child nodes for each child
        node.childNodes.forEach(child => {
            let childNode = this.buildRecursively(child);
            children.push(childNode);
        });
              
        let name = node.name;
        let id = node.id;
        let parentType = node.parentRelationshipType;
        return {
          name,
          id,
          parentType,
          children,
        };
    }
}

export interface NodeData {
  name: string;
  id:string;
  parentType: ConstraintType;
  children?: NodeData[];
}