import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import { NumberValueAccessor } from '@angular/forms';
import * as d3 from 'd3';
import { ExampleModels } from 'src/models/exampleModels';
import { FmGraphResult } from 'src/models/fmGraphResult';

@Component({
  selector: 'app-graph',
  templateUrl: './graph.component.html',
  styleUrls: ['./graph.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class GraphComponent implements OnInit {
  @Input() graphData: FmGraphResult[] | null = null; // add this line

  // Dimensions / styling.
  private margin =  {top: 20, right: 90, bottom: 30, left: 90};
  private width = 960 - this.margin.left - this.margin.right;
  private height = 500 - this.margin.top - this.margin.bottom;

  constructor() {}

  ngOnInit() {
    this.createGraph();
    console.log("Initializing Graph: " + this.graphData);
  }

  createGraph(){
    // Create the SVG.
    let svg = this.createSvg();

    // Setup data.
    let root: TreeNode = d3.hierarchy<NodeData>(ExampleModels.phoneModel, d => d.children) as TreeNode;
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
    let rectSize = 25;
    let padding = 10;

    let labels = node.append("rect")
      .attr("width", rectSize + padding)
      .attr("height", rectSize + padding)
      .attr("x", -rectSize/2 - padding/2)  // Shift the square left by half its width
      .attr("y", -rectSize/2 - padding/2)  // Shift the square up by half its height
      .attr("rx", 2)  // Horizontal corner radius
      .attr("ry", 2)  // Vertical corner radius

    labels.each((d: any) => {
      let bbox = this.getBBox();
      d3.select(this.parentNode).select("rect")
        .attr("width", bbox.width + 10)  // Add some padding
        .attr("height", bbox.height + 10)
        .attr("x", -bbox.width / 2 - 5)  // Center the rectangle on the text
        .attr("y", -bbox.height / 2 - 5);
    });
          
    // // Adds the circle to the node
    // node.append("circle")
    //     .attr("r", 10);

    // Add text to the node.
    node.append("text")
        .attr("dy", ".35em")
        .attr("x", function(d: { children: any; }) { return d.children ? -13 : 13; })
        .style("text-anchor", "middle")
        
        // .style("text-anchor", function(d: { children: any; }) { 
        //     return d.children ? "end" : "start"; })
        .text(function(d: { data: { name: any; }; }) { return d.data.name; });
  }
}
interface TreeNode extends d3.HierarchyNode<NodeData> {
  x?: number;
  y?: number;
}

interface NodeData {
  name: string;
  children?: NodeData[];
}