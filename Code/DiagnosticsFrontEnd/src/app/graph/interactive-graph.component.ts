import { Component, ElementRef, Input, Renderer2, ViewEncapsulation } from '@angular/core';
import * as d3 from 'd3';
import { AbstractGraphComponent } from './abstractGraph.component';
import { FmGraphResult } from 'src/models/fmGraphResult';
import { GraphNode } from 'src/models/node';

@Component({
  selector: 'app-interactive-graph',
  templateUrl: './graph.component.html',
  styleUrls: ['./graph.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class InteractiveGraphComponent extends AbstractGraphComponent {
    @Input() fmGraph: FmGraphResult | null = null;
    @Input() componentSize: number = 500;
    @Input() nodeSize: number = 10;
    
  constructor(
        protected override el: ElementRef, 
        protected override renderer: Renderer2
  ) {
    super(el, renderer);
  }


  protected override createGraph(): void {
    super.createGraph()
    this.initSimulation();
    this.initDrag();
  }

  //#region Draw the Nodes  

  // Draw the Nodes on the Graph SVG.
  protected initNodes(){    
    this.node = this.svg.selectAll(".node")
                    .data(this.root.descendants())
                    .classed("node", true)
                    .join("circle")
                      .attr("class", (d: any) => this.getNodeClass(d))
                      .attr("r", this.nodeSize/2)
                      .classed("fixed", (d: { x: undefined; }) => d.x !== undefined)
                    .join("text")
                      .attr("dy", ".35em")
                      .text((d: { data: { name: any; }; }) => d.data.name);
  }

  

  //#endregion

  //#region Draw the Constraints

  // Draw the Links between Nodes.
  protected initLinks(){
    this.link = this.svg.selectAll(".edge")
                    .data(this.root.links())
                    .join("line")
                    .attr("class", "edge")
  }
  
  protected override initCrossLinks() {
    return;
  }

  //#endregion

  //#region Add the interactivity to the Graph

  // Initialize the force simulator for the graph.
  protected initSimulation(){
    this.simulation = d3.forceSimulation()
                        .nodes(this.root.descendants())
                        .force("charge", d3.forceManyBody())
                        .force("center", d3.forceCenter(this.height()/2.5, this.width()/2.5))
                        .force("link", d3.forceLink(this.root.links()))
                        .on("tick", () => {this.tick()});
  }

  // Initialize the drag behaviour of the Graph Nodes.
  protected initDrag(){
    this.drag = d3.drag()
                  .on("start", () => this.dragstart())
                  .on("drag", (event: any, d: any) => this.dragged(event, d));

    this.node.call(this.drag).on("click", (d: { fx: any; fy: any; }) => this.click(d));
  }

  //#endregion

  // #region Simulation / Drag / Zoom behavior functions  

  private tick(){
    this.link
      .attr("x1", (d: { source: { x: any; }; }) => d.source.x)
      .attr("y1", (d: { source: { y: any; }; }) => d.source.y)
      .attr("x2", (d: { target: { x: any; }; }) => d.target.x)
      .attr("y2", (d: { target: { y: any; }; }) => d.target.y);
    this.node
      .attr("cx", (d: { x: any; }) => d.x)
      .attr("cy", (d: { y: any; }) => d.y);
  }

  private click(this: any, d: { fx: any; fy: any; }){
      delete d.fx;
      delete d.fy;
      d3.select(this).classed("fixed", false);
      this.simulation.alpha(0.1).restart();
  }


  private dragstart(this: any) {
      d3.select(this).classed("fixed", true);
  }

  private dragged(this: any, event: { x: any; y: any; }, d: { fx: any; fy: any; }) {
      d.fx = this.clamp(event.x, 0, this.width);
      d.fy = this.clamp(event.y, 0, this.height);
      this.simulation.alpha(0.1).restart();
  }

  private clamp(x: number, lo: number, hi: number) {
      return x < lo ? lo : x > hi ? hi : x;
  }

  //#endregion
}
