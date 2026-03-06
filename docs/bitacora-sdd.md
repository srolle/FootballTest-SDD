# Bitácora SDD (Spec Kit)

Proyecto: `FootballTest-SDD`  
Fecha de inicio: 24/02/2026  
Responsable: Equipo local (sesión guiada con Copilot)

## 1) Contexto de la iteración

- Objetivo de negocio: Definir y preparar de extremo a extremo el flujo SDD para una app web de gestión de liga (equipos, partidos, puntajes y ranking por jornada).
- Alcance de la iteración: Inicialización de Spec Kit, constitución del proyecto, especificación funcional, plan técnico, artefactos de diseño, tareas y análisis de consistencia.
- Fuera de alcance: Implementación de código productivo, despliegue y cierre de tareas de desarrollo.

## 2) Constitución

- Comando/prompt usado: `/speckit.constitution` (equivalente ejecutado mediante actualización directa del artefacto de constitución para esta sesión).
- Principios definidos:
	- Specification-First Development.
	- Requirements Clarity and Testability.
	- Quality Gates non-negotiable.
	- Incremental Delivery por User Story.
	- Traceability, Evidence and Learning.
- Restricciones técnicas: Validación de inputs obligatoria, trazabilidad mínima de cambios de resultados y simplicidad/mantenibilidad como criterio de diseño.
- Cambios respecto a versiones anteriores: Se creó versión inicial `1.0.0` (no existía constitución operativa previa).

## 3) Especificación

- Comando/prompt usado (`/speckit.specify`):
	- "Construir una app web para gestionar equipos, partidos y tabla de posiciones. Debe permitir registrar resultados, calcular puntos y mostrar ranking por jornada."
- Historias/casos principales:
	- US1: Registrar resultados de partidos (MVP).
	- US2: Calcular puntos automáticamente (3/1/0) y recálculo en cambios.
	- US3: Visualizar ranking por jornada con desempates.
- Criterios de aceptación: Definidos en formato Given/When/Then para cada historia.
- Ambigüedades detectadas: No quedaron marcadores `[NEEDS CLARIFICATION]` en el spec final.

## 4) Clarificación (opcional)

- Comando/prompt usado (`/speckit.clarify`): No ejecutado en esta iteración (spec quedó sin marcadores críticos).
- Preguntas clave respondidas: Se resolvieron por defecto razonable de dominio durante la especificación inicial.
- Decisiones tomadas: Regla de puntaje 3/1/0, desempate por DG y goles a favor, trazabilidad mínima de cambios de resultados.

## 5) Plan técnico

- Comando/prompt usado (`/speckit.plan`): Flujo equivalente ejecutado sobre artefactos de planning de Spec Kit.
- Arquitectura propuesta: Web app separada en frontend React + backend API REST .NET, con persistencia SQLite.
- Decisiones de stack:
	- UI/UX: React 19 + TypeScript.
	- Backend: ASP.NET Core Web API (.NET 9).
	- Base de datos: SQLite + EF Core.
	- Testing: xUnit (backend), Vitest + Testing Library (frontend).
- Riesgos técnicos y mitigación:
	- Riesgo de inconsistencia de reglas: cálculo centralizado en backend.
	- Riesgo de baja trazabilidad: `ResultChangeLog` obligatorio.
	- Riesgo de sobrecomplejidad: SQLite y arquitectura simple para MVP.

## 6) Tareas

- Comando/prompt usado (`/speckit.tasks`): Generación de `tasks.md` basada en `spec`, `plan`, `data-model` y contrato.
- Lista de tareas generada: 43 tareas (Setup, Foundational, US1, US2, US3, Polish).
- Dependencias críticas:
	- Phase 2 depende de Phase 1.
	- US1 depende de foundational.
	- US2 depende funcionalmente de US1.
	- US3 depende funcionalmente de US2.
- Ajustes manuales realizados: Organización por historias y formato estricto de checklist con IDs secuenciales `T001..T043`.

## 7) Análisis y checklist (opcional)

- Comando usado (`/speckit.analyze`): Revisión de consistencia cruzada ejecutada manualmente entre spec/plan/tasks/contracts.
- Hallazgos:
	- Brecha alta: `FR-001` (editar/desactivar equipos) no cubierto completamente en tareas/contrato.
	- Brecha media: cobertura parcial de NFR de performance en tasks.
	- Brecha baja: 2 tareas de polish sin ruta concreta explícita.
