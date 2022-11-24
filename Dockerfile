FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /Saitynai

# Copy everything
COPY /Saitynai/Saitynai/. ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /Saitynai
COPY --from=build-env /Saitynai/out .
ENTRYPOINT ["dotnet", "Saitynai.dll"]