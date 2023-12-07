using System.Globalization;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace Elfsquad.Core.Archer
{
    public static class ArcherParserHelper
    {
        public static string SourceTextForContext(ITerminalNode terminalNode)
        {
            return terminalNode.GetText();
        }
    }
    
    public class ArcherInheritanceParser : ArcherBaseListener
    {
        private List<(string ParentId, string ChildId)> _inheritanceRules;
        private string _currentModelId;
        
        public List<(string ParentId, string ChildId)> Parse(ArcherParser.ProgContext context)
        {
            _inheritanceRules = new List<(string ParentId, string ChildId)>();
            Antlr4.Runtime.Tree.ParseTreeWalker.Default.Walk(this, context);
            return _inheritanceRules;
        }
        
        public override void EnterModelStatement(ArcherParser.ModelStatementContext context)
        {
            _currentModelId = ArcherParserHelper.SourceTextForContext(context.ID()).Trim('[', ']');
        }
        
        public override void EnterIdList(ArcherParser.IdListContext context)
        {
            foreach (var id in context.ID())
            {
                _inheritanceRules.Add((_currentModelId, ArcherParserHelper.SourceTextForContext(id).Trim('[', ']')));
            }
        }
    }
    
    public class ArcherTranslator : ArcherBaseListener
    {   
        private Dictionary<string, ArcherModelBuilder> _archerModels;
        private string _currentModelId;
        private List<(string ParentId, string ChildId)> _inheritanceRules;
        
        public ArcherModel[] Parse(string filePath, string fileContent)
        {
            var models = Parse(fileContent);
            foreach (var model in models)
                model.FilePath = filePath;
            return models;
        }

        public async Task<ArcherModel[]> ParseAsync(FileInfo file)
        {
            return Parse(
                file.FullName,
                await File.ReadAllTextAsync(file.FullName));
        }

        public ArcherModel[] Parse(FileInfo file)
        {
            return Parse(
                file.FullName,
                File.ReadAllText(file.FullName));
        }

        public ArcherModel[] Parse(string value)
        {
            _archerModels = new Dictionary<string, ArcherModelBuilder>();
            
            var inputStream = new AntlrInputStream(value);
            var speakLexer = new ArcherLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(speakLexer);
            var parser = new ArcherParser(commonTokenStream);
            var context = parser.prog();

            _inheritanceRules = new ArcherInheritanceParser().Parse(context);
            Antlr4.Runtime.Tree.ParseTreeWalker.Default.Walk(this, context);
            
            return _archerModels.Values.Select(b => b.Build()).ToArray();
        }
        
        public override void EnterModelStatement(ArcherParser.ModelStatementContext context)
        {
            _currentModelId = ArcherParserHelper.SourceTextForContext(context.ID()).Trim('[', ']');
            base.EnterModelStatement(context);
        }

        public override void EnterVariableDeclarationStatement(ArcherParser.VariableDeclarationStatementContext context)
        {
            var id = ArcherParserHelper.SourceTextForContext(context.ID());
            foreach (var builder in GetBuilders())
            {
                var variable = GetVariableByKey(builder, id);

                var typeDeclr = context.typeDeclr();
                if (typeDeclr != null)
                {
                    if (typeDeclr.GetText() == "int")
                        variable.SetType(ArcherVariableType.Int);
                }

                var obj = context.obj();
                if (obj != null)
                {
                    variable.Object = VisitObj(obj);
                }    
            }
        }
        
        public override void EnterRelationshipStatement(ArcherParser.RelationshipStatementContext context)
        {
            var variableIds = context.ID();
            var parentKey = ArcherParserHelper.SourceTextForContext(variableIds[0]);
            var childKey = ArcherParserHelper.SourceTextForContext(variableIds[1]);
            var relType = context.KEY().GetText();

            var cardinalityExpr = context.cardinExpr();
            int? lowerBound = null;
            int? upperBound = null;
            if (cardinalityExpr != null)
            {
                var ints = cardinalityExpr.NUMBER()
                    .Select(n => Convert.ToInt32(n.GetText()))
                    .ToArray();
                lowerBound = ints[0];
                if (ints.Length > 1)
                    upperBound = ints[1];
            }

            foreach (var builder in GetBuilders())
            {
                var parentNode = GetVariableByKey(builder, parentKey);
                var childNode = GetVariableByKey(builder, childKey);

                if (lowerBound != null)
                {
                    childNode.CardinalityMin = lowerBound;
                    childNode.CardinalityMax = upperBound;
                }                

                builder.Relationships.Add(new ArcherRelationship(parentNode, childNode, relType));    
            }
        }
        
        public override void EnterConstraintStatement(ArcherParser.ConstraintStatementContext context)
        {
            var t = context.GetText();
            foreach (var builder in GetBuilders())
            {
                var expression = VisitExpression(builder, context.expr());
                builder.Constraint.Add(new ArcherConstraint(expression));    
            }
        }

        public override void EnterConditionStatement(ArcherParser.ConditionStatementContext context)
        {
            var id = ArcherParserHelper.SourceTextForContext(context.ID());

            foreach (var builder in GetBuilders())
            {
                var variable = GetVariableByKey(builder, id);
                var expression = VisitExpression(builder, context.expr());

                var conditionExpression = new ArcherCondition(variable, expression);
                variable.Conditions.Add(conditionExpression);    
            }
        }

        public override void EnterLowerBoundStatement(ArcherParser.LowerBoundStatementContext context)
        {
            var id = ArcherParserHelper.SourceTextForContext(context.ID());

            foreach (var builder in GetBuilders())
            {
                var variable = GetVariableByKey(builder, id);
                var expression = VisitExpression(builder, context.expr());

                var lowerBoundExpression = new ArcherLowerBoundExpression(variable, expression);
                variable.LowerBoundExpressions.Add(lowerBoundExpression);    
            }
        }

        public override void EnterUpperBoundStatement(ArcherParser.UpperBoundStatementContext context)
        {
            var id = ArcherParserHelper.SourceTextForContext(context.ID());

            foreach (var builder in GetBuilders())
            {
                var variable = GetVariableByKey(builder, id);
                var expression = VisitExpression(builder, context.expr());

                var upperBoundExpression = new ArcherUpperBoundExpression(variable, expression);
                variable.UpperBoundExpressions.Add(upperBoundExpression);    
            }
        }

        public override void EnterFuncDeclarationStatement(ArcherParser.FuncDeclarationStatementContext context)
        {
            var name = context.KEY()[0].GetText();
            var arguments = context.KEY().Skip(1).Select(n => n.GetText()).ToArray();

            foreach (var builder in GetBuilders())
            {
                var body = VisitExpression(builder, context.expr());
                builder.FuncDeclarations.Add(new ArcherFuncDeclaration(name, arguments, body));    
            }
        }

        public override void EnterIncludeStatement(ArcherParser.IncludeStatementContext context)
        {
            var fileName = context.STRING().GetText().Trim('"').Trim();

            if (!fileName.EndsWith(".py")) return;

            foreach (var builder in GetBuilders())
            {
                builder.IncludePythonStatements.Add(new IncludePythonStatement(fileName));   
            }
        }

        public override void EnterObjectiveStatement([NotNull] ArcherParser.ObjectiveStatementContext context)
        {
            var objective = context.objectiveDeclr().GetText() == "maximize" ? ArcherObjective.Maximize : ArcherObjective.Minimize;
            foreach (var builder in GetBuilders())
            { 
                var expression = VisitExpression(builder, context.expr());
                builder.ObjectiveExpressions.Add(new ArcherObjectiveExpression(objective, expression));
            }
        }

        private ArcherObject VisitObj(ArcherParser.ObjContext context)
        {
            var pairs = new List<ArcherObjectPair>();
            foreach (var pair in context.pair())
            {
                var modifier = pair.MODIFIER() != null ? pair.MODIFIER().GetText() : null;
                var key = pair.KEY().GetText();
                var value = VisitValue(pair.value());
                pairs.Add(new ArcherObjectPair(modifier, key, value));
            }

            return new ArcherObject(pairs.ToArray());
        }

        private object VisitValue(ArcherParser.ValueContext context)
        {
            if (context.obj() != null)
                return VisitObj(context.obj());

            if (context.array() != null)
                return VisitArray(context.array());

            if (context.primitiveValue() != null)
                return VisitPrimitiveValue(context.primitiveValue());
                
            throw new NotImplementedException("Value type not implemented");
        }

        private object VisitPrimitiveValue(ArcherParser.PrimitiveValueContext context)
        {
            if (context.STRING() != null)
                return context.STRING().GetText().Trim('"');

            if (context.NUMBER() != null)
                return Convert.ToDouble(context.NUMBER().GetText(), CultureInfo.InvariantCulture);

            if (context.BOOLEAN() != null)
                return context.BOOLEAN().GetText() == "true";
            
            throw new NotImplementedException("Primitive value type not implemented");
        }

        private object[] VisitArray(ArcherParser.ArrayContext context)
        {
            var values = new List<object>();
            foreach (var value in context.value())
            {
                values.Add(VisitValue(value));
            }
            return values.ToArray();
        }

        private ArcherExpression VisitExpression(ArcherModelBuilder builder, ArcherParser.ExprContext context)
        {
            return context switch
            {
                ArcherParser.ValueExprContext valueExprContext => VisitValueExpression(builder, valueExprContext),
                ArcherParser.MulDivExprContext mulDivExprContext => VisitMulDivExpression(builder, mulDivExprContext),
                ArcherParser.AddSubExprContext addSubExprContext => VisitAddSubExpression(builder, addSubExprContext),
                ArcherParser.EqualBinaryExprContext equalExprContext => VisitEqualBinaryExpression(builder, equalExprContext),
                ArcherParser.LogicalBinaryExprContext logicalExprContext => VisitLogicalBinaryExpression(builder, logicalExprContext),
                ArcherParser.FuncArgExprContext funcArgExprContext => VisitFuncArgExpression(funcArgExprContext),
                ArcherParser.FuncCallExprContext funcCallExprContext => VisitFuncCallExpression(builder, funcCallExprContext),
                ArcherParser.ListComprExprContext listComprExpContext => VisitListComprehensionExpression(builder, listComprExpContext),
                _ => throw new NotImplementedException("Expression type is not implemented.")
            };
        }

        private ArcherExpression VisitValueExpression(ArcherModelBuilder builder, ArcherParser.ValueExprContext context)
        {
            if (context.propExpr() != null)
                return VisitPropExpression(builder, context.propExpr());

            if (context.value() == null)  throw new NotImplementedException("Expression type is not implemented.");
            
            var value = VisitValue(context.value());
            return value switch
            {
                bool v => new ValueArcherExpression<bool>(v),
                double v => new ValueArcherExpression<double>(v),
                string v => new ValueArcherExpression<string>(v),
                ArcherObject v => new ValueArcherExpression<ArcherObject>(v),
                object[] v => new ValueArcherExpression<object[]>(v),
                _ => new ValueArcherExpression<object>(value)
            };
        }

        private BinaryArcherExpression VisitMulDivExpression(ArcherModelBuilder builder, ArcherParser.MulDivExprContext context)
        {
            var expressions = context.expr();
            var left = VisitExpression(builder, expressions[0]);
            var right = VisitExpression(builder, expressions[1]);
            var op = VisitArithExpressionOperator(context.mulDivExprOp().GetText());
            return new BinaryArcherExpression(left, op, right);
        }
        private BinaryArcherExpression VisitAddSubExpression(ArcherModelBuilder builder, ArcherParser.AddSubExprContext context)
        {
            var expressions = context.expr();
            var left = VisitExpression(builder, expressions[0]);
            var right = VisitExpression(builder, expressions[1]);
            var op = VisitArithExpressionOperator(context.addSubExprOp().GetText());
            return new BinaryArcherExpression(left, op, right);
        }

        private BinaryArcherExpression VisitEqualBinaryExpression(ArcherModelBuilder builder, ArcherParser.EqualBinaryExprContext context)
        {
            var expressions = context.expr();
            var left = VisitExpression(builder, expressions[0]);
            var right = VisitExpression(builder, expressions[1]);
            var op = VisitArithExpressionOperator(context.equalExprOp().GetText());
            return new BinaryArcherExpression(left, op, right);
        }

        private BinaryArcherExpression VisitLogicalBinaryExpression(ArcherModelBuilder builder, ArcherParser.LogicalBinaryExprContext context)
        {
            var expressions = context.expr();
            var left = VisitExpression(builder, expressions[0]);
            var right = VisitExpression(builder, expressions[1]);

            var op = context.logicalExprOp().GetText() switch
            {
                "||" => ArcherExpressionOperator.Or,
                "&&" => ArcherExpressionOperator.And,
                _ => throw new ArgumentOutOfRangeException($"Operator not supported '{context.logicalExprOp().GetText()}'.")
            };

            return new BinaryArcherExpression(left, op, right);
        }

        private ArcherExpressionOperator VisitArithExpressionOperator(string text)
        {
            switch (text)
            {
                case "*": return ArcherExpressionOperator.Mul;
                case "/": return ArcherExpressionOperator.Div;
                case "+": return ArcherExpressionOperator.Add;
                case "-": return ArcherExpressionOperator.Min;
                case "<": return ArcherExpressionOperator.LessThan;
                case ">": return ArcherExpressionOperator.GreaterThan;
                case "<=": return ArcherExpressionOperator.LessThanOrEqual;
                case ">=": return ArcherExpressionOperator.GreaterThanOrEqual;
                case "=": return ArcherExpressionOperator.Equals;
                case "==": return ArcherExpressionOperator.Equals;
            }

            throw new NotImplementedException($"Expression operator '{text}' has not been implemented.");
        }

        private FuncArgArcherExpression VisitFuncArgExpression(ArcherParser.FuncArgExprContext context)
        {
            var name = context.KEY().GetText();
            return new FuncArgArcherExpression(name);
        }
        
        private FuncCallArcherExpression VisitFuncCallExpression(ArcherModelBuilder builder, ArcherParser.FuncCallExprContext context)
        {
            return VisitFuncCall(builder, context.funcCall());
        }

        private bool InListComprehension = false;
        private ListComprehensionArcherExpression VisitListComprehensionExpression(ArcherModelBuilder builder, ArcherParser.ListComprExprContext context)
        {
            InListComprehension = true;
            var listCompr = context.listCompr();
            var expressions = listCompr.expr();
            var output = VisitExpression(builder, expressions[0]);
            var collection = VisitExpression(builder, expressions[1]);
            var outputVarId = ArcherParserHelper.SourceTextForContext(listCompr.ID()).Trim('[', ']');
            InListComprehension = false;
            return new ListComprehensionArcherExpression(output, outputVarId, collection, null);
        }

        private FuncCallArcherExpression VisitFuncCall(ArcherModelBuilder builder, ArcherParser.FuncCallContext context)
        {
            var functionName = context.KEY().GetText();
            var parameters = Array.Empty<ArcherExpression>();
            if (context.paramList() != null)
                parameters = VisitParamList(builder, context.paramList());
            return new FuncCallArcherExpression(functionName, parameters);
        }
        
        private ArcherExpression[] VisitParamList(ArcherModelBuilder builder, ArcherParser.ParamListContext context)
        {
            return context.expr().Select(e => VisitExpression(builder, e)).ToArray();
        }
        
        private PropertyArcherExpression VisitPropExpression(ArcherModelBuilder builder, ArcherParser.PropExprContext context)
        {
            var variable = GetVariableByKey(builder, ArcherParserHelper.SourceTextForContext(context.ID()[0]));
            var propertyKeys = context.ID().Skip(1).Select(i => ArcherParserHelper.SourceTextForContext(i).Trim('[', ']')).ToArray();
            return new PropertyArcherExpression(variable, propertyKeys.ToArray());
        }

        private ArcherVariable GetVariableByKey(ArcherModelBuilder builder, string key)
        {
            if (key.Trim('[', ']') == _currentModelId && builder != _archerModels[_currentModelId])
                return builder.GetVariableByKey(builder.RootKey, save: !InListComprehension);

            return builder.GetVariableByKey(key, save: !InListComprehension);
        }
        

        private IEnumerable<ArcherModelBuilder> GetBuilders()
        {
            yield return GetBuilder(_currentModelId);

            foreach (var inherit in _inheritanceRules.Where(r => r.ChildId == _currentModelId))
                yield return GetBuilder(inherit.ParentId);
        }

        private ArcherModelBuilder GetBuilder(string id)
        {
            if (!_archerModels.ContainsKey(id))
                _archerModels[id] = new ArcherModelBuilder(id);

            return _archerModels[id];
        }
    }
}