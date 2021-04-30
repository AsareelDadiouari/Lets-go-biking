#!/bin/bash
# shellcheck disable=SC2164
cd Host/bin/Debug/
mono ./Host.exe
cd ../../../HeavyClient/bin/Debug/netcoreapp3.1
mono ./HeavyClient.exe