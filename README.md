# sculk-sensor

Work-in-progress, experimental decompiler for [Sculk](https://github.com/Trivaxy/sculk).

Example output (not exactly functional):

```
# sculk source file
fn determinant(a: int, b: int, c: int) -> int {
     return b * b - 4 * a * c;
}
    
fn main() {
    determinant(4, 1, 2);
}
```

```mcfunction
# compiled mcfunction files

# _sculkmain.mcfunction
scoreboard objectives add _SCULK dummy
function test:main

# main.mcfunction
scoreboard players set _TMP0 _SCULK 4
scoreboard players operation _ARGdeterminant0 _SCULK = _TMP0 _SCULK
scoreboard players set _TMP0 _SCULK 1
scoreboard players operation _ARGdeterminant1 _SCULK = _TMP0 _SCULK
scoreboard players set _TMP0 _SCULK 2
scoreboard players operation _ARGdeterminant2 _SCULK = _TMP0 _SCULK
function test:determinant

# determinant.mcfunction
scoreboard players operation _TMP0 _SCULK = _ARGdeterminant1 _SCULK
scoreboard players operation _TMP1 _SCULK = _ARGdeterminant1 _SCULK
scoreboard players operation _TMP0 _SCULK *= _TMP1 _SCULK
scoreboard players set _TMP1 _SCULK 4
scoreboard players operation _TMP2 _SCULK = _ARGdeterminant0 _SCULK
scoreboard players operation _TMP1 _SCULK *= _TMP2 _SCULK
scoreboard players operation _TMP2 _SCULK = _ARGdeterminant2 _SCULK
scoreboard players operation _TMP1 _SCULK *= _TMP2 _SCULK
scoreboard players operation _TMP0 _SCULK -= _TMP1 _SCULK
scoreboard players operation _RET _SCULK = _TMP0 _SCULK
```

```
# decompiled _sculkmain
fn _sculkmain() {
    # objects add _SCULK;
    /*test:*/main();
}

# decompiled main
n main() {
    var _TMP0;
    _TMP0 = 4;
    _ARGdeterminant0 = _TMP0;
    _TMP0 = 1;
    _ARGdeterminant1 = _TMP0;
    _TMP0 = 2;
    _ARGdeterminant2 = _TMP0;

    # note that this call actually fails because no args are passed
    /*test:*/determinant();
}

# decompiled determinant
fn determinant(_ARGdeterminant0, _ARGdeterminant1, _ARGdeterminant2) {
    var _TMP0;
    var _TMP1;
    var _TMP2;
    _TMP0 = _ARGdeterminant1;
    _TMP1 = _ARGdeterminant1;
    _TMP0 *= _TMP1;
    _TMP1 = 4;
    _TMP2 = _ARGdeterminant0;
    _TMP1 *= _TMP2;
    _TMP2 = _ARGdeterminant2;
    _TMP1 *= _TMP2;
    _TMP0 -= _TMP1;
    return _TMP0;
}
```