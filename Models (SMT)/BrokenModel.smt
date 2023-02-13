(set-option :produce-unsat-cores true)
(set-option :produce-proofs true)

; Variable declarations
(declare-fun a () Bool)

; Constraints
(assert (= a (not a)))

; Solve
(check-sat)
(get-proof)
(get-unsat-core)