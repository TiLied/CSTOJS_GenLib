using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System;

namespace CSTOJS_GenLib;

internal class Log
{
	public static void WriteLine(string message, [CallerFilePath] string? file = null, [CallerMemberName] string? member = null, [CallerLineNumber] int line = 0)
	{
		Console.WriteLine($"({line}):{Path.GetFileName(file.Replace("\\", "/"))} {member}: {message}");
	}
}

