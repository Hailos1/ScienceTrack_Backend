# Используем базовый образ ASP.NET Core SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Копируем проект и восстанавливаем зависимости
COPY ScienceTrack/ScienceTrack.csproj .
RUN dotnet resotre

# Копируем все файлы и собираем приложение
COPY . .
RUN dotnet publish -c Release -o out

# Используем базовый образ ASP.NET Core для запуска приложения
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Указываем порт, на котором будет работать приложение
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80

# Запускаем приложение при старте контейнера
ENTRYPOINT ["dotnet", "ScienceTrack.dll"]
