(set-option :produce-unsat-cores true)
(set-option :produce-proofs true)

; Variable declarations
(declare-fun a () Bool)
(declare-fun b () Bool)

; Constraints
(assert(= a b))
(assert(not(= a b)))

; Solve
(check-sat)
(get-proof)
(get-unsat-core)