# Lets-go-biking
WCF Services, WPF Client, ASP .NET React

The objective of this project is to create all the parts of an application which would allow the user to find its way from any location to any other location (in a same city for a first release) by using as much as possible the bikes offered by JC Decaux.

This project is intended to run in a Windows Environment.

Requirements
-----------
<ul>
    <li> <a href="https://nodejs.org/en/">Node</a></li>
    <li> <a href="https://dotnet.microsoft.com/download/dotnet/3.1">.NET CORE 3.1</a></li>
    <li> <a href="https://dotnet.microsoft.com/download/dotnet-framework/net472">.NET Framework v4.7.2</a></li>
</ul>

Quick start
-----------
Launch All Modules of the project with a simple double click on:
        
    WindowsRun.cmd


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
Some users might encounter minor when starting the launch file such as:
    
> HTTP could not register URL http://+:8733/Design_Time_Addresses/Routing/Service. Your process does not have access rights to this namespace (see http://go.microsoft.com/fwlink/?LinkId=70353 for details).

To resolve this issue, please follow step by step the indications below.

* Open a powershell or command-line terminal with Administrator rights.
* Type ``net user`` to see all users on your computer, and retain your user name.
* Type ``netsh http add urlacl url=http://+:8733/Design_Time_Addresses/ user=<username>`` and replace ``username`` with yours.
* If the addition is successful you should see ``URL reservation successfully added``

Now you can restart WindowsRun.cmd and get a working Server and HeavyClient.
