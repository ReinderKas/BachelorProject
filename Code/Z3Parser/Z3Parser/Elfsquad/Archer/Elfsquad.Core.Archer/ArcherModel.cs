using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Elfsquad.Core.Archer
{
    public class ArcherModel
    {
        public ArcherVariable[] Variables;
        public ArcherRelationship[] Relationships;
        public ArcherConstraint[] Constraints;
        public ArcherFuncDeclaration[] FuncDeclarations;
        public IncludePythonStatement[] IncludePythonStatements;
        public ArcherObjectiveExpression[] Objectives;
        public string FilePath;

        public ArcherModel(
            ArcherVariable[] variables, 
            ArcherRelationship[] relationships, 
            ArcherConstraint[] constraints,
            ArcherFuncDeclaration[] funcDeclarations,
            IncludePythonStatement[] includePythonStatements,
            ArcherObjectiveExpression[] objectives)  
        {
            Variables = variables;
            Relationships = relationships;
            Constraints = constraints;
            FuncDeclarations = funcDeclarations;
            IncludePythonStatements = includePythonStatements;
            Objectives = objectives;
        }
        
        public ArcherVariable RootVariable
        {
            get
            {
                return Variables.First(v => Relationships.All(r => r.Child != v));
            }
        }

        public ArcherVariable Parent(ArcherVariable child)
            => Relationships.FirstOrDefault(r => r.Child == child)?.Parent;
        
        public ArcherVariable[] Children(ArcherVariable parent)
            => Relationships.Where(r => r.Parent == parent && r.IsStructural).Select(r => r.Child).ToArray();
    }

    public class ArcherVariable
    {
        public string Name;
        public ArcherObject Object;
        public List<ArcherLowerBoundExpression> LowerBoundExpressions;
        public List<ArcherUpperBoundExpression> UpperBoundExpressions;
        public List<ArcherCondition> Conditions;
        public ArcherVariableType Type;
        public int? CardinalityMin = null;
        public int? CardinalityMax = null;

        public ArcherVariable(string name)
        {
            Name = name;
            Object = new ArcherObject(Array.Empty<ArcherObjectPair>());
            LowerBoundExpressions = new List<ArcherLowerBoundExpression>();
            UpperBoundExpressions = new List<ArcherUpperBoundExpression>();
            Conditions = new List<ArcherCondition>();
        }

        public void SetType(ArcherVariableType type)
        {
            Type = type;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public enum ArcherVariableType
    {
        Float,
        Int        
    }

    public class ArcherRelationship
    {
        public ArcherVariable Parent;
        public ArcherVariable Child;
        public ArcherRelationshipType Type;
        
        public ArcherRelationship(ArcherVariable parent, ArcherVariable child, string type)
        {
            Parent = parent;
            Child = child;
            Type = (ArcherRelationshipType)Enum.Parse(typeof(ArcherRelationshipType), type, true);
        }
        
        private ArcherRelationshipType[] _structuralRelTypes = { ArcherRelationshipType.Optional, ArcherRelationshipType.Mandatory, ArcherRelationshipType.Alternative, ArcherRelationshipType.Or };
        public bool IsStructural => _structuralRelTypes.Contains(Type);

        public bool IsNary => Type is ArcherRelationshipType.Alternative or ArcherRelationshipType.Or;
        public bool IsBinary => !IsNary;


        public override string ToString()
        {
            return $"{Parent} -> {Type} -> {Child}";
        }
    }

    public enum ArcherRelationshipType
    {
        Optional,
        Mandatory,
        Alternative,
        Or,
        Excludes,
        Requires,
        Filters,
        Suggests,
        Removes
    }

    public class ArcherObject
    {
        public List<ArcherObjectPair> Pairs;

        public ArcherObject(ArcherObjectPair[] pairs)
        {
            Pairs = pairs.ToList();
        }

        public ArcherObject()
        {
            Pairs = new List<ArcherObjectPair>();
        }
        
        public T GetValue<T>(string key)
        {
            var pair = Pairs.FirstOrDefault(p => p.Key == key);
            if (pair == null)
                return default(T);
            return (T)Convert.ChangeType(pair.Value, typeof(T));
        }

        public object GetValue(string key)
        {
            var stack = new Queue<string>(key.Split('.'));

            var obj = this;

            while(stack.Count > 1)
            {
                key = stack.Dequeue();
                var pair = obj.Pairs.FirstOrDefault(p => p.Key == key);
                if (pair == null) return null;

                obj = pair.Value as ArcherObject;
            }

            key = stack.Dequeue();
            return obj.Pairs.FirstOrDefault(p => p.Key == key)?.Value;
        }

        public void SetValue(string key, object child)
        {
            var pair = Pairs.FirstOrDefault(p => p.Key == key);
            if (pair != null)
            {
                pair.Value = child;
                return;
            }

            Pairs.Add(new ArcherObjectPair("public", key, child));
        }
    }

    public class ArcherObjectPair
    {
        public string Modifier;
        public string Key;
        public object Value;

        public ArcherObjectPair(string modifier, string key, object value)
        {
            Modifier = modifier;
            Key = key;
            Value = value;
        }
    }

    public class ArcherConstraint
    {
        public ArcherExpression Expression;

        public ArcherConstraint(ArcherExpression expression)
        {
            Expression = expression;
        }
    }

    public class ArcherFuncDeclaration
    {
        public string Name;
        public string[] Arguments;
        public ArcherExpression Body;

        public ArcherFuncDeclaration(string name, string[] arguments, ArcherExpression body)
        {
            Name = name;
            Arguments = arguments;
            Body = body;
        }

    }

    public class ArcherCondition
    {
        public ArcherVariable Variable;
        public ArcherExpression Expression;

        public ArcherCondition(ArcherVariable variable, ArcherExpression expression)
        {
            Variable = variable;
            Expression = expression;
        }
    }

    public class ArcherLowerBoundExpression
    {
        public ArcherVariable Variable;
        public ArcherExpression Expression;

        public ArcherLowerBoundExpression(ArcherVariable variable, ArcherExpression expression)
        {
            Variable = variable;
            Expression = expression;
        }
    }
    
    public class ArcherUpperBoundExpression
    {
        public ArcherVariable Variable;
        public ArcherExpression Expression;

        public ArcherUpperBoundExpression(ArcherVariable variable, ArcherExpression expression)
        {
            Variable = variable;
            Expression = expression;
        }
    }

    public class ArcherFunctionCallExpression
    {
        
    }
    
    public class ArcherRequirement
    {
        public ArcherVariable Variable;
        public bool IsSelection;
        public double Value;
        
        public ArcherRequirement(ArcherVariable variable, bool isSelection, double value)
        {
            Variable = variable;
            IsSelection = isSelection;
            Value = value;
        } 

        public bool BoolValue()
        {
            if (IsSelection) return Value != 0;
            return true;
        }
    }
    
    public class IncludePythonStatement
    {
        public string FileName;

        public IncludePythonStatement(string fileName)
        {
            FileName = fileName;
        }
    }

    public class ArcherObjectiveExpression
    {
        public ArcherObjective Objective;
        public ArcherExpression Expression;

        public ArcherObjectiveExpression(ArcherObjective archerObjective, ArcherExpression archerExpression)
        {
            Objective = archerObjective;
            Expression = archerExpression;
        }
    }

    public enum ArcherObjective
    {
        Minimize,
        Maximize
    }
}