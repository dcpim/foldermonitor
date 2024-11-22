# Folder Monitor

## Description

Folder Monitor is a simple Windows application that monitors a folder for files matching a specific wildcard every 5 seconds, then copies moves those files to another folder. Configuration is found in the INI file of the same name.


## Configuration

The INI file should be in the same location as the binary and should have the following entries:

* SourceFolder - The name of the folder to monitor.
* DestinationFolder - The name of the folder where files should be moved or copied to.
* Wildcard - The type of files to act on.
* Overwrite - Whether to overwrite target files or stop executing if not.
* Debug - Whether to show a summary console window.
* Log - Log activity in the foldermonitor.log file.
* ExitOnError - Exit the application if an error occurs.


## Copyright

Copyright 2024 Patrick Lambert - https://dendory.net

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the “Software”), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
