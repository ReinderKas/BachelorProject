# Bachelor Project - Feature Model Analysis: a case study


## Tasks to do
### Before starting
 - :black_square_button: Write proposal?.

### Research:
 - :white_check_mark: Download/Setup Z3. 
 - :white_check_mark: Read [Z3/SMT-lib Documentation](https://compsys-tools.ens-lyon.fr/z3/smt-lib-reference-v2.5-r2015-06-28.pdf)
 - :white_check_mark: Generate proofs with Z3. 
 - :black_square_button: Investigate into feasibility of usage of [DRAT-trim](https://www.cs.utexas.edu/~marijn/drat-trim/).
 - :black_square_button: Write introduction explaining CSP.

&nbsp;

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
