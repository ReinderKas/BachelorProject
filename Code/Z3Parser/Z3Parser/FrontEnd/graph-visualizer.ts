// Sample data for the graph
const data = {
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
const svg = d3
  .select('body')
  .append('svg')
  .attr('width', 400)
  .attr('height', 400);

// Create a force simulation
const simulation = d3
  .forceSimulation()
  .force('link', d3.forceLink().id((d: any) => d.id))
  .force('charge', d3.forceManyBody())
  .force('center', d3.forceCenter(200, 200));

// Add links to the simulation
const link = svg
  .selectAll('line')
  .data(data.links)
  .enter()
  .append('line')
  .style('stroke', 'black')
  .style('stroke-width', 2);

// Add nodes to the simulation
const node = svg
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
    .attr('x1', (d: any) => d.source.x)
    .attr('y1', (d: any) => d.source.y)
    .attr('x2', (d: any) => d.target.x)
    .attr('y2', (d: any) => d.target.y);

  node.attr('cx', (d: any) => d.x).attr('cy', (d: any) => d.y);
}

// Drag event handlers
function dragstarted(event: any, d: any) {
  if (!event.active) simulation.alphaTarget(0.3).restart();
  d.fx = d.x;
  d.fy = d.y;
}

function dragged(event: any, d: any) {
  d.fx = event.x;
  d.fy = event.y;
}

function dragended(event: any, d: any) {
  if (!event.active) simulation.alphaTarget(0);
  d.fx = null;
  d.fy = null;
}
