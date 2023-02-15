@echo off

set VERSION=%1
set OUT_DIR=%2
set PROJ_PATH=%cd%\src\ModelledSystems\ModelledSystems.csproj

dotnet publish %PROJ_PATH% --configuration Release --framework net6.0 --output %OUT_DIR%\modelled-systems\net6.0 -p:VersionPrefix=%VERSION%