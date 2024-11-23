@echo off
reg add "HKCU\Software\Microsoft\Windows\CurrentVersion\Run" /v "FolderMonitor" /t REG_SZ /d "%~dp0foldermonitor.exe" /f
