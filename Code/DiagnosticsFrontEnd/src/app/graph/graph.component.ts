import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import * as d3 from 'd3';
import { GraphEdge } from 'src/models/edge';
import { FmGraphResult } from 'src/models/fmGraphResult';
import { GraphNode } from 'src/models/node';

@Component({
  selector: 'app-graph',
  templateUrl: './graph.component.html',
  styleUrls: ['./graph.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class GraphComponent implements OnInit {
  @Input() fmGraph: FmGraphResult | null = null;

  private graphNodes: GraphNode[];
  private graphEdges: GraphEdge[];

  // Dimensions / styling.
  private margin =  {top: 20, right: 90, bottom: 30, left: 90};
  private width = 1000 - this.margin.left - this.margin.right;
  private height = 1000 - this.margin.top - this.margin.bottom;

  constructor() {
    this.graphNodes = new Array();
    this.graphEdges = new Array();
  }

  ngOnInit() {
    if (!this.fmGraph){
      alert("No Feature Model data given to create a Graph for.")
      return;
    }
    
    // Initialize the necessary data.
    this.graphNodes = this.fmGraph.getVisualizationNodes();
    this.graphEdges = this.fmGraph.GetVisualizationEdges();

    this.createGraph();
  }

  createGraph(){
    // Create necessary data layouts.
    let tree = this.generateTree();
    let stratify = this.generateStratify();
    let root = tree(stratify(this.graphNodes))

    console.log("Root:");
    console.log(root);

    // Render the data.
    this.renderGraph(root);
  }


  //#region Data Layouts
  // Create a tree layout.
  private generateTree(){
    return d3.tree<GraphNode>().size([
      this.width - this.margin.left - this.margin.right,
      this.height - this.margin.top - this.margin.bottom
    ])
  }


  // Create a stratify layout.
  private generateStratify(){
    return d3.stratify<GraphNode>()
                .id(d => d.id)
                .parentId(d => {
                  let edge = this.graphEdges.find(e => e.toId.includes(d.id));
                  return edge ? edge.fromId : null;
                })
  }
  //#endregion


  //#region Data Visualization
  // Render the Graph with the given data. 
  private renderGraph(root: d3.HierarchyNode<GraphNode>)
  {
    // Create SVG (Scalable Vector Graphics) container
    let svg = d3.select('.graph')
                      .append('svg')
                      .attr('width', this.width)
                      .attr('height', this.height);


    // Move the starting point from the edge.
    let graph = svg.append('g')
                    .attr('transform', `translate(${this.margin.left},${this.margin.top})`);


    this.addLinks(graph, root)
    this.addNodes(graph, root)
  }


  private addLinks(svg: any, root: any){
    // Add links between the nodes.
    svg.selectAll(".link")
      .data(root.links())
      .enter()
      .append("path")
      .attr("class", "link")
         
      
      // Straight lines.
      .attr("d", function(d: { source: { y: string; x: string; }; target: { y: string; x: string; }; }) {
        return "M" + d.source.x + "," + d.source.y
          + "L" + d.target.x + "," + d.target.y;
      })
  }

  private addNodes(svg: any, root: any){
    // Add each node as a group.
    var node = svg.selectAll(".node")
        .data(root.descendants())
        .enter()
        .append("g")
        .attr("class", function(d: { children: any; }) { 
          return "node" + 
            (d.children ? " node--internal" : " node--leaf"); })
        .attr("transform", function(d: { x: string; y: string; }) { 
          return "translate(" + d.x + "," + d.y + ")"; });

    // Adds the circle to the node
    node.append("circle")
        .attr("r", 15);

    // Add text to the node.
    node.append("text")
        .attr("dy", ".35em")
        .attr("x", function(d: { children: any; }) { return d.children ? -13 : 13; })
        .style("text-anchor", "middle")
        .text(function(d: { data: { name: any; }; }) { return d.data.name; });
  }
  
  //#endregion
}