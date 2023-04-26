using System.Collections.Generic;
using System.Linq;

namespace Elfsquad.Core.Archer;

public class ArcherModelBuilder
{
    public string RootKey { get; }
    public Dictionary<string, ArcherVariable> Variables;
    public List<ArcherRelationship> Relationships;
    public List<ArcherConstraint> Constraint;
    public List<ArcherFuncDeclaration> FuncDeclarations;
    public List<IncludePythonStatement> IncludePythonStatements;
    public List<ArcherObjectiveExpression> ObjectiveExpressions;
    
    public ArcherModelBuilder(string rootKey)
    {
        RootKey = rootKey;
        Variables = new Dictionary<string, ArcherVariable>();
        Relationships = new List<ArcherRelationship>();
        Constraint = new List<ArcherConstraint>();
        FuncDeclarations = new List<ArcherFuncDeclaration>();
        IncludePythonStatements = new List<IncludePythonStatement>();
        ObjectiveExpressions = new List<ArcherObjectiveExpression>();
    }
    
    public ArcherVariable GetVariableByKey(string key, bool save = true)
    {
        key = key.Trim('[', ']');
        if (Variables.ContainsKey(key)) return Variables[key];

        var newVariable = new ArcherVariable(key);
        if (save) Variables[key] = newVariable;
        return newVariable;
    }

    public ArcherModel Build()
    {
        var varsWithRels = Relationships
            .Where(r => r.IsStructural)
            .Select(r => r.Child)
            .ToHashSet();

        foreach (var variable in Variables.Values.Where(v => v.Name != RootKey && !varsWithRels.Contains(v)).ToArray())
        {
            Relationships.Add(new ArcherRelationship(GetVariableByKey(RootKey), variable, "mandatory"));
        }

        return new ArcherModel(
            Variables.Values.ToArray(),
            Relationships.ToArray(),
            Constraint.ToArray(),
            FuncDeclarations.ToArray(),
            IncludePythonStatements.ToArray(),
            ObjectiveExpressions.ToArray());
    }
}