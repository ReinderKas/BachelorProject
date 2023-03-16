((proof
(let (($x26 (= a b)))
 (let (($x28 (not $x26)))
 (let ((@x29 (asserted $x28)))
 (let ((@x27 (asserted $x26)))
 (unit-resolution @x27 (mp @x29 (rewrite (= $x28 $x28)) $x28) false)))))))


(
    let 
    (
        ($x26 (= a b))
    )
    (
        let 
        (
            ($x28 (not $x26))
        )
        (
            let 
            (
                (@x29 (asserted $x28))
            )
            (
                let 
                (
                    (@x27 (asserted $x26))
                )
                (
                    unit-resolution @x27 (mp @x29 (rewrite (= $x28 $x28)) $x28) false
                )
            )
        )
    )
)