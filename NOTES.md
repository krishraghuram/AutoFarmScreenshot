dotnet new classlib

dotnet add package Pathoschild.Stardew.ModBuildConfig --version 4.4.0

rm -rf "/home/raghuram/.local/share/Steam/steamapps/common/Stardew Valley/Mods/AutoFarmScreenshot" &&\
dotnet build -c Release