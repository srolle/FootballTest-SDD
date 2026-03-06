# US3 Quality Gate Evidence

Fecha: 2026-03-06 15:49:48 -03:00  
Historia: US3 - Ver ranking por jornada

## T051 - Ejecucion de pruebas US3

- Backend integracion (T033): `dotnet test .\\backend\\tests\\Integration\\Integration.csproj --filter "FullyQualifiedName~StandingsQueryTests"`.
  - Resultado: **PASS**.
  - Resumen: total 1, failed 0, succeeded 1, skipped 0.
- Frontend render (T034): `npm run test -- src/test/standings-page.test.tsx`.
  - Resultado: **PASS**.
  - Resumen: 1 test file, 1 test passed.

## T052 - Verificacion contrato OpenAPI US3

Endpoint verificado contra `contracts/standings-api.yaml`:

- `GET /standings?matchdayNumber=` -> implementado en `StandingsController`.

Notas de validacion:

- Se valida `matchdayNumber > 0` y se devuelve error `application/problem+json` para query invalida.
- La respuesta incluye campos de tabla requeridos por contrato (`teamId`, `teamName`, `played`, `won`, `drawn`, `lost`, `goalsFor`, `goalsAgainst`, `goalDifference`, `points`, `position`).

## T053 - Prueba de performance de standings

- Prueba ejecutada: `dotnet test .\\backend\\tests\\Integration\\Integration.csproj --filter "FullyQualifiedName~StandingsPerformanceTests"`.
- Resultado: **PASS**.
- Criterio validado en prueba: `p90 <= 2000ms` para `GET /standings?matchdayNumber=1` sobre muestra de 30 requests (con warm-up previo).

## T054 - Lint/format scoped a cambios US3

- Frontend lint scoped:
  - `npm run lint -- src/pages/StandingsPage.tsx src/components/StandingsTable.tsx src/services/leagueApi.ts src/types/api.ts src/test/standings-page.test.tsx`
  - Resultado: **PASS**.
- Build backend: `dotnet build .\\backend\\FootballTest.sln` -> **PASS**.
- Build frontend: `npm run build` -> **PASS**.

## Resultado de Quality Gate US3

Estado: **PASS**
