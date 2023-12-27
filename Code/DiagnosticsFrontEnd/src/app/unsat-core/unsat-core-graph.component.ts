import { Component, ElementRef, Input, Renderer2, ViewEncapsulation } from "@angular/core";
import { AbstractGraphComponent } from "../graph/abstractGraph.component";
import { FmGraphResult } from "src/models/fmGraphResult";
import { HierarchyPointNode } from "d3-hierarchy";
import { GraphNode } from "src/models/node";
import { ConstraintType, GraphEdge } from "src/models/edge";
import { UnsatCoreResult } from "src/models/unsatCoreResult";
import { OptionsComponent } from "../options/options.component";
import { MatDialog } from "@angular/material/dialog";
import { DiagnoseOptions } from "src/models/diagnoseDialogType";

@Component({
    selector: 'unsat-graph',
    templateUrl: '../graph/graph.component.html',
    styleUrls: ['../graph/graph.component.css'],
    encapsulation: ViewEncapsulation.None
})

export class UnsatCoreGraph extends AbstractGraphComponent {
    @Input() model: string = "";
    @Input() unsatCore: UnsatCoreResult | null = null;
    @Input() fmGraph: FmGraphResult | null = null; 
    @Input() componentHeight: number = 500;
    @Input() componentWidth: number = 500;
    @Input() nodeSize: number = 15;
    @Input() nodeSpacing: number = 1;

    constructor(
      protected override el: ElementRef, 
      protected override renderer: Renderer2,
      public dialog: MatDialog
    ) {
      super(el, renderer)
    }
  
    //#region Draw the Nodes
    // Visualize the Nodes.
    protected initNodes(){
        // Add each node as a group.
        this.node = this.svg.selectAll(".node")
            .data(this.root.descendants()) 
            .enter()
            .append("g")
  
            
        // Assign correct position
            .attr("transform", (d: { x: string; y: string; }) => "translate(" + parseInt(d.x) * this.nodeSpacing + "," + d.y + ")")
            
        // Adds the circle to the node 
        this.node.append("circle")
            .attr("r", this.nodeSize)
            .attr("class", (d: HierarchyPointNode<GraphNode>) => this.getNodeClass(d))
            
        // Add text to the node.
        this.node.append("text")
            .attr("dy", ".35em")
            .style("text-anchor", "middle")
            .text((d: { data: { name: any; }; }) => d.data.name);
        this.node.on('click', (_event: any, d: HierarchyPointNode<GraphNode>) => this.openDialog(d));
    }

    protected override getNodeClass(d: HierarchyPointNode<GraphNode>): string{
      if (this.unsatCore?.remove(d.data.name)){
        return "node node-red";
      }

      return (this.unsatCore?.nodes.find(n => n.name == d.data.name) != undefined)
                    ? "node node-orange"
                    : "node node-green";
    }
  
    //#endregion
  
  
    //#region Draw the Constraints
  
    protected initLinks(){
      // Add links between the nodes.
      this.link = this.svg.selectAll(".link")
                          .data(this.root.links())
                          .enter()
                          .append("path")
  
                          // Not a graph Node. It's a link
                          .attr("class", "edge")
  
                          
                          // Straight lines.
                          .attr("d", (d: { source: { x: string; y: string; }; target: { x: string; y: string; }; }) => "M" + parseInt(d.source.x) * this.nodeSpacing + "," + d.source.y + 
                                          "L" + parseInt(d.target.x) * this.nodeSpacing + "," + d.target.y)
    }
  
      
  
    protected initCrossLinks(){
      if (!this.fmGraph) return;
  
      this.crossLink = this.svg.selectAll(".cross-hierarchy")
                                    .data(this.fmGraph.getEdgesCrossTree())
                                    .enter()
                                    .append("g")
                                    .attr("class", (rel: GraphEdge) => this.getCrossRelationshipClass(rel));
  
      // Render links of type1
      this.crossLink.filter((d: { relType: number; }) => d.relType === 4)
                      .append("line")
                      .attr("x1", (d: { fromId: string | number; }) => this.pointNodeById[d.fromId].x * this.nodeSpacing)
                      .attr("y1", (d: { fromId: string | number; }) => this.pointNodeById[d.fromId].y)
                      .attr("x2", (d: { toId: string | number; }) => this.pointNodeById[d.toId].x * this.nodeSpacing) 
                      .attr("y2", (d: { toId: string | number; }) => this.pointNodeById[d.toId].y)
                              
      // Render links of type1
      this.crossLink.filter((d: { relType: number; }) => d.relType === 5)
                      .append("line")
                      .attr("x1", (d: { fromId: string | number; }) => this.pointNodeById[d.fromId].x * this.nodeSpacing)
                      .attr("y1", (d: { fromId: string | number; }) => this.pointNodeById[d.fromId].y)
                      .attr("x2", (d: { toId: string | number; }) => this.pointNodeById[d.toId].x * this.nodeSpacing)
                      .attr("y2", (d: { toId: string | number; }) => this.pointNodeById[d.toId].y)
  
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

    private openDialog(d: HierarchyPointNode<GraphNode>){
      if (this.fmGraph?.root?.name === d.data.name
        || !this.unsatCore?.nodes.some(n => n.name === d.data.name)){
        return;
      }

      var data = {
        "model" : this.model,
        "nodeId" : d.data.id,
        "nodeName" : d.data.name
      }

      let dialogRef = this.dialog.open(OptionsComponent, {
        width: '350px',
        height: '350px',
        data: data
      });


      dialogRef.afterClosed().subscribe((result: any) => {
        console.log('The dialog was closed. Selected Item: ', result);
        this.changeConstraint(result, d);
      });
    }



    private changeConstraint(option: DiagnoseOptions, node: HierarchyPointNode<GraphNode>){
      switch(option){
        case DiagnoseOptions.Optional:
          this.makeOptional(node);
          return;

        case DiagnoseOptions.Delete:
          this.deleteConstraint(node);
          return;

        default:
          return;
      }
    }

    private makeOptional(node: HierarchyPointNode<GraphNode>){
      let relationship = this.fmGraph?.edges.find(e => e.isNonXTree 
                                                      && e.to.some(toNode => toNode.id === node.data.id));

      if (relationship){
        relationship.relType = ConstraintType.Optional    
      }  
      this.unsatCore = null;
      this.ngOnChanges();                                      
    }



    private deleteConstraint(node: HierarchyPointNode<GraphNode>){
      let relationship = this.fmGraph?.edges.find(e => !e.isNonXTree 
        && (e.from.id === node.data.id 
            || e.to.some(toNode => toNode.id === node.data.id)))

      this.fmGraph?.edges.forEach((rel,index)=>{
        if(rel === relationship) this.fmGraph?.edges.splice(index,1);
      });
      this.unsatCore = null;
      this.ngOnChanges();     
    }
  }
