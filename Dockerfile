FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["webapp.csproj", "."]
RUN dotnet restore "./webapp.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "webapp.csproj" -c Release -o /app/build
RUN dotnet publish "webapp.csproj" -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

RUN apt-get update \
 && apt-get install -y --no-install-recommends tzdata \
 && ln -fs /usr/share/zoneinfo/Europe/Zagreb /etc/localtime \
 && echo "Europe/Zagreb" > /etc/timezone \
 && dpkg-reconfigure --frontend noninteractive tzdata \
 && apt-get clean \
 && rm -rf /var/lib/apt/lists/*

 ENV TZ=Europe/Zagreb

COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "webapp.dll"]