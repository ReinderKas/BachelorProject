(set-option :produce-proofs true)

; Variable declarations
(declare-fun a () Int)
(declare-fun b () Int)
(declare-fun c () Int)

; Constraints
(assert (> a 0))
(assert (> b 0))
(assert (> c 0))
(assert (= (+ (* a a) (* b b)) (* c c)))

; Solve
(check-sat)
(get-model)
(get-proof)
