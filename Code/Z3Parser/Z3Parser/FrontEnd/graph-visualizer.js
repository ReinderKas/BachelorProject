// Sample data for the graph
var data = {
    nodes: [
        { id: 'A' },
        { id: 'B' },
        { id: 'C' },
        { id: 'D' },
        { id: 'E' },
    ],
    links: [
        { source: 'A', target: 'B' },
        { source: 'B', target: 'C' },
        { source: 'C', target: 'D' },
        { source: 'D', target: 'E' },
        { source: 'E', target: 'A' },
    ],
};
// Create the SVG container for the graph
var svg = d3
    .select('body')
    .append('svg')
    .attr('width', 400)
    .attr('height', 400);
// Create a force simulation
var simulation = d3
    .forceSimulation()
    .force('link', d3.forceLink().id(function (d) { return d.id; }))
    .force('charge', d3.forceManyBody())
    .force('center', d3.forceCenter(200, 200));
// Add links to the simulation
var link = svg
    .selectAll('line')
    .data(data.links)
    .enter()
    .append('line')
    .style('stroke', 'black')
    .style('stroke-width', 2);
// Add nodes to the simulation
var node = svg
    .selectAll('circle')
    .data(data.nodes)
    .enter()
    .append('circle')
    .attr('r', 10)
    .style('fill', 'steelblue')
    .call(d3.drag().on('start', dragstarted).on('drag', dragged).on('end', dragended));
// Update the positions of nodes and links during simulation tick
simulation.nodes(data.nodes).on('tick', ticked);
simulation.force('link').links(data.links);
function ticked() {
    link
        .attr('x1', function (d) { return d.source.x; })
        .attr('y1', function (d) { return d.source.y; })
        .attr('x2', function (d) { return d.target.x; })
        .attr('y2', function (d) { return d.target.y; });
    node.attr('cx', function (d) { return d.x; }).attr('cy', function (d) { return d.y; });
}
// Drag event handlers
function dragstarted(event, d) {
    if (!event.active)
        simulation.alphaTarget(0.3).restart();
    d.fx = d.x;
    d.fy = d.y;
}
function dragged(event, d) {
    d.fx = event.x;
    d.fy = event.y;
}
function dragended(event, d) {
    if (!event.active)
        simulation.alphaTarget(0);
    d.fx = null;
    d.fy = null;
}
