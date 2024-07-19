import std // import all members of std.f to the current scope

let outer = Box.make(())
let doWithBox b = Box.set(3, b)

doWithBox(outer)
print(outer)

let call f x y = f(x, y) // define function that can call functions

let x = 2 // define a variable
print(call('+, x, 2)) // call the + operator

let d = y -> 1 + y // define a function
print(call(d, 2))

print(((x, y) -> x + y)(1, 2)) // call anonymous function directly

print(String.length("hello")) // call a static function from a module
print("hello".length) // get an attribute from a value
print("hello".0) // index based access - returns the first character
print("hello".(1+1)) // computed index based access . returns the third character

let m = (2,3,5,7,11) // define a tuple variable
print(m.2) //print the third element of m - 5

// define functions to be decorated to other functions
let on_enter call = print("Entering " + call)
let on_exit call value = print("Exiting " + call + " with " + value)

// use the functions from above as decorators for f
@enter(on_enter)
@exit(on_exit)
let f x = print(x);

f("hello") // before calling f 'on_enter' will be called and after calling f 'on_exit' will be called.

let add1 x = x + 1
let mul2 x = x * 2

let add1mul2 x = add1 + mul2 //combine add1 and mul2 as new function

print(add1mul2(2)) //prints 6
