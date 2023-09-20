import { Component, ViewEncapsulation, Input, AfterViewInit } from '@angular/core';
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

  private graphDrawer: GraphDrawer | null = null;
  private svg: d3.Selection<SVGSVGElement, unknown, HTMLElement, any> | null = null;

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

    this.svg = d3.select('.graph')
                  .append('svg')
                  .attr('width', this.width)
                  .attr('height', this.height);

    this.createGraph();
  }

  createGraph(){
    if (!this.fmGraph || !this.svg) return;

    // Create necessary data layouts.
    let tree = this.generateTree();
    let stratify = this.generateStratify();
    let root = tree(stratify(this.fmGraph.getGraphNodes()))

    // Move the starting point from the edge.
    let graphSvg = this.svg.append('g')
                    .attr('transform', `translate(${this.margin.left},${this.margin.top})`);
                    
    // Initialize Graph Drawer here since we want to assert that the FmGraph is initialized.
    this.graphDrawer = new GraphDrawer(this.fmGraph, root, graphSvg);
              
    // Render the data.
    this.graphDrawer.renderGraph();
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
}