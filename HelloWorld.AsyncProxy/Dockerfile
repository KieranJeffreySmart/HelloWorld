FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["HelloWorld.AsyncProxy.csproj", ""]
RUN dotnet restore "HelloWorld.AsyncProxy.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "HelloWorld.AsyncProxy.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HelloWorld.AsyncProxy.csproj" -c Release -o /app/publish

ENV HELLOWORLD_URL=http://localhost:5000


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# In final stage
# Copy diagnostics tools
ENTRYPOINT ["dotnet", "./HelloWorld.AsyncProxy.dll"]