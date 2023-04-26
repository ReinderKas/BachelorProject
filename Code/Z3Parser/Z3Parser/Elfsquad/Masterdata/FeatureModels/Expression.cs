namespace Elfskot.Core.Models.Entities.FeatureModel
{
    [Serializable]
    public class Expression
    {
        public Guid Id { get; set; }

        public string ExpressionString { get; set; }

        public List<ExpressionVariable> Variables { get; set; } = new List<ExpressionVariable>();

        public Delegate CompiledExpression;

        public ExpressionType ExpressionType { get; set; }

        public bool PureFunction { get; set; }
    }

    public enum ExpressionType
    {
        LinqExpression,
        PythonExpression
    }


    [Serializable]
    public class ExpressionVariable
    {
        public string Tag { get; set; }
        public Guid NodeId { get; set; }
        /// <summary>
        /// When the expression node is not part of the expression's feature model, this field contains the
        /// identifier of the feature model this node belongs to.
        /// </summary>
        public Guid? FeatureModelId { get; set; }
        public bool Children { get; set; }
        public ChildCalculationMethod ChildCalculationMethod { get; set; }
        public bool UseUnitPrice { get; set; }
        public Guid? FeaturePropertyId { get; set; }

        //[SwaggerExclude]
        //public FeatureProperty FeatureProperty { get; set; }

        /// <summary>
        /// If the exists property is set to true;
        ///     Take the value of the property * 1 if the NodeId is present in the configuration; otherwise 0.
        /// Else;
        ///     Multiply the value of the property by the value of the NodeId in the configuration.
        /// </summary>
        public bool Exists { get; set; }
        public Guid ExpressionId { get; set; }


        public Expression Expression { get; set; }
    }

    [Serializable]
    public enum ChildCalculationMethod
    {
        Sum,
        Average,
        Min,
        Max
    }
}
