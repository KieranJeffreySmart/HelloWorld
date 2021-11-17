FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5050

ENV ASPNETCORE_URLS=http://*:5050

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["HelloWorld.csproj", ""]
RUN dotnet restore "HelloWorld.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "HelloWorld.csproj" -c Release -o /app/build 

FROM build AS publish
RUN dotnet publish "HelloWorld.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HelloWorld.dll"]