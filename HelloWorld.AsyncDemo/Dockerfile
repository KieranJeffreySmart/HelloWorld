#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /src
COPY ["HelloWorld.AsyncDemo.csproj", ""]
RUN dotnet restore "HelloWorld.AsyncDemo.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "HelloWorld.AsyncDemo.csproj" -c Release -o /app/build

RUN dotnet tool install --tool-path /tools dotnet-trace

FROM build AS publish
RUN dotnet publish "HelloWorld.AsyncDemo.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /tools
COPY --from=build /tools .  
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["../tools/dotnet-trace", "collect", "--", "dotnet", "./HelloWorld.AsyncDemo.dll"]
