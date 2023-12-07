
using Microsoft.Z3;

namespace Z3Parser.Z3Constraints
{
    public class AConstraint
    {
        public bool Hierarchical { get; set; }
        public BoolExpr Expression { get; set; }

        public AConstraint(BoolExpr expression)
        {
            Expression = expression;
        }
    }



    public class UserRequirement : AConstraint
    {
        public UserRequirement(BoolExpr expr)
            : base(expr) { Hierarchical = true; } // True because it works on a single Variable, not Cross Tree.
    }

    public class Alternative : AConstraint
    {
        public Alternative(BoolExpr expr)
            : base(expr) { Hierarchical = true; }
    }

    public class Mandatory : AConstraint
    {
        public Mandatory(BoolExpr expr)
            : base(expr) { Hierarchical = true; }
    }

    public class Optional : AConstraint
    {
        public Optional(BoolExpr expr)
            : base(expr) { Hierarchical = true; }
    }

    public class Or : AConstraint
    {
        public Or(BoolExpr expr)
            : base(expr) { Hierarchical = true; }
    }

    public class Requires : AConstraint
    {
        public Requires(BoolExpr expr)
            : base(expr) { Hierarchical = false; }
    }

    public class Excludes : AConstraint
    {
        public Excludes(BoolExpr expr)
            : base(expr) { Hierarchical = false; }
    }
}
