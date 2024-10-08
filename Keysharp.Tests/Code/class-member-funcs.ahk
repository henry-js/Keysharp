a := ""

class myclass
{
	a := ""
	b :=
	c := "asdf"
	x := 123
	y := x
	static s1 := 10

	classfunc()
	{
		return 123
	}

	static classfuncstatic()
	{
		return s1
	}

	classfuncusesstatic()
	{
		return s1 * x
	}

	classfuncwithlocalvars()
	{
		lv1 := 10
		lv2 := 10
		return lv1 * lv2
	}

	classfuncwithreadmembervars()
	{
		return x * y
	}

	classfuncwithwritelocalmembervars()
	{
		x := 88
		y := 99
	}

	classfuncwithwritemembervars()
	{
		global
		x := 88
		y := 99
	}

	classfuncwithlocalstaticvars()
	{
		static aa := 100
		return aa * 10
	}

	classfuncwriteglobalvars()
	{
		global a := 0
		program.a := 1
	}
	
	static classfuncstaticwithparams(val1, val2)
	{
		return val1 * val2
	}

	classfuncwithparams(val1, val2)
	{
		return val1 * val2
	}

	classvarfunc(p1, theparams*)
	{
		temp := p1

		for n in theparams
		{
			temp += theparams[A_Index]
		}
	
		temp += p1

		for n in theparams
		{
			temp += n
		}

		return temp
	}
	
	static classvarfuncstatic(p1, theparams*)
	{
		temp := p1

		for n in theparams
		{
			temp += theparams[A_Index]
		}
	
		temp += p1

		for n in theparams
		{
			temp += n
		}

		return temp
	}
	
	classfuncwiththis()
	{
		this.a := 999
		val := this.a
		return val
	}

	ClassFuncCaseSensitive()
	{
		this.a := 1000
		ClassFuncCaseSensitive2()
		classfunccasesensitive2()
	}

	ClassFuncCaseSensitive2()
	{
		this.b := 2000
	}

	static ClassFuncCaseSensitiveStatic()
	{
		ClassFuncCaseSensitiveStatic2()
		classfunccasesensitivestatic2()
	}

	static ClassFuncCaseSensitiveStatic2()
	{
		global s1 := 999
	}
}

classobj := myclass()
val := classobj.classfunc()

If (val == 123)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

val := myclass.classfuncstatic()

If (val == 10)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

val := classobj.classfuncusesstatic()

If (val == 1230)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

myclass.s1 := 1

val := classobj.classfuncusesstatic()

If (val == 123)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

val := classobj.classfuncwithlocalvars()

If (val == 100)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

val := classobj.classfuncwithreadmembervars()

If (val == 15129)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

classobj.classfuncwithwritelocalmembervars()

if (classobj.x == 123)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

if (classobj.y == 123)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

classobj.classfuncwithwritemembervars()

if (classobj.x == 88)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

if (classobj.y == 99)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

val := classobj.classfuncwithlocalstaticvars()

if (val == 1000)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

classobj.classfuncwriteglobalvars()

if (classobj.a == 0)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

if (program.a == 1)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

val := myclass.classfuncstaticwithparams(150, 2)

if (val == 300)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

val := classobj.classfuncwithparams(500, 2)

if (val == 1000)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

val := myclass.classvarfuncstatic(1, 2, 3)

If (val == 12)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"
	
val := classobj.classvarfunc(1, 2, 3)

If (val == 12)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

val := classobj.classfuncwiththis()

If (val == 999)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

classobj.ClassFuncCaseSensitive()

if (classobj.a == 1000)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

if (classobj.b == 2000)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

classobj.a := ""
classobj.b := ""

classobj.classfunccasesensitive()

if (classobj.a == 1000)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

if (classobj.b == 2000)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

myclass.s1 := ""
myclass.ClassFuncCaseSensitiveStatic()

if (myclass.s1 == 999)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

funcadd := FuncObj("classfuncwithparams", classobj)

val := funcadd(10, 20)

if (val == 200)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

funcadd := FuncObj("classfuncstaticwithparams", classobj)

val := funcadd(10, 10)

if (val == 100)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

funcadd := FuncObj("classvarfunc", classobj)

val := funcadd(1, 2, 3)

if (val == 12)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

funcadd := FuncObj("classvarfuncstatic", classobj)

val := funcadd(1, 2, 3)

if (val == 12)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

; Test command style when using methods.

class myclass2
{
	classfunc0()
    {
		program.a := 0
    }

    classfunc1(p1)
    {
		program.a := p1
    }
    
    classfunc2(p1, p2)
    {
		program.a := p1 + p2
    }

    classfunc3(p1, p2, p3*)
    {
		temp := p1 + p2

		for n in p3
		{
			temp += p3[A_Index]
		}
	
		program.a := temp
    }
}

a := ""
class2obj := myclass2()
class2obj.classfunc0

if (a == 0)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

a := ""
class2obj.classfunc1 1

if (a == 1)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

a := ""
class2obj.classfunc2 1, 2

if (a == 3)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"

a := ""
class2obj.classfunc3 1, 2, 4, 5, 6

if (a == 18)
	FileAppend, "pass", "*"
else
	FileAppend, "fail", "*"