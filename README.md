# Lets-go-biking
![alt text](https://i.imgur.com/skWl39Z.png)

The objective of this project is to create all the parts of an application which would allow the user to find its way from any location to any other location (in a same city for a first release) by using as much as possible the bikes offered by [JC Decaux](https://en.wikipedia.org/wiki/JCDecaux).

This project is intended to run in a Windows Environment.

Requirements
-----------
<ul>
    <li> <a href="https://nodejs.org/en/">Node</a></li>
    <li> <a href="https://dotnet.microsoft.com/download">.NET 6.0</a></li>
    <li> <a href="https://dotnet.microsoft.com/download/dotnet-framework/net472">.NET Framework v4.7.2</a></li>
</ul>

Quick start
-----------
Launch All Modules of the project with a simple double click on:
        
    WindowsRun.cmd
    
Docker
-----------
* Docker Build: `` docker build --tag=host/biking .``
* Docker Run: `` docker run --rm -it -v "pwd":/host host/biking ``

Project Architecture
-----------
![alt text](https://i.imgur.com/E3D15qY.png)

Project Structure
-----------
```
Lets-go-biking
│   README.md
│   Dockerfile 
│   lisezmoi.txt          # Instructions in French
│   readme.txt            # Instructions in English
│   WindowsRun.cmd        # Startup program to run the backend and all clients (Heavy and Light)
│   ... 
└───HeavyClient
│   │   ...
│   └───bin
│       │   ...
│       └───Debug
│           │   ...
│           └───netcoreapp3.1
│           │   ...
│           │   HeavyClient.exe # The "Heavy Client" That represent the desktop Application
│           │   ...
└───LightClient
│   │   ...
│   └───ClientApp         # ReactJs Project running on localhost:3000
│       │   ...            
└───Routing
│   │   ...
│   │   IService1.cs      # A WCF Service Interface that contains Service1 contracts Signature
│   │   Service1.cs       # A WCF Service implementing an exposed contract to the clients
│   └───bin
│       │   ...
│       └───Debug
│           │   ...
│           │   Routing.exe # WCF application authorizing SOAP and REST requests from clients
│           │   ...
└───WebProxyService
│   │   ...
│   │   IService1.cs      # A WCF Service Interface that contains Service1 contracts Signature
│   │   Service1.cs       # A WCF Service implementing operation contracts for the Routing Server
│   └───bin
│       │   ...
│       └───Debug
│           │   ...
│           │   WebProxyService.exe  # Base WCF application making calls to external apis like api.jcdecaux.com
│           │   ...
```

Technical Issues
-----------
Some users might encounter minor issues when starting the launch file such as:
    
* > HTTP could not register URL http://+:8733/Design_Time_Addresses/Routing/Service. Your process does not have access rights to this namespace (see http://go.microsoft.com/fwlink/?LinkId=70353 for details).

To resolve this issue, please follow step by step the indications below.

1. Open a powershell or command-line terminal with Administrator rights.
2. Type ``net user`` to see all users on your computer, and retain your user name.
3. Type ``netsh http add urlacl url=http://+:8733/Design_Time_Addresses/ user=<username>`` and replace ``username`` with yours.
4. If the addition is successful you should see ``URL reservation successfully added``

Now you can restart WindowsRun.cmd and get a working Server and HeavyClient.

* > Windows Defender SmartScreen prevented an unrecognized app from starting. Running this app might put your PC at risk.

  Obviously the program is not intended to harm your computer. To resolve this issue:

        1. Right click on WindowsRun.cmd
        2. Select properties option.
        3. Click on checkbox to check Unblock at the bottom of Properties.
