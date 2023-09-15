import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import * as d3 from 'd3';
import { GraphEdge } from 'src/models/edge';
import { FmGraphResult } from 'src/models/fmGraphResult';
import { GraphNode } from 'src/models/node';
import { GraphDrawer } from './graphDrawer';

@Component({
  selector: 'app-graph',
  templateUrl: './graph.component.html',
  styleUrls: ['./graph.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class GraphComponent implements OnInit {
  @Input() fmGraph: FmGraphResult | null = null;

  private nodes: GraphNode[];
  private edgesHierarchy: GraphEdge[];
  private edgesCrossTree: GraphEdge[];

  private graphDrawer: GraphDrawer;

  private svg: d3.Selection<SVGSVGElement, unknown, HTMLElement, any>;

  // Dimensions / styling.
  private margin =  {top: 20, right: 90, bottom: 30, left: 90};
  private width = 1000 - this.margin.left - this.margin.right;
  private height = 1000 - this.margin.top - this.margin.bottom;

  constructor() {
    this.nodes = new Array();
    this.edgesHierarchy = new Array();
    this.edgesCrossTree = new Array();
    this.svg = d3.select('.graph')
                  .append('svg')
                  .attr('width', this.width)
                  .attr('height', this.height);

    this.graphDrawer = new GraphDrawer(this.svg);
  }

  ngOnInit() {
    if (!this.fmGraph){
      alert("No Feature Model data given to create a Graph for.")
      return;
    }
    
    // Initialize the necessary data.
    this.nodes = this.fmGraph.getGraphNodes();
    this.edgesHierarchy = this.fmGraph.getEdgesHierarchy();
    this.edgesCrossTree = this.fmGraph.getEdgesCrossTree();

    this.createGraph();
  }

  createGraph(){
    // Create necessary data layouts.
    let tree = this.generateTree();
    let stratify = this.generateStratify();
    let root = tree(stratify(this.nodes))

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
                  let edge = this.edgesHierarchy.find(e => e.toId.includes(d.id));
                  return edge ? edge.fromId : null;
                })
  }
  //#endregion


  //#region Data Visualization
  // Render the Graph with the given data. 
  private renderGraph(root: d3.HierarchyNode<GraphNode>)
  {
    // Create SVG (Scalable Vector Graphics) container
    let 


    // Move the starting point from the edge.
    let graphSvg = svg.append('g')
                    .attr('transform', `translate(${this.margin.left},${this.margin.top})`);


    this.addLinks(graphSvg, root)
    this.addNodes(graphSvg, root)
    this.addCrossLinks(graphSvg, root)
    this.addTooltip(graphSvg, root);
    this.addLegend(graphSvg);
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

  private addCrossLinks(svg: any, root:any){
    // Define arrow markers for cross-tree constraints
    svg.append("defs").selectAll("marker")
      .data(["end"]) 
      .enter().append("marker")
      .attr("id", String)
      .attr("viewBox", "0 -5 10 10")
      .attr("refX", 15)
      .attr("refY", 0)
      .attr("markerWidth", 6)
      .attr("markerHeight", 6)
      .attr("orient", "auto")
      .append("path")
      .attr("d", "M0,-5L10,0L0,5")
      .attr("fill", "red");  // Color of the arrow

    // Cross-tree constraints visualization
    svg.selectAll(".constraint")
      .data(this.edgesCrossTree)
      .enter()
      .append("line")
      .attr("class", "constraint")
      // ... (x1, y1, x2, y2 calculations)
      .style("stroke", "red")  // Cross-tree constraints in red for emphasis
      .style("stroke-dasharray", ("3, 3"))  // Dashed line for cross-tree constraints
      .attr("marker-end", "url(#end)");  // Arrow marker
  }

  private addNodes(svg: any, root: any){
    // Add each node as a group.
    var node = svg.selectAll(".node")
        .data(root.descendants())
        .enter()
        .append("g")

    // Assign correct class
        .attr("class", function(d: { children: any; }) { 
          return "node" + 
            (d.children ? " node--internal" : " node--leaf"); })

    // Assign correct position
        .attr("transform", function(d: { x: string; y: string; }) { 
          return "translate(" + d.x + "," + d.y + ")"; })


    // Adds the circle to the node
    node.append("circle")
        .attr("r", 15);

    // Add text to the node.
    node.append("text")
        .attr("dy", ".35em")
        .style("text-anchor", "middle")
        .text(function(d: { data: { name: any; }; }) { return d.data.name; });
  }

  private addTooltip(svg: any, root: any){
    const tooltip = d3.select("body").append("div")
    .attr("class", "tooltip");

    svg.selectAll(".constraint-hitbox")
    .data(root.links())
    .enter()
    .append("line")
    .attr("class", "constraint-hitbox")
    .style("stroke", "transparent")
    .style("stroke-width", "10px")
    .on("mouseover", function(event: { pageX: number; pageY: number; }, d: { source: any; target: any; }) {
        tooltip.transition()
            .duration(100)
            .style("opacity", .9);
        tooltip.html(`[${d.source.data.name}] -> [${d.target.data.name}]`)
            .style("left", (event.pageX + 5) + "px")
            .style("top", (event.pageY - 28) + "px");
    })
    .on("mouseout", function(d: any) {
        tooltip.transition()
            .duration(1000)
            .style("opacity", 0.2);
    });
  }

  private addLegend(svg: any){
    const legend = svg.append("g")
    .attr("transform", "translate(20,20)");  // Adjust this for legend's position

      // For hierarchical constraints
      legend.append("line")
          .attr("x1", 0)
          .attr("y1", 0)
          .attr("x2", 20)
          .attr("y2", 0)
          .style("stroke", "black");

      legend.append("text")
          .attr("x", 30)
          .attr("y", 5)
          .text("Hierarchical Constraint")
          .attr("alignment-baseline", "middle");

      // For cross-tree constraints
      legend.append("line")
          .attr("x1", 0)
          .attr("y1", 20)
          .attr("x2", 20)
          .attr("y2", 20)
          .style("stroke", "red")
          .style("stroke-dasharray", ("3, 3"));

      legend.append("text")
          .attr("x", 30)
          .attr("y", 25)
          .text("Cross-tree Constraint")
          .attr("alignment-baseline", "middle");
  }

  //#endregion
}