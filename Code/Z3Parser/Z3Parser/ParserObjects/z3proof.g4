grammar z3proof;

parse: expr EOF;

expr: let | asserted | rewrite | funcCall;

let: '(' 'let' '(' binding (',' binding)* ')' expr ')';
binding: '(' '$' ID expr ')';
asserted: '(' 'asserted' expr ')';
rewrite: '(' 'rewrite' expr expr ')';
funcCall: '(' ID (expr)* ')';

ID: [a-zA-Z]+;
INT: [0-9]+;
WS: [ \t\r\n]+ -> skip;