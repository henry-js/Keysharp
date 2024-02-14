;#Include %A_ScriptDir%/header.ahk

b := false

try
{
	throw Error("asdf")
}
catch
	b := true

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false

try
{
	throw Error("asdf")
}
catch
{
	b := true
}

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false

try
{
	throw Error("asdf")
} catch
	b := true

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false

try
{
	throw Error("asdf")
}
catch Error
{
	b := true
}

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false

try
{
	throw Error("asdf")
} catch Error
{
	b := true
}

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false

try
{
	throw Error("asdf")
} catch Error
	b := true

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false
str := ""

try
{
	throw Error("tester")
}
catch Error as errex
{
	b := true
	str := errex.RawMessage
}

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

if (str == "tester")
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false

try
{
	throw Error("tester")
}
catch Error as errex
{
}
finally
{
	b := true
}

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false
str := ""

try
{
	throw Error("tester")
}
catch Error as errex
{
}
else
{
	b := true
}

if (b == false)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

if (str == "")
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false

try
{
	throw IndexError("tester")
}
catch IndexError as errex
{
	b := true
}

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false

try
{
	throw IndexError("tester")
}
catch IndexError as errex
{
	b := true
}

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false

try
{
	throw KeyError("tester")
}
catch KeyError as errex
{
	b := true
}

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false

try
{
	throw MemberError("tester")
}
catch MemberError as errex
{
	b := true
}

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false

try
{
	throw MemoryError("tester")
}
catch MemoryError as errex
{
	b := true
}

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false

try
{
	throw MethodError("tester")
}
catch MethodError as errex
{
	b := true
}

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false

try
{
	throw OSError(123)
}
catch OSError as errex
{
	b := true
}

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false

try
{
	throw PropertyError("tester")
}
catch PropertyError as errex
{
	b := true
}

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false

try
{
	throw TargetError("tester")
}
catch TargetError as errex
{
	b := true
}

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false

try
{
	throw TimeoutError("tester")
}
catch TimeoutError as errex
{
	b := true
}

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false

try {
	throw TypeError("tester")
}
catch TypeError as errex
{
	b := true
}

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false

try {
	throw ValueError("tester")
}
catch ValueError as errex {
	b := true
}

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false

try ; this is a comment
{
	throw ZeroDivisionError("tester")
} catch ZeroDivisionError as errex { ; another comment
	b := true
}
catch(KeysharpException) ; last comment
{
	b := false
}
catch(OSError) {
	b := false
} catch(IndexError) {
	b := false
}
catch(propertyerror)
{
	b := false
}
catch(KeyError())
{
	b := false
}
catch(membererror)
{
	b := false
}
catch(MemoryError) {
	b := false
}
catch(MethodError) {
	b := false
}
catch(targeterror)
{
	b := false
}

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false

try b := true

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

try bb := true

if (bb == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false
try throw Error("test")
catch
{
	b := true
}

if (b == true)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := true
xx := 0

if b
{
	try
	{
		loop
		{
			xx++
			if (xx == 5)
				break
		}
	}
}
else
	xx := 0
	
if (xx == 5)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

b := false
xx := 0

if b
{
	try
	{
		loop
		{
			xx++
			if (xx == 5)
				break
		}
	}
}
else
	xx := 123
	
if (xx == 123)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

xx := 0

try loop
{
	xx++
	if (xx == 5)
		break

}

if (xx == 5)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

xx := 0

try while (xx < 5)
	xx++

if (xx == 5)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

xx := 0

try xx++

if (xx == 1)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

try xx := StrLen("hello")

if (xx == 5)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

xx := 0

loop
	try
		xx++
	catch
		x := 0
until xx > 2

if (xx == 3)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"