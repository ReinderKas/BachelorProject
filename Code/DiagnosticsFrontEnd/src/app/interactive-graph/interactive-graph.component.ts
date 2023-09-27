import { AfterViewInit, Component, ElementRef, Input, Renderer2 } from '@angular/core';
import { FmGraphResult } from 'src/models/fmGraphResult';
import * as d3 from 'd3';
import { GraphNode } from 'src/models/node';

@Component({
  selector: 'app-interactive-graph',
  templateUrl: '../graph/graph.component.html',
  styleUrls: ['../graph/graph.component.css']
})
export class InteractiveGraphComponent implements AfterViewInit {
  @Input() fmGraph: FmGraphResult | null = null;
  @Input() componentSize: number = 500;
  

  private root: any;
  private svg: any;

  private simulation : any;
  private link: any;
  private node: any;  
  private drag: any;

  // // Dimensions / styling.
  private margin =  {top: 20, right: 90, bottom: 30, left: 90};
  private width() { return this.componentSize - this.margin.left - this.margin.right; }
  private height() { return this.componentSize - this.margin.top - this.margin.bottom; }

  constructor(
    private el: ElementRef, 
    private renderer: Renderer2
  ) {}

  ngAfterViewInit() {
    if (!this.fmGraph){
      alert("No Feature Model data given to create a Graph for.")
      return;
    }

    this.clearGraph();
    this.adjustSize();
    this.createGraph();
  }
  
  // Called when the 'componentSize' property changes (through binding from the parent/app component).
  ngOnChanges() {
    this.clearGraph();
    this.adjustSize();
    this.createGraph();
  }

  
  
  // Rescale the element of the component to the given height and width.
  private adjustSize() {
    this.renderer.setStyle(this.el.nativeElement, 'width', this.componentSize + 'px');
    this.renderer.setStyle(this.el.nativeElement, 'height', this.componentSize + 'px');
  }

  // Remove the existing SVG from the graph div in this component.
  private clearGraph(){
    // Select and remove all D3 objects within the SVG container
    const svg2 = d3.select(this.el.nativeElement);
        svg2.selectAll('*:not(.graph)').remove();
  }



  // #region Rendering the Graph itself.

  // Create and draw the new Graph.
  private createGraph(){          
    this.initRoot();
    this.initSVG();
    this.initLinks();    
    this.initNodes();
    this.initSimulation();
    this.initDrag()
  }

  // Initialize the root hierarchical object.
  private initRoot(){
    if (!this.fmGraph) {
      alert("Feature Model object is undefined!")
      return;
    }
    // Create necessary data layouts.
    let tree = this.generateTree();
    let stratify = this.generateStratify();
    this.root = tree(stratify(this.fmGraph.getGraphNodes()))
  }

  // Initialize the SVG object on which we can draw the Graph
  private initSVG(){
    this.svg =  d3.select(".graph")
                .append("svg")
                .attr("viewBox", [0, 0, this.width(), this.height()]);    
  }

  // Draw the Links between Nodes.
  private initLinks(){
    this.link = this.svg.selectAll(".link")
                    .data(this.root.links())
                    .join("line")
                    .classed("link", true)
  }

  // Draw the Nodes on the Graph SVG.
  private initNodes(){
    this.node = this.svg.selectAll(".node")
                    .data(this.root.descendants())
                    .join("circle")
                    .attr("r", 10)
                    .classed("node", true)
                    .classed("fixed", (d: { x: undefined; }) => d.x !== undefined)
  }

  // Initialize the force simulator for the graph.
  private initSimulation(){
    this.simulation = d3.forceSimulation()
                        .nodes(this.root.descendants())
                        .force("charge", d3.forceManyBody())
                        .force("center", d3.forceCenter(this.height()/2, this.width()/2))
                        .force("link", d3.forceLink(this.root.links()))
                        .on("tick", () => {this.tick()});
  }

  // Initialize the drag behaviour of the Graph Nodes.
  private initDrag(){
    this.drag = d3.drag()
                  .on("start", () => this.dragstart())
                  .on("drag", (event: any, d: any) => this.dragged(event, d));

    this.node.call(this.drag).on("click", (event: any, d: { fx: any; fy: any; }) => this.click(event, d));
  }

  //#endregion


  // #region Data Layouts

  // Create a tree layout.
  private generateTree(){
    return d3.tree<GraphNode>().size([
      this.width() - this.margin.left - this.margin.right,
      this.height() - this.margin.top - this.margin.bottom
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


  // #region Drag behavior  

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
      delete d.fx;
      delete d.fy;
      d3.select(this).classed("fixed", false);
      this.simulation.alpha(1).restart();
  }


  private dragstart(this: any) {
      d3.select(this).classed("fixed", true);
  }

  private dragged(this: any, event: { x: any; y: any; }, d: { fx: any; fy: any; }) {
      d.fx = this.clamp(event.x, 0, this.width);
      d.fy = this.clamp(event.y, 0, this.height);
      this.simulation.alpha(1).restart();
  }

  private clamp(x: number, lo: number, hi: number) {
      return x < lo ? lo : x > hi ? hi : x;
  }

  //#endregion
}
