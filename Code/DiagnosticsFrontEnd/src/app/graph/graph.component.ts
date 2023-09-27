import { Component, ViewEncapsulation, Input, AfterViewInit, ElementRef, Renderer2 } from '@angular/core';
import * as d3 from 'd3';
import { FmGraphResult } from 'src/models/fmGraphResult';
import { GraphNode } from 'src/models/node';

@Component({
  selector: 'app-graph',
  templateUrl: './graph.component.html',
  styleUrls: ['./graph.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class GraphComponent implements AfterViewInit {
  @Input() fmGraph: FmGraphResult | null = null;
  @Input() componentSize: number = 500;

  private node: any;
  private link: any;
  private crossLink: any;
  private svg: any;
  private root: any;
  private zoom: any;
  private legend: any;

  private pointNodeById: any = {};

  // Dimensions / styling.
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
  }

  ngOnChanges() {
    this.clearGraph();
    this.adjustSize();
    this.createGraph();
  }
  
  
  // Rescale the element of the component to the given height and width.
  private adjustSize() {
    this.renderer.setStyle(this.el.nativeElement, 'display', 'block');
    this.renderer.setStyle(this.el.nativeElement, 'width', this.componentSize + 'px');
    this.renderer.setStyle(this.el.nativeElement, 'height', this.componentSize + 'px');
  }

  // Remove the existing SVG from the graph div in this component.
  private clearGraph(){
    // Select and remove all D3 objects within the SVG container excepth the graph div container.
    let graph = d3.select(this.el.nativeElement);
    graph.selectAll('*:not(.graph)').remove();
        
    // Remove the Tooltip component
    d3.select(".tooltip")
      .remove()
  }

  private createGraph(){
    if (!this.fmGraph) return;

    this.initZoom();
    this.initRoot();
    this.initSVG(); 
    this.initNodes();
    this.initLinks();   
    this.initCrossLinks();
    this.initLegend();
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
    
    this.root.descendants().forEach((d: { id: string | number; }) => {
      this.pointNodeById[d.id] = d
    });
  }
  
  // Initialize the SVG object on which we can draw the Graph
  private initSVG(){
    this.svg =  d3.select(".graph")
                .append("svg")
                .attr("viewBox", [0, 0, this.width(), this.height()]); 

    // THESIS: Set the zoom here so we can zoom with the cursor in the entire viewbox.
    this.svg.call(this.zoom);

    this.svg = this.svg.append('g')
                        .attr('transform', `translate(${this.margin.left},${this.margin.top})`)
  }
  

  //#region Draw Nodes

  // Visualize the Nodes.
  private initNodes(){
    // Add each node as a group.
    this.node = this.svg.selectAll(".node")
        .data(this.root.descendants())
        .enter()
        .append("g")

    // Assign correct class
        .attr("class", (d: { children: any; }) => "node" + (d.children ? " node--internal" : " node--leaf"))
        
    // Assign correct position
        .attr("transform", (d: { x: string; y: string; }) => "translate(" + d.x + "," + d.y + ")")
        
    // Adds the circle to the node 
    this.node.append("circle")
        .attr("r", 15)
        
    // Add text to the node.
    this.node.append("text")
        .attr("dy", ".35em")
        .style("text-anchor", "middle")
        .text((d: { data: { name: any; }; }) => d.data.name);
  }

  //#endregion



  //#region Draw Constraints

  private initLinks(){
    // Add links between the nodes.
    this.link = this.svg.selectAll(".link")
                        .data(this.root.links())
                        .enter()
                        .append("path")
                        .attr("class", "link")
                        
                        // Straight lines.
                        .attr("d", (d: { source: { x: string; y: string; }; target: { x: string; y: string; }; }) => "M" + d.source.x + "," + d.source.y + 
                                        "L" + d.target.x + "," + d.target.y)
  }

    

  private initCrossLinks(){
    if (!this.fmGraph) return;

    this.crossLink = this.svg.selectAll(".cross-hierarchy")
                                  .data(this.fmGraph.getEdgesCrossTree())
                                  .enter()
                                  .append("g")
                                  .attr("class", "cross-hierarchy");

    // Render links of type1
    this.crossLink.filter((d: { relType: number; }) => d.relType === 4)
                    .append("line")
                    .attr("class", "excludes")
                    .attr("x1", (d: { fromId: string | number; }) => this.pointNodeById[d.fromId].x)
                    .attr("y1", (d: { fromId: string | number; }) => this.pointNodeById[d.fromId].y)
                    .attr("x2", (d: { toId: string | number; }) => this.pointNodeById[d.toId].x) 
                    .attr("y2", (d: { toId: string | number; }) => this.pointNodeById[d.toId].y)
                    .style("stroke", "red");
                            
    // Render links of type1
    this.crossLink.filter((d: { relType: number; }) => d.relType === 5)
                    .append("line")
                    .attr("class", "requires")
                    .attr("x1", (d: { fromId: string | number; }) => this.pointNodeById[d.fromId].x)
                    .attr("y1", (d: { fromId: string | number; }) => this.pointNodeById[d.fromId].y)
                    .attr("x2", (d: { toId: string | number; }) => this.pointNodeById[d.toId].x)
                    .attr("y2", (d: { toId: string | number; }) => this.pointNodeById[d.toId].y)
                    .style("stroke", "blue");

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
  
  
  //#region Legend
  
  // Add the legend so that it's immediately clear what everything means.
  private initLegend(){
    this.legend = this.svg.append("g")
                            .attr("transform", "translate(" + -this.componentSize/10  + ",0)");  // Adjust this for legend's position

          // For hierarchical constraints
    this.legend.append("line")
                .attr("x1", 0)
                .attr("y1", 0)
                .attr("x2", 20)
                .attr("y2", 0)
                .style("stroke", "black");

    this.legend.append("text")
                .attr("x", 30)
                .attr("y", 5)
                .text("Hierarchical")
                .attr("alignment-baseline", "middle");

      // For cross-tree constraints
    this.legend.append("line")
                .attr("x1", 0)
                .attr("y1", 20)
                .attr("x2", 20)
                .attr("y2", 20)
                .style("stroke", "red")
                .style("stroke-dasharray", ("3, 3"));

    this.legend.append("text")
                .attr("x", 30)
                .attr("y", 25)
                .text("Cross-tree")
                .attr("alignment-baseline", "middle");
  }
  
  private initZoom(){
      this.zoom = d3.zoom()
          .on('zoom', (e) => this.handleZoom(e));
  }
  

  private handleZoom(e: { transform: string | number | boolean | readonly (string | number)[] | d3.ValueFn<d3.BaseType, unknown, string | number | boolean | readonly (string | number)[] | null> | null; }) {
      this.svg.attr('transform', e.transform);
  }

  //#endregion

  //#region Data Layouts

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
  
}