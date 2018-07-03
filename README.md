

# Startup


Startup is a small utility, to automate the start of non-service processes on Windows Server.
It provides a little bit more control over the `Startup` folder of one user account.


---


**Version 2.0.0.2 Release**
#### **[DOWNLOAD](https://github.com/MrX13415/Startup/releases)**

This tool is only compatible with **Windows 10** and **Windows Server 2016**.
Make sure, you are at least on **Version 1607** of Windows.



## Installation


1. Place the `_Bin` folder anywhere on your hard drive.

2. Move the `Config` folder to a desired location. 

3. Create a link to the `_Bin/startup.exe` in the `startup`
   folder of the appropriate user account.
   (To go to the `startup` folder of the current user,
   type `shell:startup` in the address bar and hit **Enter**.)
 
4. Open and edit the `_Bin/Startup.exe.config` file to match your needs.
   Make sure the path to your `Config` folder is set correctly.
 
6. To allow the desired user account to login by itself,
   you can use the tool **Autologon** form Sysinternals.
   Download: [Autologon - Windows Sysinternals](https://docs.microsoft.com/en-us/sysinternals/downloads/autologon)
 
   1. Download and open the tool **Autologon**
   2. Enter the name and password of the desired user account
   3. Click on **Enable**

   To disable auto login, run the tool again and Click on **Disable**.



## Usage


Place a link in the `Config` folder for any service to start on login.
Rename each link to match the following format:

 `<Index>.<Group> - <Name>`  e.g: `1.0 - MyService`
 
See `Config/Readme.txt` for more information.



## License


MIT License

Copyright (c) 2018 MrX13415

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.


---
