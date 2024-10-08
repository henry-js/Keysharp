#if WINDOWS
	pid := 0
	Run("notepad.exe", "", "max", &pid)
	ProcessWait(pid)
	ProcessSetPriority("H", pid)
	exists := ProcessExist(pid)

	if (exists != 0)
	{
		Sleep(2000)
		ProcessClose(pid)
		ProcessWaitClose(pid)
	}

	Sleep(1000)
	exists := ProcessExist("notepad.exe")

	if (exists == 0)
		FileAppend, "pass", "*"
	else
		FileAppend, "fail", "*"

	pid := RunWait("notepad.exe", "", "max")
	Sleep(1000)
	exists := ProcessExist("notepad.exe")

	if (exists == 0)
		FileAppend, "pass", "*"
	else
		FileAppend, "fail", "*"
#else
	pid := 0
	Run("xed", "", "max", &pid)
	ProcessWait(pid)
	exists := ProcessExist(pid)

	if (exists != 0)
	{
		Sleep(2000)
		ProcessClose(pid)
		ProcessWaitClose(pid)
	}

	Sleep(1000)
	exists := ProcessExist("xed")

	if (exists == 0)
		FileAppend, "pass", "*"
	else
		FileAppend, "fail", "*"

	pid := RunWait("xed", "", "max")
	Sleep(1000)
	exists := ProcessExist("xed")

	if (exists == 0)
		FileAppend, "pass", "*"
	else
		FileAppend, "fail", "*"
#endif