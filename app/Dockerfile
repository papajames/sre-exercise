FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

COPY SreExercise/*.sln .
COPY SreExercise/SreExercise.Web/*.csproj ./SreExercise.Web/
RUN dotnet restore

COPY SreExercise/SreExercise.Web/. ./SreExercise.Web/
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "SreExercise.Web.dll"]