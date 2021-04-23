del Launch.exe
csc Startup.cs /r:WebProxyService/bin/Debug/WebProxyService.dll /r:Routing/bin/Debug/Routing.dll /out:Launch.exe /pdb:WebProxyService/bin/Debug/WebProxyService.pdb /pdb:Routing/bin/Debug/Routing.pdb -additionalfile:WebProxyService/bin/Debug/WebProxyService.dll.config -additionalfile:Routing/bin/Debug/Routing.dll.config
REM Launch.exe