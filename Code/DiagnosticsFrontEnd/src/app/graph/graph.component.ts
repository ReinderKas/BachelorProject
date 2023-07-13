import { Component, OnInit, ViewChild, ElementRef, ViewEncapsulation, Input } from '@angular/core';
import * as d3 from 'd3';
import { FmGraphResult } from 'src/models/fmGraphResult';

@Component({
  selector: 'app-graph',
  templateUrl: './graph.component.html',
  styleUrls: ['./graph.component.css'],
  encapsulation: ViewEncapsulation.None
})

export class GraphComponent implements OnInit {
  @Input() graphData: FmGraphResult[] | null = null; // add this line

  constructor() { }

  ngOnInit() {
    this.createGraph();
    console.log("Initializing Graph: " + this.graphData);
  }

  createGraph() {
    const data = [1, 2, 3, 4, 5]; // Your data
  
    const svg = d3.select(".graph")
      .append('svg')
      .attr('width', '500')
      .attr('height', '500');
  
    const contentWidth = +svg.attr('width');
    const contentHeight = +svg.attr('height');
  
    const xScale = d3.scaleLinear()
      .domain([0, d3.max(data) || 0])  // provide 0 as a fallback value
      .range([0, contentWidth]);
  
    const yScale = d3.scaleBand()
      .domain(data.map((d) => d.toString()))
      .rangeRound([contentHeight, 0])
      .padding(0.1);
  
    svg.append('g')
      .selectAll('rect')
      .data(data)
      .enter()
      .append('rect')
      .attr('x', 0)
      .attr('y', (d) => yScale(d.toString()) || 0)
      .attr('width', (d) => xScale(d))
      .attr('height', yScale.bandwidth());
  }
}