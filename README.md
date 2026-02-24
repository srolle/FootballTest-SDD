# FootballTest-SDD

Guía rápida para ejecutar la aplicación en entorno local (backend .NET + frontend React/Vite).

## Prerrequisitos

- Windows, macOS o Linux
- [.NET SDK 9.0](https://dotnet.microsoft.com/download)
- Node.js 20+ y npm
- Entity Framework Core CLI (`dotnet ef`)

Verifica instalación:

```powershell
dotnet --version
dotnet ef --version
node --version
npm --version
```

## 1) Restaurar dependencias

Desde la raíz del repo:

```powershell
dotnet restore .\backend\FootballTest.sln
Set-Location .\frontend
npm install
Set-Location ..
```

## 2) Configurar base de datos local (SQLite)

Aplicar migraciones existentes:

```powershell
dotnet ef database update --project .\backend\src\Infrastructure\Infrastructure.csproj --startup-project .\backend\src\Api\Api.csproj
```

Nota: la app usa `Data Source=league.db` (SQLite). El archivo se crea automáticamente al aplicar migraciones.

## 3) Ejecutar backend

```powershell
dotnet run --project .\backend\src\Api\Api.csproj --launch-profile https
```

URLs por defecto (launch profile):

- `https://localhost:7057`
- `http://localhost:5066`

Si es la primera vez con HTTPS en local:

```powershell
dotnet dev-certs https --trust
```

## 4) Ejecutar frontend

Crear `frontend/.env` para apuntar al backend:

```env
VITE_API_BASE_URL=https://localhost:7057
```

Levantar frontend:

```powershell
Set-Location .\frontend
npm run dev
```

Vite mostrará la URL local (normalmente `http://localhost:5173`).

## 5) Build y pruebas

Backend:

```powershell
dotnet build .\backend\FootballTest.sln
dotnet test .\backend\FootballTest.sln
```

Frontend:

```powershell
Set-Location .\frontend
npm run build
npm run test
```

## Problemas comunes

- Error de CORS o conexión frontend-backend: valida `VITE_API_BASE_URL` en `frontend/.env`.
- Certificado HTTPS no confiable: ejecuta `dotnet dev-certs https --trust`.
- Error de base de datos: vuelve a correr `dotnet ef database update`.
