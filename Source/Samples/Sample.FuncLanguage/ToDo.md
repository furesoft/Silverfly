[ ] implement unpacking operator
    myoption? => myoption.unpack() 
[ ] implement custom data types
type Box(ref)
[ ] add better syntax for indexing objects by range
        current: obj.1..15

[ ] implement comparison with string on Symbol to avoid duplicative explciit casting like: if(mysymbol == (Symbol)"+")

[ ] implement to scope to import clr classes/functions
[ ] implement functions to determine if value is of specific value type or implement operator
    isNumber(val) -> bool
    isBool(val) -> bool
    val is number -> isNumber(val)
    binary operator will be rewritten to a function call

    sample:
    if (val is number) then 4
    else if (val is bool) then 5
    else ()

[ ] implement function to call clr methods on Values
