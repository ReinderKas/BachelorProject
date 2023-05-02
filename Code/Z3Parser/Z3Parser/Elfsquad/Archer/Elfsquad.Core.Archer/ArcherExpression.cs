namespace Elfsquad.Core.Archer
{
    public abstract class ArcherExpression
    { }

    public class BinaryArcherExpression : ArcherExpression
    {
        public ArcherExpression Left;
        public ArcherExpressionOperator Operator;
        public ArcherExpression Right;

        public BinaryArcherExpression(ArcherExpression left, ArcherExpressionOperator op, ArcherExpression right)
        {
            Left = left;
            Operator = op;
            Right = right;
        }
    }

    public class ValueArcherExpression<T> : ArcherExpression
    {
        public T Value;

        public ValueArcherExpression(T value)
        {
            Value = value;
        }
    }

    public class PropertyArcherExpression : ArcherExpression
    {
        public ArcherVariable Variable;
        public string[] Keys;
        
        public PropertyArcherExpression(ArcherVariable variable, string[] keys)
        {
            Variable = variable;
            Keys = keys;
        }
    }

    public class FuncCallArcherExpression : ArcherExpression
    {
        public string FunctionName;
        public ArcherExpression[] Arguments;

        public FuncCallArcherExpression(string functionName, ArcherExpression[] arguments)
        {
            FunctionName = functionName;
            Arguments = arguments;
        }
    }

    public class FuncArgArcherExpression : ArcherExpression
    {
        public string Name;

        public FuncArgArcherExpression(string name)
        {
            Name = name;
        }
    }

    public class ListComprehensionArcherExpression : ArcherExpression
    {
        public ArcherExpression Output;
        public string OutputVarId;
        public ArcherExpression Collection;
        public ArcherExpression Condition;

        public ListComprehensionArcherExpression(ArcherExpression output, string outputVarId, ArcherExpression collection, ArcherExpression condition)
        {
            Output = output;
            OutputVarId = outputVarId;
            Collection = collection;
            Condition = condition;
        }
    }
    
    public enum ArcherExpressionOperator
    {
        Div, Mul, Add, Min, GreaterThan, LessThan, GreaterThanOrEqual, LessThanOrEqual, Equals, And, Or
    }
}