- Acciones correctivas aplicadas:
	- Se amplió contrato API con edición y desactivación de equipos.
	- Se actualizó tasks para cubrir FR-001 de forma explícita.
	- Se reemplazó tarea no mapeada por caso de uso alineado a consulta de standings.
	- Se agregó tarea explícita de verificación de performance para SC-003.
	- Se concretaron rutas de evidencia en tareas de pruebas y calidad.
- Comando usado (`/speckit.checklist`): Checklist de calidad de especificación generado en `specs/001-league-standings/checklists/requirements.md`.
- Brechas detectadas: Checklist de spec en verde; brechas pendientes están en alineación tasks/contract.

## 8) Implementación

- Comando/prompt usado (`/speckit.implement`): Implementación iniciada de forma manual (sin ejecutar comando formal de Spec Kit en esta sesión).
- Cambios de código relevantes:
	- Backend: `Api` configurada con `AddInfrastructure`, middleware global y pipeline base; entidades de dominio y `LeagueDbContext` con migración inicial; repositorios (`TeamRepository`, `MatchRepository`, `MatchdayRepository`) conectados por DI.
	- Backend (pendiente): carpeta `Api/Controllers` aún sin controladores implementados.
	- Frontend: enrutamiento base (`/`, `/results`, `/standings`) y servicios HTTP (`leagueApi`) para equipos, jornadas, partidos y actualización de resultados.
	- Frontend (estado UI): páginas en estado base/MVP con contenido inicial.
- Tests ejecutados y resultado:
	- Backend Unit + Integration (`UnitTest1` en ambos proyectos): 2/2 pruebas en verde.
	- Frontend: ejecución por archivo con runner no detectó pruebas; queda pendiente corrida formal de suite Vitest.
- Build/lint y resultado:
	- Backend: `dotnet build backend/FootballTest.sln` exitoso.
	- Frontend: `npm run build` (tsc + vite build) exitoso.

## 9) Evidencia

- Commits/PRs: Pendiente de registro por el equipo.
- Archivos de salida importantes:
	- `.specify/memory/constitution.md`
	- `specs/001-league-standings/spec.md`
	- `specs/001-league-standings/plan.md`
	- `specs/001-league-standings/research.md`
	- `specs/001-league-standings/data-model.md`
	- `specs/001-league-standings/contracts/standings-api.yaml`
	- `specs/001-league-standings/tasks.md`
	- `specs/001-league-standings/checklists/requirements.md`
- Capturas o referencias visuales: No aplica todavía.

## 10) Retrospectiva corta

- Qué funcionó bien: Flujo SDD completo hasta consistencia; artefactos claros, trazables y en orden.
- Qué no funcionó: Se detectó desalineación tardía entre requisitos y contrato/tareas para edición/desactivación de equipos.
- Qué mejorar en la próxima iteración: Corregir brechas de consistencia antes de iniciar implementación y luego ejecutar `/speckit.implement` por fases.
- Estado al cierre de esta actualización: brechas críticas de consistencia corregidas en contrato y tasks; listo para iniciar implementación.

## 11) Actualización operativa (24/02/2026)

- Consulta registrada: "¿por qué demoras tanto en analizar?".
- Causa de demora observada: tiempo dedicado a revisar contexto del workspace y validar consistencia entre artefactos antes de responder con cambios accionables.
- Acción aplicada: actualización inmediata de esta bitácora para dejar trazabilidad del estado y del motivo.
- Estado actual: Copilot operativo y listo para continuar con implementación y/o validaciones según prioridad del equipo.

## 12) Remediación de hallazgos (24/02/2026)

