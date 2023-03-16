(set-option :produce-unsat-cores true)
(set-option :produce-proofs true)

; Variable declarations
(declare-fun a () Bool)
(declare-fun b () Bool)

; Constraints
(assert(!(= a b) :named first))
(assert(!(not(= a b)) :named second))

; Solve
(check-sat)
(get-proof)
(get-unsat-core)