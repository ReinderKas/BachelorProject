import * as d3 from 'd3';
import { FmGraphResult } from 'src/models/fmGraphResult';
import { GraphNode } from 'src/models/node';



export class GraphDrawer{
    private fmGraph: FmGraphResult;
    private root: d3.HierarchyPointNode<GraphNode>
    private svg:  d3.Selection<SVGGElement, unknown, HTMLElement, any>
    
    constructor(
        fmGraph: FmGraphResult,
        root: d3.HierarchyPointNode<GraphNode>,
        svg:  d3.Selection<SVGGElement, unknown, HTMLElement, any>
    ) {
        this.fmGraph = fmGraph;
        this.root = root;
        this.svg = svg
    }

    
    // Render the Graph with the given data. 
    public renderGraph(){
        this.addLinks()
        this.addCrossLinks()
        this.addTooltip()
        this.addLegend()
        this.addNodes()
    }

    //#region Draw Nodes

    // Visualize the Nodes.
    private addNodes(){

        console.log(this.root.descendants())

        
        // Add each node as a group.
        var node = this.svg.selectAll(".node")
            .data(this.root.descendants())
            .enter()
            .append("g")
    
        // Assign correct class
            .attr("class", d => "node" + (d.children ? " node--internal" : " node--leaf"))
            
        // Assign correct position
            .attr("transform", d => "translate(" + d.x + "," + d.y + ")")
            
        // Adds the circle to the node 
        node.append("circle")
            .attr("r", 15)
            
        // Add text to the node.
        node.append("text")
            .attr("dy", ".35em")
            .style("text-anchor", "middle")
            .text(d => d.data.name);
      }

    //#endregion



    //#region Draw Constraints

    private addLinks(){
        // Add links between the nodes.
        this.svg.selectAll(".link")
                .data(this.root.links())
                .enter()
                .append("path")
                .attr("class", "link")
                
                // Straight lines.
                .attr("d", d => "M" + d.source.x + "," 
                                    + d.source.y + 
                                "L" + d.target.x + "," 
                                    + d.target.y)
      }

      

    private addCrossLinks(){
        // Define arrow markers for cross-tree constraints
        this.svg.append("defs").selectAll("marker")
                .data(["end"]) 
                .enter().append("marker")
                .attr("id", String)
                .attr("viewBox", "0 -5 10 10")
                .attr("refX", 15)
                .attr("refY", 0)
                .attr("markerWidth", 6)
                .attr("markerHeight", 6)
                .attr("orient", "auto")
                .append("path")
                .attr("d", "M0,-5L10,0L0,5")
                .attr("fill", "red");  // Color of the arrow

        // Cross-tree constraints visualization
        this.svg.selectAll(".constraint")
                .data(this.fmGraph.getEdgesCrossTree())
                .enter()
                .append("line")
                .attr("class", "constraint")
                // ... (x1, y1, x2, y2 calculations)
                .style("stroke", "red")  // Cross-tree constraints in red for emphasis
                .style("stroke-dasharray", ("3, 3"))  // Dashed line for cross-tree constraints
                .attr("marker-end", "url(#end)");  // Arrow marker
    }




    private drawMandatory(){
    }
    
    private drawOptional(){
    }
    
    private drawAlternative(){
    }
    
    private drawOr(){
    }
    
    private drawExclusion(){
    }
    
    private drawRequires(){
    }

    //#endregion

    //#region Legend and Tooltip
    // Add the tooltip so that if we hover over the link we get the necessary information.
    private addTooltip(){
        const tooltip = d3.select("body")
                            .append("div")
                            .attr("class", "tooltip");

        this.svg.selectAll(".constraint-hitbox")
                .data(this.root.links())
                .enter()
                .append("line")
                .attr("class", "constraint-hitbox")
                .style("stroke", "transparent")
                .style("stroke-width", "10px")
                .on("mouseover", function(event: { pageX: number; pageY: number; }, d: { source: any; target: any; }) {
                    tooltip.transition()
                        .duration(100)
                        .style("opacity", .9);
                    tooltip.html(`[${d.source.data.name}] -> [${d.target.data.name}]`)
                        .style("left", (event.pageX + 5) + "px")
                        .style("top", (event.pageY - 28) + "px");
                })
                .on("mouseout", function(d: any) {
                    tooltip.transition()
                        .duration(1000)
                        .style("opacity", 0.2);
                });
    }
    
    // Add the legend so that it's immediately clear what everything means.
    private addLegend(){
        const legend = this.svg.append("g")
                                .attr("transform", "translate(-50,0)");  // Adjust this for legend's position

            // For hierarchical constraints
            legend.append("line")
                .attr("x1", 0)
                .attr("y1", 0)
                .attr("x2", 20)
                .attr("y2", 0)
                .style("stroke", "black");

            legend.append("text")
                .attr("x", 30)
                .attr("y", 5)
                .text("Hierarchical")
                .attr("alignment-baseline", "middle");

            // For cross-tree constraints
            legend.append("line")
                .attr("x1", 0)
                .attr("y1", 20)
                .attr("x2", 20)
                .attr("y2", 20)
                .style("stroke", "red")
                .style("stroke-dasharray", ("3, 3"));

            legend.append("text")
                .attr("x", 30)
                .attr("y", 25)
                .text("Cross-tree")
                .attr("alignment-baseline", "middle");
    }
    //#endregion
}