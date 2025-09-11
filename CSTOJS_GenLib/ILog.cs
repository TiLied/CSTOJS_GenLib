using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace CSTOJS_GenLib
{
	internal interface ILog
	{
		static ILog()
		{
			if (File.Exists(Directory.GetCurrentDirectory() + "/debugGenCSharpLib.txt"))
				File.Delete(Directory.GetCurrentDirectory() + "/debugGenCSharpLib.txt");

			Trace.Listeners.Add(new TextWriterTraceListener("debugGenCSharpLib.txt"));
			Trace.AutoFlush = true;
			Trace.Listeners.Add(new ConsoleTraceListener());
		}

		public void WriteLine(string message, [CallerFilePath] string? file = null, [CallerMemberName] string? member = null, [CallerLineNumber] int line = 0)
		{
			Trace.WriteLine($"({line}):{Path.GetFileName(file.Replace("\\", "/"))} {member}: {message}");
		}
	}
}
