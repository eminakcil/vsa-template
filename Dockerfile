FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["VsaTemplate.csproj", "./"]
RUN dotnet restore "./VsaTemplate.csproj"

COPY . .
RUN dotnet build "VsaTemplate.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VsaTemplate.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .

RUN mkdir -p /app/Logs && chown -R $APP_UID /app/Logs

USER $APP_UID

ENTRYPOINT ["dotnet", "VsaTemplate.dll"]