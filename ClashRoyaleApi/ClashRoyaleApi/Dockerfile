FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App
EXPOSE 80

ARG TimeStamp
ENV TimeStamp=${TimeStamp}

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out 

RUN apt-get update && apt-get install -y tzdata

ENV TZ=Europe/Amsterdam

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "ClashRoyaleApi.dll"]