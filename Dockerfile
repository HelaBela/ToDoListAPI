
FROM mcr.microsoft.com/dotnet/core/runtime:2.2

COPY ToDoList/bin/Release/netcoreapp2.2/published/ ToDoList

ENTRYPOINT ["dotnet", "ToDoList/ToDoList.dll"]


