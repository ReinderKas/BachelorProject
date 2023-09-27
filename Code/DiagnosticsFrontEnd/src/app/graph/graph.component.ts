import { Component, ViewEncapsulation, Input, AfterViewInit, ElementRef, Renderer2 } from '@angular/core';
import * as d3 from 'd3';
import { FmGraphResult } from 'src/models/fmGraphResult';
import { GraphNode } from 'src/models/node';
import { GraphDrawer } from './graphDrawer';

@Component({
  selector: 'app-graph',
  templateUrl: './graph.component.html',
  styleUrls: ['./graph.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class GraphComponent implements AfterViewInit {
  @Input() fmGraph: FmGraphResult | null = null;
  @Input() componentSize: number = 500;
  @Input() zoom: number = 100;

  private graphDrawer: GraphDrawer | null = null;
  private svg: any;

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
    console.log(this.componentSize)
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
    
    this.svg = d3.select('.graph')
                  .append('svg')
                  .attr("viewBox", [0, 0, this.width()*(this.zoom/100), this.height()*(this.zoom/100)])
                  .append('g')
                  .attr('transform', `translate(${this.margin.left},${this.margin.top})`);
        
    // Create necessary data layouts.
    let tree = this.generateTree();
    let stratify = this.generateStratify();
    let root = tree(stratify(this.fmGraph.getGraphNodes()))
                    
    // Initialize Graph Drawer here since we want to assert that the FmGraph is initialized.
    this.graphDrawer = new GraphDrawer(this.fmGraph, root, this.svg);
              
    // Render the data.
    this.graphDrawer.renderGraph();
  }



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