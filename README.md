# 📦 Prueba Técnica - Backend (.NET 6)

Este proyecto es una API construida con **ASP.NET Core 6** que descarga archivos Excel desde el sitio web de CENACE utilizando **Selenium**, los almacena en una carpeta local, inserta en db y expone endpoints para su procesamiento.

---

## 🚀 Tecnologías

- ASP.NET Core 6
- Selenium WebDriver
- ChromeDriver
- C#
- Swagger
- PostgreSQL

---

## ⚙️ Requisitos

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- Google Chrome
- [ChromeDriver](https://sites.google.com/chromium.org/driver/) compatible con tu versión de Chrome
- Visual Studio 
- PostgreSQL

---

## 🛠️ Configuración

1. Clona el repositorio:
   git clone https://github.com/AlexandroFP/Prueba-Backend.git
   
   cd Prueba-Backend/Cenace.API
   
2.Crea tu base de datos local y configura tu conexión en appsettings.json, por ejemplo:
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=CenaceDb;Username=postgres;Password=tupassword"
}

3.Ejecuta el proyecto:
dotnet run

4.Navega
http://localhost:5052/swagger/index.html
