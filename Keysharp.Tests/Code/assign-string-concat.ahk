

x = hello
y := x . " world"

If x != "hello"
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"
	
If y != "hello world"
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"
	
If y = "hello world"
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"
	
y := x " world"

If y != "hello world"
	FileAppend, "fail", "*"
else
	FileAppend, "pass", "*"
	
If y = "hello world"
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

y := x . " world " x
	
If (y == "hello world hello")
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

y := x " world " . x
	
If (y == "hello world hello")
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

y := x . " world " . x
	
If (y == "hello world hello")
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

y := x " world " x
	
If (y == "hello world hello")
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"