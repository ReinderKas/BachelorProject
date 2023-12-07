namespace Elfskot.Core.Models.Entities.FeatureModels
{
    public class FeatureModel
    {
        /// <summary>
        /// NOTE: Temporary storage field - will be removed when the git implementation has completed
        /// </summary>
        public string ArcherModel { get; set; }

        public FeatureModelNode RootNode { get; set; }

        public List<FeatureModelNode> AllNodes { get; set; }
        public List<FeatureModelRelationship> AllRelationships { get; set; }


        public static List<FeatureModelRelationshipTypes> CrossTreeRelationshipTypes
            => new List<FeatureModelRelationshipTypes> { FeatureModelRelationshipTypes.Excludes, FeatureModelRelationshipTypes.Required, FeatureModelRelationshipTypes.Implies };
    }
}
