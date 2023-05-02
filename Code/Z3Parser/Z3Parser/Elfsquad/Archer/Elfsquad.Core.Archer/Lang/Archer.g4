grammar Archer;

prog            
                :       (stat ';'?)+ EOF;
         
stat            
                :       'model' ID idList? '{' (stat ';'?)+ '}'             # modelStatement
                |       ID '-' KEY cardinExpr? '->' ID obj?                 # relationshipStatement
                |       typeDeclr? ID obj?                                  # variableDeclarationStatement
                |       'constraint' expr                                   # constraintStatement
                |       'condition' ID '='? expr                            # conditionStatement
                |       'lower_bound' ID '='? expr                          # lowerBoundStatement
                |       'upper_bound' ID '='? expr                          # upperBoundStatement
                |       'func' KEY '(' (KEY  (',' KEY )* )? ')' '=>' expr   # funcDeclarationStatement
                |       '#include' STRING                                   # includeStatement
                |       objectiveDeclr expr                                 # objectiveStatement
                |       COMMENT                                             # commentStatement
                ;     

typeDeclr       :       ('int'|'float');

objectiveDeclr  :       ('minimize'|'maximize')
                ;

paramList       :       expr  (',' expr )* 
                ;
                
idList          :       ':' ID (',' ID)*
                ;

expr            :       expr mulDivExprOp expr                              # mulDivExpr
                |       expr addSubExprOp expr                              # addSubExpr
                |       KEY                                                 # funcArgExpr
                |       expr equalExprOp expr                               # equalBinaryExpr
                |       expr logicalExprOp expr                             # logicalBinaryExpr
                |       ( propExpr | value  )                               # valueExpr
                |       funcCall                                            # funcCallExpr
                |       listCompr                                           # listComprExpr
                ;

cardinExpr      :       '(' NUMBER ',' NUMBER? ')'
                ;
              
funcCall        :       KEY '(' paramList? ')' ;                           

mulDivExprOp    :       ( MUL | DIV );
addSubExprOp    :       ( ADD | MIN );
equalExprOp     :       ( GREATERTHAN | LESSTHAN | LESSTHAN_E | GREATERTHAN_E | EQUALS );
logicalExprOp   :       ( AND | OR ) ;

propExpr        :       ID ('.' ID )* ;

obj
                :       '{' (pair)* '}'
                ;

pair
                :       MODIFIER? KEY '=' value ';'
                ;

value
                :       primitiveValue
                |       obj
                |       array
                ;

primitiveValue  :       STRING
                |       NUMBER
                |       BOOLEAN
                ;

array
                :       '[' value (',' value)* ']'
                |       '[' ']'
                ;
                
listCompr       :       '{' expr 'for' ID 'in' expr '}'
                ;


ID      
                :       '[' [a-zA-Z0-9 ]+ ']';

STRING
                :       '"' .*? '"'
                ;

NUMBER
                :       '-'? INT ('.' [0-9] +)?
                ;

BOOLEAN
                :       'true'
                |       'false'
                ;

fragment INT
                :       '0' | [1-9] [0-9]*
                ;

COMMENT         
                :       '//' ~[\r\n]*;

MODIFIER        
                :       'public' | 'private';

KEY
                :       [a-zA-Z0-9]+;

MUL             :       '*';
DIV             :       '/';
MIN             :       '-';
ADD             :       '+';
GREATERTHAN     :       '>';
LESSTHAN        :       '<';
GREATERTHAN_E   :       '>=';
LESSTHAN_E      :       '<=';
EQUALS          :       ('=' | '==');
AND             :       '&&';
OR             :        '||';



WS
                :       [ \t\r\n] + -> skip
                ;