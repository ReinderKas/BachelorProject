grammar MyLang;

expr : INT
     | ID
     | expr op=('*' | '/') expr
     | expr op=('+' | '-') expr
     ;

ID   : [a-zA-Z]+ ;
INT  : [0-9]+ ;
WS   : [ \t\r\n]+ -> skip ;