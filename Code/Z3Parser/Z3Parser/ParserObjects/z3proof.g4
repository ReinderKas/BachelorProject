grammar z3proof;

parse: expr EOF;

expr: letExpr | mpExpr | assertedExpr | rewriteExpr | funcCall | VAR;
letExpr: '(' 'let' '(' bindings ')' expr ')';
bindings: (binding (',' binding)*)?;
binding: '(' VAR expr ')';
mpExpr: '(' 'mp' expr expr expr ')';
assertedExpr: '(' 'asserted' expr ')';
rewriteExpr: '(' 'rewrite' expr expr ')';
funcCall: '(' ID (expr)* ')';

ID: [a-zA-Z]+;
VAR: '$' [a-zA-Z0-9]+;
WS: [ \t\r\n]+ -> skip;
