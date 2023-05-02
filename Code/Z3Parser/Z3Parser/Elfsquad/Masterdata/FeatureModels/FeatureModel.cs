using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
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
