using Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints.Relationships;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints;

namespace DiagnosticsApi.Models
{ 
    // TODO: Currently only works for normal Relationship Constraints. Not Expression Constraints
    // TODO: Currently only works for normal Relationship Constraints. Not Expression Constraints
    [Serializable]
    public class ConstraintResult
    {
        public ConstraintType Type;
        public Guid FromNode;
        public Guid[] ToNodes;


        public ConstraintResult(IRelationshipConstraint constraint)
        {
            switch (constraint)
            {
                case MandatoryFeatureModelConstraint:
                    Type = ConstraintType.Mandatory; break;
                case OptionalFeatureModelConstraint:
                    Type = ConstraintType.Optional; break;
                case AlternativeFeatureModelConstraint:
                    Type = ConstraintType.Alternative; break;
                case OrFeatureModelConstraint:
                    Type = ConstraintType.Or; break;

                case ExcludesFeatureModelConstraint:
                    Type = ConstraintType.Excludes; break;
                case RequiresFeatureModelConstraint:
                    Type = ConstraintType.Requires; break;
                default:
                    break;
            }

            FromNode = constraint.FromNodeId;
            ToNodes = constraint.ToNodeIds().ToArray();
        }
    }

    public enum ConstraintType
    {
        Mandatory,
        Optional,
        Alternative,
        Or,

        Excludes,
        Requires,
    }
}
