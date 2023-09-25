import { AfterViewInit, Component, Input } from '@angular/core';
import { FmGraphResult } from 'src/models/fmGraphResult';
import { GraphDrawer } from './graphDrawer';
import * as d3 from 'd3';
import { GraphNode } from 'src/models/node';

@Component({
  selector: 'app-interactive-graph',
  templateUrl: './interactive-graph.component.html',
  styleUrls: ['./interactive-graph.component.css']
})
export class InteractiveGraphComponent implements AfterViewInit {
  @Input() fmGraph: FmGraphResult | null = null;

  private simulation : any;
  private link: any;
  private node: any;  
  private drag: any;

  // Dimensions / styling.
  private margin =  {top: 20, right: 90, bottom: 30, left: 90};
  private width = 700 - this.margin.left - this.margin.right;
  private height = 700 - this.margin.top - this.margin.bottom;

  constructor() {}

  ngAfterViewInit() {
    if (!this.fmGraph){
      alert("No Feature Model data given to create a Graph for.")
      return;
    }

    this.createGraph();
  }

  createGraph(){
    if (!this.fmGraph) return;

    // Create necessary data layouts.
    let tree = this.generateTree();
    let stratify = this.generateStratify();
    let root = tree(stratify(this.fmGraph.getGraphNodes()))

    let svg = d3.select(".graph")
                .append("svg")
                .attr("viewBox", [0, 0, this.width, this.height]);                    
    
    this.link = svg.selectAll(".link")
                    .data(root.links())
                    .join("line")
                    .classed("link", true);

    this.node = svg.selectAll(".node")
        .data(root.descendants())
        .join("circle")
        .attr("r", 12)
        .classed("node", true)
        .classed("fixed", d => d.x !== undefined)

    this.simulation = d3.forceSimulation()
            .nodes(root.descendants())
            .force("charge", d3.forceManyBody())
            // .force("center", d3.forceCenter(this.height/2, this.width/2))
            .force("link", d3.forceLink(root.links()))
            .on("tick", () => {this.tick()});


    this.drag = d3.drag()
                  .on("start", () => this.dragstart())
                  .on("drag", (event: any, d: any) => this.dragged(event, d));

    this.node.call(this.drag).on("click", (event: any, d: { fx: any; fy: any; }) => this.click(event, d));
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
    if (!this.fmGraph){
      alert("Feature Model Graph undefined when generating Stratify hierarchy.");
      return d3.stratify<GraphNode>();
    }

    let edgesHierarchy = this.fmGraph.getEdgesHierarchy();
    return d3.stratify<GraphNode>()
                .id(d => d.id)
                .parentId(d => {
                  let edge = edgesHierarchy.find(e => e.toId.includes(d.id));
                  return edge ? edge.fromId : null;
                })
  }
  //#endregion


  

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

  private click(this: any, event: any, d: { fx: any; fy: any; }){
    console.log("Click.")
    console.log(this)

      delete d.fx;
      delete d.fy;
      d3.select(this).classed("fixed", false);
      this.simulation.alpha(1).restart();
  }


  private dragstart(this: any) {
    // console.log("DragStart")
    // console.log(this)
      d3.select(this).classed("fixed", true);
  }

  private dragged(this: any, event: { x: any; y: any; }, d: { fx: any; fy: any; }) {
    console.log("Dragged")
    console.log(this)
      d.fx = this.clamp(event.x, 0, this.width);
      d.fy = this.clamp(event.y, 0, this.height);
      this.simulation.alpha(1).restart();
  }

  private clamp(x: number, lo: number, hi: number) {
      return x < lo ? lo : x > hi ? hi : x;
  }
}
