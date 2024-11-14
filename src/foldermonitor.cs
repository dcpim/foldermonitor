// Folder Monitor is a simple application that monitors a folder for files matching a specific wildcard
// every 5 seconds, then moves those files to another folder. Configuration is found in the INI file of
// the same name.
//
// (C) Copyright 2024 Patrick Lambert - https://dendory.net
// Provided under the MIT License
//
// Compile with: C:\Windows\Microsoft.NET\Framework\v3.5\csc.exe /out:foldermonitor.exe foldermonitor.cs
//

using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Folder Monitor")]
[assembly: AssemblyCopyright("(C) 2024 Patrick Lambert")]
[assembly: AssemblyFileVersion("1.0.0")]

namespace FolderMonitor
{
	public class Program
	{
			// Import DLL modules
			[DllImport("kernel32.dll")]
			static extern IntPtr GetConsoleWindow();
			[DllImport("user32.dll")]
			static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
			[DllImport("kernel32", CharSet = CharSet.Unicode)]
			static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);
		static void Main(string[] args)
		{

			// Variables
			string prgfile = Assembly.GetExecutingAssembly().GetName().Name;
			string inifile = new FileInfo(prgfile + ".ini").FullName;
			var src = new StringBuilder(255);
			var dst = new StringBuilder(255);
			var wildc = new StringBuilder(255);
			var debug = new StringBuilder(255);
			var act = new StringBuilder(255);
			var overw = new StringBuilder(255);

			// Read INI file
			GetPrivateProfileString(prgfile, "SourceFolder", "", src, 255, inifile);
			GetPrivateProfileString(prgfile, "DestinationFolder", "", dst, 255, inifile);
			GetPrivateProfileString(prgfile, "Wildcard", "", wildc, 255, inifile);
			GetPrivateProfileString(prgfile, "Debug", "", debug, 255, inifile);
			GetPrivateProfileString(prgfile, "Action", "", act, 255, inifile);
			GetPrivateProfileString(prgfile, "Overwrite", "", overw, 255, inifile);

			// Hide window if not in debug mode
			if(debug.ToString().ToLower() != "true")
			{
				var handle = GetConsoleWindow();
				ShowWindow(handle, 0);
			}

			Console.WriteLine("Folder Monitor - (C) 2024 Patrick Lambert - https://dendory.net");
			Console.WriteLine("Provided as free software under the MIT license.");
			Console.WriteLine("");

			// Main loop
			while(true)
			{
				Console.WriteLine("Monitoring " + src.ToString() + "\\" + wildc.ToString() + "...");
				DirectoryInfo d = new DirectoryInfo(@src.ToString());
				FileInfo[] Files = d.GetFiles(wildc.ToString());
				foreach(FileInfo file in Files) // List all files in SourceFolder matching Wildcard
				{
					string srcfile = src.ToString() + "\\" + file.Name;
					string dstfile = dst.ToString() + "\\" + file.Name;
					if(overw.ToString().ToLower() == "true")
					{
						if(File.Exists(dstfile)) // Erase destination if Overwrite is set to true and file exists
						{
							File.Delete(dstfile);
						}
					}
					if(act.ToString().ToLower() != "move") // Copy file to destination
					{
						Console.WriteLine("* Copying file " + srcfile + " to " + dstfile);
						File.Copy(srcfile, dstfile);
					}
					else // Move file to destination
					{
						Console.WriteLine("* Moving file " + srcfile + " to " + dstfile);
						File.Move(srcfile, dstfile);
					}
				}
				System.Threading.Thread.Sleep(5000);
			}
		}
	}
}
