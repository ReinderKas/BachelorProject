import {  Input, AfterViewInit, ElementRef, Renderer2, Injectable } from '@angular/core';
import { FmGraphResult } from 'src/models/fmGraphResult';
import * as d3 from 'd3';
import { GraphNode } from 'src/models/node';

@Injectable()
export abstract class AbstractGraphComponent implements AfterViewInit {
    @Input() abstract fmGraph: FmGraphResult | null;
    @Input() abstract componentSize: number;
    
  
    protected root: any;
    protected svg: any;
  
    protected link: any;
    protected crossLink: any;
    protected node: any;  
    protected drag: any;
    protected zoom: any;
    protected legend: any;
    protected simulation : any;
    protected pointNodeById: any = {};
  
    // // Dimensions / styling.
    protected margin =  {top: 20, right: 90, bottom: 30, left: 90};
    protected width() { return this.componentSize - this.margin.left - this.margin.right; }
    protected height() { return this.componentSize - this.margin.top - this.margin.bottom; }
  
    constructor(
        protected el: ElementRef, 
        protected renderer: Renderer2
    ) {}
  
    ngAfterViewInit() {
      if (!this.fmGraph){
        alert("No Feature Model data given to create a Graph for.")
        return;
      }
    }
    
    // Called when the 'componentSize' property changes (through binding from the parent/app component).
    ngOnChanges() {
      this.clearGraph();
      this.adjustSize();
      this.createGraph();
    }
  
    //#region Logic for what to do when changing the Graph Component information.
    
    // Rescale the element of the component to the given height and width.
    protected adjustSize() {
      this.renderer.setStyle(this.el.nativeElement, 'display', 'block');
      this.renderer.setStyle(this.el.nativeElement, 'width', this.componentSize + 'px');
      this.renderer.setStyle(this.el.nativeElement, 'height', this.componentSize + 'px');
    }
  
    // Remove the existing SVG from the graph div in this component.
    protected clearGraph(){
        // Select and remove all D3 objects within the SVG container excepth the graph div container.
        const toClear = d3.select(this.el.nativeElement);
        toClear.selectAll('*:not(.graph)').remove();
      
        // Remove the Tooltip component
        d3.select(".tooltip")
            .remove()
    }


    //#endregion
    
    // #region Data Layouts

    // Create a tree layout.
    protected generateTree(){
        return d3.tree<GraphNode>().size([
        this.width() - this.margin.left - this.margin.right,
        this.height() - this.margin.top - this.margin.bottom
        ])
    }


    // Create a stratify layout.
    protected generateStratify(){
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


    


    // #region Rendering the Graph itself.
  
    // Create and draw the new Graph.
    protected createGraph(){          
      this.initRoot();
      this.initZoom();
      this.initSVG(); 
      this.initLinks();   
      this.initCrossLinks();
      this.initNodes();
      this.initLegend();
    }

    // Seperate implementations for the static and interactive graphs.
    protected abstract initNodes(): any;        // Draw the Nodes.
    protected abstract initLinks(): any;        // Draw the Hierarchical Links.
    protected abstract initCrossLinks(): any;   // Draw the Cross-Hierarchical Links.
    


    // Initialize the d3 root hierarchical object.
    protected initRoot(){
        if (!this.fmGraph) {
            alert("Feature Model object is undefined!")
            return;
        }

        // Create necessary data layouts.
        let tree = this.generateTree();
        let stratify = this.generateStratify();
        this.root = tree(stratify(this.fmGraph.getGraphNodes()))
        
        this.root.descendants().forEach((d: { id: string | number; }) => { this.pointNodeById[d.id] = d });
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


    protected initZoom(){
        this.zoom = d3.zoom()
              .on('zoom', (e) => this.handleZoom(e));
      }
    
    protected handleZoom(e: { transform: string | number | boolean | readonly (string | number)[] | d3.ValueFn<d3.BaseType, unknown, string | number | boolean | readonly (string | number)[] | null> | null; }) {
        this.svg.attr('transform', e.transform);
    }

    
  // Add the legend so that it's immediately clear what everything means.
  protected initLegend(){
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

    //#endregion
  
}