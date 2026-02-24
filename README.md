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

### Opción recomendada para demo local (HTTP en puerto fijo)

```powershell
$env:ASPNETCORE_URLS='http://localhost:5080'
dotnet run --project .\backend\src\Api\Api.csproj --no-launch-profile
```

URL esperada:

- `http://localhost:5080`

### Opción con launch profile (HTTP/HTTPS)

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

Para demo rápida, ejecutar con variable en sesión (sin crear archivo `.env`):

```powershell
Set-Location .\frontend
$env:VITE_API_BASE_URL='http://localhost:5080'
npm run dev -- --host
```

Alternativa persistente: crear `frontend/.env` para apuntar al backend:

```env
VITE_API_BASE_URL=http://localhost:5080
```

Luego levantar frontend:

```powershell
Set-Location .\frontend
npm run dev
```

Vite mostrará la URL local (normalmente `http://localhost:5173`).

## 5) Demo guiada (US1)

Con backend y frontend levantados, ir a `http://localhost:5173/results` y ejecutar:

1. **Crear equipo**
	- Nombre: `Barcelona`
	- Click en `Crear equipo`
2. **Crear equipo**
	- Nombre: `Real Madrid`
	- Click en `Crear equipo`
3. **Crear jornada**
	- Número: `1`
	- Click en `Crear jornada`
4. **Crear partido**
	- Local: `Barcelona`
	- Visitante: `Real Madrid`
	- Click en `Crear partido`
5. **Registrar resultado**
	- Se autocompleta `Match ID` con el último partido creado
	- Goles local: `2`
	- Goles visitante: `1`
	- Click en `Guardar resultado`

Resultado esperado: mensajes de éxito en la UI y persistencia en SQLite (`league.db`).

## 6) Build y pruebas

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
- Puerto API ocupado (`5066/7057/5080`): cierra el proceso previo o usa otro puerto en `ASPNETCORE_URLS`.
