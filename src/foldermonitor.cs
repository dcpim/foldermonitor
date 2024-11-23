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
			Console.WriteLine("Folder Monitor - (C) 2024 Patrick Lambert - https://dendory.net");
			Console.WriteLine("Provided as free software under the MIT license.");
			Console.WriteLine("");

			// Variables
			string prgfolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string prgfile = Assembly.GetExecutingAssembly().GetName().Name;
			string inifile = prgfolder + "\\" + prgfile + ".ini";
			string logfile = prgfolder + "\\" + prgfile + ".log";
			var src = new StringBuilder(255);
			var dst = new StringBuilder(255);
			var wildc = new StringBuilder(255);
			var debug = new StringBuilder(255);
			var act = new StringBuilder(255);
			var overw = new StringBuilder(255);
			var eoe = new StringBuilder(255);
			var log = new StringBuilder(255);
			var waitt = new StringBuilder(255);

			// Read INI file
			if(!File.Exists(inifile))
			{
				Console.WriteLine("Error: Configuration file [" + inifile + "] not found.");
				Environment.Exit(1);
			}

			GetPrivateProfileString(prgfile, "SourceFolder", "", src, 255, inifile);
			GetPrivateProfileString(prgfile, "DestinationFolder", "", dst, 255, inifile);
			GetPrivateProfileString(prgfile, "Wildcard", "", wildc, 255, inifile);
			GetPrivateProfileString(prgfile, "Debug", "", debug, 255, inifile);
			GetPrivateProfileString(prgfile, "Action", "", act, 255, inifile);
			GetPrivateProfileString(prgfile, "Overwrite", "", overw, 255, inifile);
			GetPrivateProfileString(prgfile, "Log", "", log, 255, inifile);
			GetPrivateProfileString(prgfile, "ExitOnError", "", eoe, 255, inifile);
			GetPrivateProfileString(prgfile, "WaitTime", "", waitt, 255, inifile);

			// Hide window if not in debug mode
			if(debug.ToString().ToLower() == "false")
			{
				var handle = GetConsoleWindow();
				ShowWindow(handle, 0);
			}

			// Main loop
			while(true)
			{
				Console.WriteLine("Monitoring " + src.ToString() + "\\" + wildc.ToString() + "...");
				DirectoryInfo d = new DirectoryInfo(@src.ToString());
				FileInfo[] Files = d.GetFiles(wildc.ToString());
				foreach(FileInfo file in Files) // List all files in SourceFolder matching Wildcard
				{
					try
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
							if(log.ToString().ToLower() == "true")
							{
								File.AppendAllText(logfile, "Copy: " + srcfile + " => " + dstfile + Environment.NewLine);
							}
							File.Copy(srcfile, dstfile);
						}
						else // Move file to destination
						{
							Console.WriteLine("* Moving file " + srcfile + " to " + dstfile);
							if(log.ToString().ToLower() == "true")
							{
								File.AppendAllText(logfile, "Move: " + srcfile + " => " + dstfile + Environment.NewLine);
							}
							File.Move(srcfile, dstfile);
						}
					}
					catch(Exception ex)
					{
						Console.WriteLine("Error: " + ex.Message);
						if(log.ToString().ToLower() == "true")
						{
							File.AppendAllText(logfile, "Error: " + ex.Message + Environment.NewLine);
						}
						if(eoe.ToString().ToLower() == "true")
						{
							Environment.Exit(1);
						}
					}
				}
				System.Threading.Thread.Sleep(int.Parse(waitt.ToString()));
			}
		}
	}
}
