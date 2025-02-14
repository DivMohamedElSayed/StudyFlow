Use .NET 9 SDK as the base image
FROM mcr.microsoft.com/dotnet/nightly:9.0 AS build

Set working directory
WORKDIR /app

Copy project files
COPY . ./

Restore dependencies
RUN dotnet restore

Build the application
RUN dotnet publish -c Release -o /publish

Runtime stage
FROM mcr.microsoft.com/dotnet/nightly:9.0 AS runtime

WORKDIR /app

COPY --from=build /publish .

Expose the application port
EXPOSE 8080

Start the application
CMD ["dotnet", "StudyFlow.API.dll"]