- Acción completada (CRITICAL 1): Se cuantificó `FR-011` en `spec.md` con formato obligatorio `application/problem+json`, campos mínimos (`status`, `title`, `traceId`, `errors`) y condición de no persistencia para solicitudes inválidas.
- Acción completada (CRITICAL 2): Se movieron quality gates al cierre de cada historia en `tasks.md` (US1, US2 y US3) para cumplir la constitución de calidad por incremento.
- Acción completada (HIGH): Se ajustaron `SC-001`, `SC-003`, `SC-004` y `SC-005` con métricas operables y ventana de medición explícita en `spec.md`.
- Acción completada (HIGH): Se agregaron tareas `T055`, `T056` y `T057` para evidencia de cumplimiento de `SC-001`, `SC-004` y `SC-005` en `tasks.md`.
- Acción completada (MEDIUM): Se limpió `plan.md` eliminando marcadores de plantilla `ACTION REQUIRED` para evitar ambigüedad de artefacto incompleto.
- Acción completada (MEDIUM): Se alinearon rutas de pruebas frontend en `tasks.md` con la estructura real (`frontend/src/test/`).
- Acción completada (LOW): Se ajustó `Phase 6` en `tasks.md` para explicitar consolidación de evidencia (sin duplicar ejecuciones ya cerradas por quality gates de historia, salvo cambios nuevos).
- Estado: CRITICAL, HIGH, MEDIUM y LOW resueltos en artefactos de especificación y planificación de ejecución.

## 13) Validación final de consistencia (24/02/2026)

- Comando usado (`/speckit.analyze`): pasada final posterior a remediaciones en `spec.md`, `plan.md` y `tasks.md`.
- Resultado: sin hallazgos `CRITICAL` ni `HIGH`; cobertura de requisitos reportada al 100% con tareas asociadas.
- Nota de mejora menor: solapamiento residual controlado entre quality gates por historia y consolidación de evidencia en `Phase 6` (ya documentado como consolidación, no re-ejecución).
- Estado de preparación: artefactos SDD listos para continuar con implementación incremental iniciando por US1 (`T013`, `T020`–`T023`).

## 14) Avance de implementación US1 (24/02/2026)

- Tareas completadas: `T013`, `T017`, `T018`, `T020`, `T021`, `T022`, `T023`, `T024`, `T025`, `T026`.
- Backend implementado:
	- `GlobalExceptionMiddleware` actualizado para responder `application/problem+json` con `traceId` y errores de validación por campo.
	- Excepción de validación estructurada `RequestValidationException` para errores de dominio.
	- `TeamsController` con endpoints `POST /teams`, `GET /teams`, `PUT /teams/{teamId}`, `PATCH /teams/{teamId}/deactivate`.
	- `MatchdaysController` con endpoint `POST /matchdays`.
	- `MatchesController` con endpoints `POST /matches` y `PUT /matches/{matchId}/result` con validaciones de existencia/estado.
	- Caso de uso `UpdateMatchResultUseCase` implementado con registro de trazabilidad (`ResultChangeLog`) por alta/edición de resultado.
	- DTO agregado: `UpdateTeamRequest`.
- Validación técnica:
	- `dotnet build backend/FootballTest.sln` en verde.
	- `dotnet test backend/FootballTest.sln` en verde (4 tests OK, incluyendo `ResultValidationTests` y `MatchesResultEndpointTests`).
	- `npm run build` en frontend en verde tras implementar formulario y servicio de US1.
- Frontend implementado:
	- `ResultsPage` con flujo mínimo completo de US1: crear equipo, crear jornada, crear partido y registrar resultado final.
	- `leagueApi` ampliado para cubrir endpoints de equipos/jornadas/partidos/resultados, incluyendo actualización y desactivación de equipos.
- Quality gate US1 completado:
	- `T045`: pruebas US1 ejecutadas en verde (backend + frontend).
	- `T046`: verificación de contrato OpenAPI para endpoints US1 documentada.
	- `T047`: lint scoped frontend ejecutado en verde y evidenciado.
	- Evidencia consolidada en `specs/001-league-standings/checklists/us1-quality-gate.md`.
- Estado de US1: implementado y validado en backend/frontend para flujo MVP de registro de resultados.

## 15) Actualización operativa y plan corto US2 (2026-03-06 15:26:13 -03:00)

- Solicitud atendida: actualización de bitácora con fecha/hora y continuación con el punto 2 (plan de ejecución de US2).
- Estado consolidado al momento:
	- Artefactos SDD (`spec`, `plan`, `tasks`, `research`, `data-model`, `contract`) vigentes y consistentes para continuar implementación.
	- US1 cerrada con quality gate en verde.
	- US2 y US3 pendientes de implementación y validación.

