using Elfskot.Core.Masterdata.FeatureModels.Translator;
using Microsoft.Z3;
using System.Globalization;
using Z3Parser.FeatureModels;

namespace Z3Parser.Elfsquad.Archer.Elfsquad.Core.Archer
{
    public class ArcherVariableStore
    {
        protected readonly Z3Solver _z3Solver;
        protected readonly Dictionary<NodeProperty, Expr> _expressionByVariable;
        protected readonly Dictionary<string, Expr> _expressionByName;

        public ArcherVariableStore(Z3Solver z3Solver)
        {
            _z3Solver = z3Solver;
            _expressionByVariable = new();
            _expressionByName = new();
        }



        public Expr GetExpression(NodeProperty nodeProperty)
        {
            if (!_expressionByVariable.TryGetValue(nodeProperty, out Expr expr))
                return AddExpression(nodeProperty);

            return expr;
        }

        public Expr AddExpression(NodeProperty variable)
        {
            var name = $"{variable.Node.NodeId}.{variable.Property}";
            Expr expr = variable.Type switch
            {
                NodePropertyType.Boolean => _z3Solver.Z3Context.MkBoolConst(name),
                NodePropertyType.Float => _z3Solver.Z3Context.MkRealConst(name),
                NodePropertyType.Int => _z3Solver.Z3Context.MkIntConst(name),
                _ => throw new NotImplementedException("Type is not supported.")
            };

            if (variable.Node.Properties.TryGetValue(variable.Property, out var value))
            {
                if (variable.Type == NodePropertyType.Boolean)
                {
                    bool boolValue = Convert.ToBoolean(value, CultureInfo.InvariantCulture);
                    _z3Solver.Z3Context.MkEq(
                        expr as BoolExpr,
                        boolValue ? _z3Solver.Z3Context.MkTrue() : _z3Solver.Z3Context.MkFalse());
                }
                else
                {
                    var ratNum = DecimalToRatNum(_z3Solver.Z3Context, Convert.ToDecimal(value, CultureInfo.InvariantCulture));

                    //_z3Solver.Solver..AssertSoft(_z3Solver.Z3Context.MkEq(
                    //    expr as ArithExpr,
                    //    ratNum), 1, "default");


                    // TODO: Implement in non-optimization solver.
                }
            }

            _expressionByVariable.Add(variable, expr);
            _expressionByName.Add(name, expr);

            return expr;
        }


        public static RatNum DecimalToRatNum(Context context, decimal value)
        {
            var accuracy = 0.00000000001m;

            if (Math.Abs(value) < accuracy) return context.MkReal(0);

            int sign = Math.Sign(value);

            value = Math.Abs(value);

            // Accuracy is the maximum relative error; convert to absolute maxError
            decimal maxError = sign == 0 ? accuracy : value * accuracy;

            int n = (int)Math.Floor(value);
            value -= n;

            if (value < maxError)
            {
                return context.MkReal(sign * n, 1);
            }

            if (1 - maxError < value)
            {
                return context.MkReal(sign * (n + 1), 1);
            }

            decimal z = value;
            int previousDenominator = 0;
            int denominator = 1;
            int numerator;

            do
            {
                z = 1.0m / (z - (int)z);
                int temp = denominator;
                denominator = denominator * (int)z + previousDenominator;
                previousDenominator = temp;
                numerator = Convert.ToInt32(value * denominator);
            }
            while (Math.Abs(value - (decimal)numerator / denominator) > maxError && z != (int)z);

            return context.MkReal((n * denominator + numerator) * sign, denominator);
        }

    }
}
