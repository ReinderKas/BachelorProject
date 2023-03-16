; CLI Result
((proof
 (let (($x35 (not one)))
 (let ((@x34 (monotonicity (rewrite (= (= a (not a)) false)) (= (=> one (= a (not a))) (=> one false)))))
 (let ((@x39 (trans @x34 (rewrite (= (=> one false) $x35)) (= (=> one (= a (not a))) $x35))))
 (let ((@x40 (mp (asserted (=> one (= a (not a)))) @x39 $x35)))
 (unit-resolution @x40 (asserted one) false)))))))

; Formatted
(
    let 
    (
        (
            @x34 
            (
                monotonicity 
                (
                    rewrite 
                    (= one false)                                   ; (x => false)
                ) 
                (
                    = (=> one one) (=> one false)                   ; (x => x) = (x => false)
                )
            )
        )
    )
    (
        let 
        (
            (
                @x39 
                (
                    trans @x34 
                    (
                        rewrite 
                        (= (=> one false) $x35)                     ; (x => false) = !x
                    ) 
                    (
                        = (=> one one) $x35                         ; (x => x) = !x
                    )
                )
            )
        )
        (
            let 
            (
                (
                    @x40 
                    (
                        mp 
                        (asserted (=> one (= a (not a))))           ; asserted (x => x)
                        @x39                                        ; (x => x) = !x
                        $x35                                        ; !x
                    )
                )
            )
            
            (unit-resolution @x40 (asserted one) false)             ; asserted (x => x) 
                                                                    ; AND ((x => x) = !x) 
                                                                    ; AND (!x)
                                                                    ; AND asserted(!x)
        )
    )
)