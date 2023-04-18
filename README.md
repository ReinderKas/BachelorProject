# Bachelor Project - Feature Model Analysis: a case study
[Overleaf](https://www.overleaf.com/project/63ec92e134a27342e8a123d5) <br>

## Tasks to do
[Backlog (Jira)](https://reinderkas.atlassian.net/jira/software/projects/BP/boards/1/roadmap)

&nbsp;

# Antlr
## Getting started <br>
[download](https://www.nuget.org/packages/Antlr/) - Nuget package<br>
[tutorial](https://tomassetti.me/getting-started-with-antlr-in-csharp/) - Getting started with antlr in C#. <br>



## Videos:
 - [Formal Methods for the Informal Engineer: Tutorial #1 - The Z3 Theorem Prover](https://www.youtube.com/watch?v=56IIrBZy9Rc)

## Discussions:
 - [(Github, Dec 9 - 2020) On proof generation and proof checking](https://github.com/Z3Prover/z3/discussions/4881)
 - [(Github, Aug 20 - 2020) Proof Checking in Z3 - state of the art](https://github.com/Z3Prover/z3/discussions/5000)

## Implementation
 - :white_check_mark:[Z3 Theorem Prover](https://github.com/Z3Prover/z3) (Z3 version 4.12.1 - 64 bit) <br>
 - :black_square_button: [DRAT-trim](https://github.com/marijnheule/drat-trim) (SAT-problems)
 - :white_check_mark:[SMT-lib](https://smtlib.cs.uiowa.edu/) <br>

## Documentation
 - [Documentation smt-lib Z3](https://compsys-tools.ens-lyon.fr/z3/smt-lib-reference-v2.5-r2015-06-28.pdf)

## Examples
 - [DRAT-trim](https://www.cs.utexas.edu/~marijn/drat-trim/#examples)
 - [Playground lyonFr](https://compsys-tools.ens-lyon.fr/z3/)
 - [Playground](https://jfmc.github.io/z3-play/)


&nbsp;
&nbsp;


## Ideas

### Diagnostics
 - Analyzing infeasibility of Constraint Satisfaction Problems.
 -   Diagnostics on CSPs
  * Z3 proof generator
  * Inference engines

[Direct Debug](https://github.com/AIG-ist-tugraz/DirectDebug)



Currently the Diagnostics is very basic. <br>
It would be nice to be able to give changes to the model in order to fix broken models. <br>
 
- Proof generator.
- Proof by contradiction on CSPs.
- Also with Expressions (non-logic).
- Z3 proof generator.
