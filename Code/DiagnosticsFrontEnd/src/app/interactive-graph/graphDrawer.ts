import * as d3 from 'd3';
import { FmGraphResult } from 'src/models/fmGraphResult';
import { GraphNode } from 'src/models/node';



export class GraphDrawer{
    private simulation : any;
    private link: any;
    private node: any;  
    
    // TODO: Clean this up. 
    // Dimensions / styling.
    private margin =  {top: 20, right: 90, bottom: 30, left: 90};
    private width = 700 - this.margin.left - this.margin.right;
    private height = 700 - this.margin.top - this.margin.bottom;


    constructor(
        fmGraph: FmGraphResult,
        root: d3.HierarchyPointNode<GraphNode>,
        svg:  d3.Selection<SVGSVGElement, undefined, HTMLElement, undefined>
    ) {
        this.link = svg.selectAll(".link")
                        .data(root.links())
                        .join("line")
                        .classed("link", true);

        this.node = svg.selectAll(".node")
                        .data(root.descendants())
                        .join("circle")
                        .attr("r", 12)
                        .classed("node", true)
                        // .classed("fixed", d => d.fx !== undefined)

        this.simulation = d3.forceSimulation()
                            .nodes(root.descendants())
                            .force("charge", d3.forceManyBody())
                            .force("center", d3.forceCenter(500, 500))
                            .force("link", d3.forceLink(root.links()))
                            .on("tick", () => {this.tick()});
    }

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

    private dragged(event: { x: any; y: any; }, d: { fx: any; fy: any; }) {
        d.fx = this.clamp(event.x, 0, this.width);
        d.fy = this.clamp(event.y, 0, this.height);
        this.simulation.alpha(1).restart();
    }

    private clamp(x: number, lo: number, hi: number) {
        return x < lo ? lo : x > hi ? hi : x;
    }

}