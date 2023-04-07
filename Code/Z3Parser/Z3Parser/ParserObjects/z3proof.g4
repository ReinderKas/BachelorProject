grammar z3proof;


z3proof : expr;

expr
     : letExpr
     | assertExpr
     | notExpr
     | equalsExpr
     | unitResolutionExpr
     | mpExpr
     | rewriteExpr
     | '(' expr ')'
     | ID ;

letExpr             : '(let' '(' '(' ID expr ')' ')' expr ')';

assertExpr          : '(' '@' ID '(asserted' expr ')' ')';

notExpr             : '(' 'not' expr ')';

equalsExpr          : '(' '= a b' ')';

unitResolutionExpr  : '(' 'unit-resolution' expr expr expr ')';

mpExpr              : '(' 'mp' expr expr expr ')';

rewriteExpr         : '(' 'rewrite' '(' '= expr expr' ')' ')';

ID                  : '$'?[a-zA-Z0-9]+;

WS                  : [ \t\r\n]+ -> skip;