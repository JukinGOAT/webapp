# Use the .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["webapp.csproj", "."]
RUN dotnet restore "./webapp.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "webapp.csproj" -c Release -o /app/build
RUN dotnet publish "webapp.csproj" -c Release -o /app/publish

# Use the ASP.NET Core runtime image for the final app
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "webapp.dll"]