### Plan corto de ejecución US2 (orden recomendado)

1. `T027` Definir y dejar en rojo pruebas unitarias de regla 3/1/0 y estadísticas por equipo.
2. `T029` Implementar `StandingsCalculator` en dominio para cálculo determinístico (puntos, PJ, G, E, P, GF, GC, DG).
3. `T031` Implementar persistencia de snapshots `StandingEntry` por jornada.
4. `T030` Implementar `RecalculateStandingsUseCase` al editar resultado finalizado.
5. `T032` Implementar `GetStandingsByMatchdayUseCase` para consulta por jornada.
6. `T028` Ejecutar y ajustar prueba de integración de recálculo tras edición de resultado.
7. `T048` Correr quality gate de pruebas US2 y registrar evidencia.
8. `T049` Verificar endpoints impactados por recálculo contra OpenAPI y registrar resultado.
9. `T050` Ejecutar lint/format/build de cambios US2 y registrar evidencia.

### Criterios de validación US2 (operativos)

- Regla 3/1/0 exacta para victoria/empate/derrota en todos los casos de prueba.
- Recalculo correcto de tabla al modificar un resultado finalizado (sin inconsistencias acumuladas).
- Snapshot por jornada persistido y consultable sin duplicidades por equipo/jornada.
- Sin regresión de US1 (actualización de resultado mantiene trazabilidad y validaciones existentes).
- Evidencia mínima registrada en checklist US2 correspondiente.

## 16) Avance de implementación US2 - hito inicial (2026-03-06 15:31:27 -03:00)

- Tareas ejecutadas en este hito:
	- `T027` completada: pruebas unitarias de cálculo de standings agregadas en `backend/tests/Unit/StandingsCalculationTests.cs`.
	- `T029` completada: servicio de dominio `StandingsCalculator` implementado en `backend/src/Domain/Services/StandingsCalculator.cs`.
- Cobertura funcional incorporada en pruebas:
	- Regla 3 puntos por victoria.
	- Regla 1 punto por empate.
	- Ordenamiento por criterios de desempate (Points, GoalDifference, GoalsFor) y asignación de posición.
- Validación técnica ejecutada:
	- `dotnet test .\\backend\\tests\\Unit\\Unit.csproj` -> **PASS** (6 tests OK, 0 fallos).
	- `dotnet build .\\backend\\FootballTest.sln` -> **PASS**.
- Estado US2 tras este hito:
	- Avance backend de cálculo base listo para integrar persistencia de snapshots (`T031`) y recálculo por cambio de resultado (`T030`).

## 17) Avance de implementación US2 - persistencia y recálculo (2026-03-06 15:34:29 -03:00)

- Tareas completadas en este hito:
	- `T028` prueba de integración de recálculo: `backend/tests/Integration/StandingsRecalculationTests.cs`.
	- `T030` caso de uso de recálculo: `backend/src/Application/UseCases/RecalculateStandingsUseCase.cs`.
	- `T031` persistencia de snapshots: `backend/src/Infrastructure/Repositories/StandingEntryRepository.cs`.
	- `T032` consulta por jornada: `backend/src/Application/UseCases/GetStandingsByMatchdayUseCase.cs`.
- Soporte técnico agregado para US2:
	- Contratos extendidos: `IMatchRepository`, `IMatchdayRepository`, `IStandingEntryRepository`.
	- Implementaciones extendidas: `MatchRepository` (query de partidos finalizados hasta jornada), `MatchdayRepository` (búsqueda por número), DI en `ServiceCollectionExtensions`.
	- Integración funcional: `UpdateMatchResultUseCase` ahora dispara recálculo de standings tras registrar/editar resultado.
- Evidencia de validación:
	- `dotnet test .\\backend\\FootballTest.sln` -> **PASS** (8 tests OK, 0 fallos).
	- `get_errors` sobre proyectos backend -> sin errores.
- Estado US2 tras este hito:
	- Núcleo backend implementado para cálculo, persistencia y recálculo de tabla por jornada.
	- Pendiente quality gate US2 (`T048`, `T049`, `T050`) y exposición de endpoint de standings en US3.

