# مرحلة البناء
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# نسخ ملف المشروع أولاً واستعادة الحزم
COPY *.csproj ./
RUN dotnet restore

# نسخ باقي ملفات المشروع ونشرها
COPY . ./
RUN dotnet publish -c Release -o out

# مرحلة التشغيل
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "SharjahEventsWeb.dll"]