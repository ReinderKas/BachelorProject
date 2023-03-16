; CLI result
((proof
    (mp (asserted (= a (not a))) (rewrite (= (= a (not a)) false)) false)))

; Formatted
(mp 
    (asserted 
        (= a (not a))               ; a = !a
    ) 
    (rewrite 
        (= (= a (not a)) false)     ; (a = !a) = false
    )
    false                           ; false
)