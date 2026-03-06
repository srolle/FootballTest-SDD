@echo off
REM Batch script to launch backend and frontend for local demo.
REM Usage: double-click or run from repository root.

echo Starting backend in a new window...
start "Backend" cmd /k "set ASPNETCORE_URLS=http://localhost:5000 && dotnet run --project backend\src\Api\Api.csproj --no-launch-profile"

echo Waiting a moment before launching frontend...
timeout /t 2 /nobreak >nul

echo Starting frontend in a new window...
start "Frontend" cmd /k "cd frontend && set VITE_API_BASE_URL=http://localhost:5000 && npm run dev -- --host"

echo Demo launch commands executed. Close these windows to stop servers.
pause