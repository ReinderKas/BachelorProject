import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import * as d3 from 'd3';
import { ExampleModels } from 'src/models/exampleModels';
import { FmGraphResult, NodeData } from 'src/models/fmGraphResult';

@Component({
  selector: 'app-graph',
  templateUrl: './graph.component.html',
  styleUrls: ['./graph.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class GraphComponent implements OnInit {
  @Input() fmGraph: FmGraphResult | null = null;

  // Dimensions / styling.
  private margin =  {top: 20, right: 90, bottom: 30, left: 90};
  private width = 960 - this.margin.left - this.margin.right;
  private height = 500 - this.margin.top - this.margin.bottom;

  constructor() {}

  ngOnInit() {
    this.createGraph();
    console.log("Initializing with Graph Data: ");
    console.log(this.fmGraph);
  }

  createGraph(){
    // Create the SVG.
    let svg = this.createSvg();

    if (!this.fmGraph){
      alert("No Feature Model data given to create a Graph for.")
      return;
    }

    // Setup data.
    let structure = this.fmGraph.getNodeStructure();

    let root: TreeNode = d3.hierarchy<NodeData>(structure, d => d.children) as TreeNode;
    d3.tree<NodeData>().size([this.width, this.height])(root);

    // Fill the SVG with data.
    this.addLinks(svg, root);
    this.addNodes(svg, root);    
  }


  private createSvg(){
    // Create the svg (Scalable Vector Graphics) on which to work
    // Clear the svg
    d3.select(".graph").selectAll("*").remove();

    // Append the svg object to the body of the page
    return d3.select(".graph").append("svg")
      .attr("width", this.width + this.margin.right + this.margin.left)
      .attr("height", this.height + this.margin.top + this.margin.bottom)
      .append("g")
      .attr("transform", "translate("
            + this.margin.left + "," + this.margin.top + ")");
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
}

interface TreeNode extends d3.HierarchyNode<NodeData> {
  x?: number;
  y?: number;
}
