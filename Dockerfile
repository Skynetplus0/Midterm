FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Tüm repoyu kopyala
COPY . .

# Ana API'yi derle (Klasör yoluna dikkat et, Midterm/Midterm.csproj gibi)
RUN dotnet publish "Midterm/Midterm.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Port ayarı (Render genelde 10000 kullanır)
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "Midterm.dll"]