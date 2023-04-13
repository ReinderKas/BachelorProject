(set-option :produce-unsat-cores true)
(set-option :produce-proofs true)

; Variable declarations
(declare-fun a () Bool)
(declare-fun b () Bool)
(declare-fun c () Bool)


; Constraints
(assert(=> a b))
(assert(=> a c))
(assert(=> a (not b)))
(assert(= a true))

; Solve
(check-sat)
(get-proof)
(get-unsat-core) 