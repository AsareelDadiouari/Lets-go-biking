csc Startup.cs /r:WebProxyService.dll /r:Routing.dll
#powershell -command "Start-Sleep -s 5"
#ping 192.0.2.2 -n 1 -w 5000 > nul
#del nul
cmd.exe /c Startup.exe