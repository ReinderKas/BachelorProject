import { Component, ViewEncapsulation, Input, ElementRef, Renderer2 } from '@angular/core';
import { FmGraphResult } from 'src/models/fmGraphResult';
import { AbstractGraphComponent } from './abstractGraph.component';
import { HierarchyPointNode } from 'd3-hierarchy';
import { GraphNode } from 'src/models/node';
import { GraphEdge } from 'src/models/edge';

@Component({
  selector: 'app-graph',
  templateUrl: './graph.component.html',
  styleUrls: ['./graph.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class GraphComponent extends AbstractGraphComponent {
  @Input() fmGraph: FmGraphResult | null = null;
  @Input() componentHeight: number = 500;
  @Input() componentWidth: number = 500;
  @Input() nodeSize: number = 10;
  @Input() nodeSpacing: number = 1;

  constructor(
    protected override el: ElementRef, 
    protected override renderer: Renderer2
  ) {
    super(el, renderer)
  }

  //#region Draw the Nodes
  // Visualize the Nodes.
  protected initNodes(){
      // Add each node as a group.
      this.node = this.svg.selectAll(".node")
          .data(this.root.descendants())
          .enter()
          .append("g")

          
      // Assign correct position
          .attr("transform", (d: { x: string; y: string; }) => "translate(" + parseInt(d.x) * this.nodeSpacing + "," + d.y + ")")
          
      // Adds the circle to the node 
      this.node.append("circle")
          .attr("r", this.nodeSize)
          .attr("class", (d: HierarchyPointNode<GraphNode>) => this.getNodeClass(d))
          
      // Add text to the node.
      this.node.append("text")
          .attr("dy", ".35em")
          .style("text-anchor", "middle")
          .text((d: { data: { name: any; }; }) => d.data.name);
          
    console.log(this.node)
  }

  //#endregion


  //#region Draw the Constraints

  protected initLinks(){
    // Add links between the nodes.
    this.link = this.svg.selectAll(".link")
                        .data(this.root.links())
                        .enter()
                        .append("path")

                        // Not a graph Node. It's a link
                        .attr("class", "edge")

                        
                        // Straight lines.
                        .attr("d", (d: { source: { x: string; y: string; }; target: { x: string; y: string; }; }) => "M" + parseInt(d.source.x) * this.nodeSpacing + "," + d.source.y + 
                                        "L" + parseInt(d.target.x) * this.nodeSpacing + "," + d.target.y)
  }

    

  protected initCrossLinks(){
    if (!this.fmGraph) return;

    this.crossLink = this.svg.selectAll(".cross-hierarchy")
                                  .data(this.fmGraph.getEdgesCrossTree())
                                  .enter()
                                  .append("g")
                                  .attr("class", (rel: GraphEdge) => this.getCrossRelationshipClass(rel));

    // Render links of type1
    this.crossLink.filter((d: { relType: number; }) => d.relType === 4)
                    .append("line")
                    .attr("x1", (d: { fromId: string | number; }) => this.pointNodeById[d.fromId].x * this.nodeSpacing)
                    .attr("y1", (d: { fromId: string | number; }) => this.pointNodeById[d.fromId].y)
                    .attr("x2", (d: { toId: string | number; }) => this.pointNodeById[d.toId].x * this.nodeSpacing) 
                    .attr("y2", (d: { toId: string | number; }) => this.pointNodeById[d.toId].y)
                            
    // Render links of type1
    this.crossLink.filter((d: { relType: number; }) => d.relType === 5)
                    .append("line")
                    .attr("x1", (d: { fromId: string | number; }) => this.pointNodeById[d.fromId].x * this.nodeSpacing)
                    .attr("y1", (d: { fromId: string | number; }) => this.pointNodeById[d.fromId].y)
                    .attr("x2", (d: { toId: string | number; }) => this.pointNodeById[d.toId].x * this.nodeSpacing)
                    .attr("y2", (d: { toId: string | number; }) => this.pointNodeById[d.toId].y)

    // Cross-tree constraints visualization
    this.svg.selectAll(".constraint")
            .data(this.fmGraph.getEdgesCrossTree())
            .enter()
            .append("line")
            .attr("class", "constraint")
            .style("stroke", "red")  // Cross-tree constraints in red for emphasis
            .style("stroke-dasharray", ("3, 3"))  // Dashed line for cross-tree constraints
            .attr("marker-end", "url(#end)");  // Arrow marker
  }

  //#endregion
}