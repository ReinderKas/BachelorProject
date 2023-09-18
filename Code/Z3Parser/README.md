# To generate the parser:
Move to the [ParserObject](https://github.com/ReinderKas/BachelorProject/tree/main/Code/Z3Parser/Z3Parser/ParserObjects) folder where the `.g4` grammar file exists. <br>
In the folder run the following command:

```Console
antlr4 -Dlanguage=CSharp z3proof.g4
```

&nbsp;
&nbsp;

&nbsp;

TODO: Find out if I'm allowd to talk about this.

# **Feature Model**
Some usefull points to clarify before explaining the implementations for the models.
+ The Root of the Model must always be selected (i.e.: `true`).<br>
Turning off all the nodes in a model is always a solution to the problem, but this is not a solution we're interested in, since we know we want a given product configuration.

+ If a Child node is selected, then the parent node **MUST** be selected as well.<br>
There is no situation in which the child is selected and the parent is not.

&nbsp;


# Hierarchical Constraints
Hierarchical Constraints are the constraints that define the hierarchy and thus the Parent-Child relationships. <br>
Some things to note for Hierarchical constraints:
 + It is impossible to create a Feature Model consisting only of hierarchical constraints for which there is no solution. <br>
 Eventhough deleting/editing a hierarchical constraint could lead us to a solution when we find a unsatisfiable model, we already know that we do not want to do this. The search space for finding the correct 'fix' to the model will only be within (and due to) Cross Hierarchy constraints.

 + Again, it is impossible for a Child node to be selected when its Parent node is not selected. <br>

## Mandatory
The Mandatory constraint indicates that if a parent node is selected, the child node also always has to be selected. <br>
If the parent is `true`, then the child must be `true` as well.

```C#
Parent.isSelected == Child.isSelected
```

## Optional
The Optional constraint gives us the option to turn a node on or off depending on what we want. <br>
If the parent is `true`, the child does not necessarily have to be `true` as well.

```C#
!Child.isSelected || (Parent.isSelected == Child.isSelected)
```
## Alternative
In an Alternative constraint, we have one and only a single one node selected at one time. <br>
In essenct, we must have one selected, and cannot have more than one Node selected.

```C#

```

## Or
In an Or constraint, we have at least one node selected. <br>
In essence, we must have one Node selected, but we can have multiple as well.
```C#

```

# Cross Hierarchy Constraints
## Excludes
An Exclude constriant between two Nodes means that if one Node is selected, then the other Node cannot be selected. <br>
This works both ways, so if the second Node is selected, then the first Node cannot be selected either. <br>
If both Nodes are not selected, then this constraint is also satisfied.

## Requires
A Requires constraint between two Nodes means that if Node A is selected, then Node B must be selected as well. <br>
This is similar to a **Mandatory** constraint, but this constraint works between the hierarchies in stead of within the hierarchy. This distinction means that is deserves its own name. <br>
By definition, if Node B is selected, then Node A does not necessarily have to be selected. This only works from A to B. <br>
(The above is reasoning about the constraint [A] - requires -> [B])
```C#
!A.isSelected || (B.isSelected == A.isSelected)
```