## 18) Cierre quality gate US2 (2026-03-06 15:35:42 -03:00)

- Tareas cerradas:
	- `T048` pruebas US2 ejecutadas y evidenciadas.
	- `T049` verificación de contrato OpenAPI para endpoint impactado por recálculo.
	- `T050` validación técnica scoped a cambios US2.
- Evidencia registrada en:
	- `specs/001-league-standings/checklists/us2-quality-gate.md`.
- Resumen de resultados:
	- Unit US2: 3/3 pruebas en verde.
	- Integration US2: 1/1 prueba en verde.
	- Build backend: exitoso.
	- `get_errors` en backend/tests: sin errores.
- Estado de ejecución:
	- US2 implementada y validada en backend.
	- Siguiente bloque natural: US3 (`T033` a `T038`, luego `T051` a `T054`).

## 19) Avance de implementación US3 (2026-03-06 15:45:53 -03:00)

- Tareas completadas en este hito:
	- `T033` prueba de integración de consulta de standings: `backend/tests/Integration/StandingsQueryTests.cs`.
	- `T034` prueba de render frontend de standings: `frontend/src/test/standings-page.test.tsx`.
	- `T035` endpoint `GET /standings?matchdayNumber=`: `backend/src/Api/Controllers/StandingsController.cs`.
	- `T036` desempate por `Points`, `GoalDifference`, `GoalsFor` validado en `StandingsCalculator`.
	- `T037` página de ranking por jornada con selector: `frontend/src/pages/StandingsPage.tsx`.
	- `T038` componente reutilizable de tabla: `frontend/src/components/StandingsTable.tsx`.
- Cambios de soporte incorporados:
	- DTO de respuesta de standings con nombre de equipo: `backend/src/Application/DTOs/StandingEntryResponse.cs`.
	- Extensión de validadores para query de standings: `ValidateGetStandings`.
	- `leagueApi` y tipos frontend actualizados para consumir `/standings`.
- Evidencia técnica ejecutada:
	- `dotnet test .\\backend\\tests\\Integration\\Integration.csproj --filter "FullyQualifiedName~StandingsQueryTests"` -> **PASS** (1 test OK).
	- `npm run test -- src/test/standings-page.test.tsx` -> **PASS** (1 test OK).
	- `npm run build` (frontend) -> **PASS**.
	- `get_errors` en archivos modificados -> sin errores.
- Estado US3:
	- Implementación funcional backend + frontend completada.
	- Pendiente cierre de quality gate US3 (`T051`, `T052`, `T053`, `T054`).

## 20) Cierre quality gate US3 (2026-03-06 15:49:48 -03:00)

- Tareas cerradas:
	- `T051` pruebas US3 backend/frontend ejecutadas y evidenciadas.
	- `T052` verificación de contrato OpenAPI para `GET /standings?matchdayNumber=`.
	- `T053` prueba de rendimiento de standings ejecutada (criterio p90 <= 2000ms validado).
	- `T054` lint/format/build scoped a cambios US3.
- Evidencia registrada en:
	- `specs/001-league-standings/checklists/us3-quality-gate.md`.
- Resumen de resultados:
	- Integración backend US3: 1/1 prueba en verde (`StandingsQueryTests`).
	- Performance backend US3: prueba en verde (`StandingsPerformanceTests`).
	- Frontend US3: 1/1 prueba en verde (`standings-page.test.tsx`).
	- Lint frontend scoped: en verde.
	- Build backend + frontend: en verde.
- Estado de ejecución:
	- US3 implementada y validada.
	- Próximo bloque natural: consolidación final de Phase 6 (`T039`-`T044`, `T055`-`T057`).

## 21) Validación integral post-US3 (2026-03-06 15:51:24 -03:00)

- Ejecución de suites completas para control de regresión:
	- `dotnet test .\\backend\\FootballTest.sln` -> **PASS** (total 10, failed 0, succeeded 10).
	- `npm run test` en frontend -> **PASS** (3 archivos de prueba, 3 pruebas en verde).
- Resultado:
	- Sin regresiones detectadas en US1, US2 y US3 tras integrar endpoint y UI de standings.
	- Estado de calidad mantenido para continuar con consolidación de evidencias de Phase 6.
