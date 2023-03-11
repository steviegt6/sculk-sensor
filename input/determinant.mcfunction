# test
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
