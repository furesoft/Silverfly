module Box;

type Box(ref);

let make val = new Box(val)
let set val box = box.ref = val
let get box = box.ref
