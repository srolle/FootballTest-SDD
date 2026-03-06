# US2 Quality Gate Evidence

Fecha: 2026-03-06 15:35:42 -03:00  
Historia: US2 - Calcular puntos automaticamente

## T048 - Ejecucion de pruebas US2

- Unitario US2 (T027): `dotnet test .\\backend\\tests\\Unit\\Unit.csproj --filter "FullyQualifiedName~StandingsCalculationTests"`.
  - Resultado: **PASS**.
  - Resumen: total 3, failed 0, succeeded 3, skipped 0.
- Integracion US2 (T028): `dotnet test .\\backend\\tests\\Integration\\Integration.csproj --filter "FullyQualifiedName~StandingsRecalculationTests"`.
  - Resultado: **PASS**.
  - Resumen: total 1, failed 0, succeeded 1, skipped 0.

## T049 - Verificacion contrato OpenAPI de recálculo

Endpoints impactados por recálculo verificados contra `contracts/standings-api.yaml`:

- `PUT /matches/{matchId}/result` -> implementado en `MatchesController` y conectado a `UpdateMatchResultUseCase` con recálculo de standings.

Notas de validacion:

- US2 no introduce cambios de contrato en request/response del endpoint de resultado; el recálculo ocurre como comportamiento interno.
- El endpoint `GET /standings?matchdayNumber=` permanece definido en contrato y se implementara en US3.

## T050 - Lint/format scoped a cambios US2

- Backend no expone comando de lint C# en scripts del repositorio.
- Validacion tecnica aplicada para cambios US2:
  - `dotnet build .\\backend\\FootballTest.sln` -> **PASS**.
  - `dotnet test .\\backend\\FootballTest.sln` -> **PASS** (8 tests, 0 fallos).
  - `get_errors` en proyectos backend/tests -> sin errores.

## Resultado de Quality Gate US2

Estado: **PASS**
