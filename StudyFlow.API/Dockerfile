# Use official .NET 9 SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy and restore dependencies
COPY StudyFlow.API.csproj ./
RUN dotnet restore

# Copy the rest of the project files
COPY . ./
RUN dotnet publish -c Release -o /out

# Use a lightweight .NET 9 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /out .

# Expose the port your API runs on (usually 5000 or 8080)
EXPOSE 5000

# Run the API
ENTRYPOINT ["dotnet", "StudyFlow.API.dll"]