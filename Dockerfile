
FROM mcr.microsoft.com/dotnet/core/runtime:2.2

RUN mkdir /app
WORKDIR /app
COPY ./ /app
RUN dotnet build


ENTRYPOINT ["dotnet", "ToDoList/ToDoList.dll"]

CMD ["run"]


