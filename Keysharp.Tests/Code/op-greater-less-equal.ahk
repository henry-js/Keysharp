

x := 1
y := 1

If (x <= y)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

If (x >= y)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

If (!(x <= y))
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x >= y))
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"
	
x := 1
y := 2

If (x <= y)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

If (x >= y)
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x <= y))
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x >= y))
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

x := -1
y := -1

If (x <= y)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

If (x >= y)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

If (!(x <= y))
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x >= y))
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

x := -1
y := 2

If (x <= y)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

If (x >= y)
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x <= y))
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x >= y))
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

x := -2
y := -1

If (x <= y)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

If (x >= y)
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x <= y))
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x >= y))
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

x := 1.234
y := 1.234

If (x <= y)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

If (x >= y)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

If (!(x <= y))
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x >= y))
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"
	
x := 1.234
y := 2.456

If (x <= y)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

If (x >= y)
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x <= y))
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x >= y))
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

x := -1.234
y := -1.234

If (x <= y)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

If (x >= y)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

If (!(x <= y))
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x >= y))
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"
	
x := -1.234
y := 2.456

If (x <= y)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

If (x >= y)
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x <= y))
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x >= y))
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"


x := -2.234
y := -1.456

If (x <= y)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

If (x >= y)
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x <= y))
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x >= y))
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

x := 0
y := 0

If (x <= y)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

If (x >= y)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

If (!(x <= y))
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x >= y))
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"
	
x := 0
y := 1

If (x <= y)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

If (x >= y)
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x <= y))
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x >= y))
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

x := "a"
y := "a"

If (x <= y)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

If (x >= y)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

If (!(x <= y))
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x >= y))
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"	

x := "a"
y := "b"

If (x <= y)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

If (x >= y)
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x <= y))
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"

If (!(x >= y))
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"