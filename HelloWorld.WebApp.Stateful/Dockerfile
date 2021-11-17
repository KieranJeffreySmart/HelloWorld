FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-nanoserver-1809 AS build
WORKDIR /src
COPY ["HelloWorld.WebApp.Stateful.csproj", ""]
RUN dotnet restore "HelloWorld.WebApp.Stateful.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "HelloWorld.WebApp.Stateful.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HelloWorld.WebApp.Stateful.csproj" -c Release -o /app/build

FROM base AS final
WORKDIR /app
VOLUME c:/app/log
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "HelloWorld.WebApp.Stateful.dll"]