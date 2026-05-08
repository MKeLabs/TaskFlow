FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["TaskFlow.API/TaskFlow.API.csproj", "TaskFlow.API/"]
COPY ["TaskFlow.BLL/TaskFlow.BLL.csproj", "TaskFlow.BLL/"]
COPY ["TaskFlow.DAL/TaskFlow.DAL.csproj", "TaskFlow.DAL/"]
RUN dotnet restore "TaskFlow.API/TaskFlow.API.csproj"

COPY . .
WORKDIR "/src/TaskFlow.API"
RUN dotnet build "TaskFlow.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TaskFlow.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskFlow.API.dll"]
