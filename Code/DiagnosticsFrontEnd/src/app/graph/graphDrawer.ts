import { Component, OnInit, ViewEncapsulation, Input } from '@angular/core';
import * as d3 from 'd3';
import { GraphEdge } from 'src/models/edge';
import { FmGraphResult } from 'src/models/fmGraphResult';
import { GraphNode } from 'src/models/node';



export class GraphDrawer{
    private svg:  d3.Selection<SVGSVGElement, unknown, HTMLElement, any>;

    constructor(svg:  d3.Selection<SVGSVGElement, unknown, HTMLElement, any>) {
        this.svg = svg;
    }

    //#region Draw Nodes


    //#endregion



    //#region Draw Constraints

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
}

