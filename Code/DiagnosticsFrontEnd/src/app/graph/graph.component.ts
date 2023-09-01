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

    let graphNodes = this.fmGraph.getVisualizationNodes();
    let graphEdges = this.fmGraph.GetVisualizationEdges();

    console.log(graphNodes);
    console.log(graphEdges);
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
}