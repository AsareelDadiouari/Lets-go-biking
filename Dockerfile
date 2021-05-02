FROM microsoft/powershell:latest

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

WORKDIR /docker

COPY Host/bin/Debug/Host.exe Host/bin/Debug/Host.exe

ADD Host/bin/Debug/Host.exe Host.exe

EXPOSE 9000

ENTRYPOINT ["pwsh","Host.exe"]