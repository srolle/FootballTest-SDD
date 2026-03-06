---

description: "Task list for Football League Standings Management"
---

# Tasks: Football League Standings Management

**Input**: Design documents from `/specs/001-league-standings/`  
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/standings-api.yaml

**Tests**: Se incluyen tareas de pruebas por la estrategia de calidad definida en el plan.

**Organization**: Tareas agrupadas por historia de usuario para permitir entrega incremental y validación independiente.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Puede ejecutarse en paralelo (archivos distintos, sin dependencia directa)
- **[Story]**: Historia de usuario (US1, US2, US3)
- Todas las tareas incluyen rutas explícitas

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Inicializar solución backend/frontend y configuración base.

- [X] T001 Crear solución .NET y proyecto API en backend/src/Api/
- [X] T002 Crear proyecto frontend React + TypeScript en frontend/
- [X] T003 [P] Configurar estructura de carpetas backend (Domain/Application/Infrastructure) en backend/src/
- [X] T004 [P] Configurar estructura de frontend (pages/components/services/hooks/types/state) en frontend/src/
- [X] T005 Configurar conexión SQLite y cadena de conexión base en backend/src/Infrastructure/Persistence/
- [ ] T006 [P] Configurar linters y formato para backend y frontend en backend/ y frontend/
- [X] T007 [P] Configurar proyectos de pruebas backend Unit/Integration en backend/tests/
- [X] T008 [P] Configurar Vitest + Testing Library en frontend/src/test/

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Base de dominio, persistencia y pipeline HTTP antes de historias.

- [X] T009 Crear entidades Team, Matchday, Match, StandingEntry y ResultChangeLog en backend/src/Domain/Entities/
- [X] T010 Crear DbContext y mapeos EF Core para SQLite en backend/src/Infrastructure/Persistence/
- [X] T011 Crear migración inicial y script de creación de base en backend/src/Infrastructure/Persistence/Migrations/
- [X] T012 [P] Implementar validaciones comunes (nombre único, goles >= 0, equipos distintos) en backend/src/Api/Validators/
- [X] T013 [P] Implementar manejo global de errores con `ProblemDetails` (`application/problem+json`) y respuestas 400 consistentes en backend/src/Api/
- [X] T014 [P] Implementar repositorios base para Team/Matchday/Match en backend/src/Infrastructure/Repositories/
- [X] T015 [P] Implementar cliente HTTP base y tipado compartido de API en frontend/src/services/ y frontend/src/types/
- [X] T016 Configurar enrutamiento base y layout de aplicación en frontend/src/

**Checkpoint**: Infraestructura lista para iniciar historias de usuario.

---

## Phase 3: User Story 1 - Registrar resultados de partidos (Priority: P1) 🎯 MVP

**Goal**: Permitir registrar y actualizar resultados válidos de partidos.

**Independent Test**: Crear equipos/jornada/partido, cargar resultado y verificar persistencia y validación.

### Tests for User Story 1

- [X] T017 [P] [US1] Crear pruebas unitarias de validación de resultado en backend/tests/Unit/ResultValidationTests.cs
- [X] T018 [P] [US1] Crear prueba de integración de endpoint PUT /matches/{matchId}/result en backend/tests/Integration/MatchesResultEndpointTests.cs
- [X] T019 [P] [US1] Crear prueba de formulario de carga de resultado en frontend/src/test/result-form.test.tsx

### Implementation for User Story 1

- [X] T020 [US1] Implementar endpoints POST /teams, GET /teams, PUT /teams/{teamId} y PATCH /teams/{teamId}/deactivate en backend/src/Api/Controllers/TeamsController.cs
- [X] T021 [US1] Implementar endpoint POST /matchdays en backend/src/Api/Controllers/MatchdaysController.cs
- [X] T022 [US1] Implementar endpoint POST /matches en backend/src/Api/Controllers/MatchesController.cs
- [X] T023 [US1] Implementar endpoint PUT /matches/{matchId}/result con validaciones en backend/src/Api/Controllers/MatchesController.cs
- [X] T024 [US1] Implementar registro de trazabilidad de cambios de resultado en backend/src/Application/UseCases/UpdateMatchResultUseCase.cs
- [X] T025 [P] [US1] Implementar página/formulario de registro de resultados en frontend/src/pages/ResultsPage.tsx
- [X] T026 [P] [US1] Implementar servicio frontend para equipos, jornadas, partidos y resultado en frontend/src/services/leagueApi.ts

### Quality Gate for User Story 1

- [X] T045 [US1] Ejecutar pruebas de US1 (T017, T018, T019) y registrar evidencia en specs/001-league-standings/checklists/us1-quality-gate.md
- [X] T046 [US1] Verificar contrato OpenAPI de endpoints US1 (`/teams`, `/matchdays`, `/matches`, `/matches/{matchId}/result`) y registrar resultado en specs/001-league-standings/checklists/us1-quality-gate.md
- [X] T047 [US1] Ejecutar lint/format scoped a cambios de US1 y registrar estado en specs/001-league-standings/checklists/us1-quality-gate.md

**Checkpoint**: Usuario puede registrar resultados con validación y trazabilidad básica.

---

## Phase 4: User Story 2 - Calcular puntos automáticamente (Priority: P2)

**Goal**: Calcular y recalcular puntos y estadísticas por resultados finalizados.

**Independent Test**: Con varios resultados, verificar totales por equipo y recálculo tras edición.

### Tests for User Story 2

- [X] T027 [P] [US2] Crear pruebas unitarias de regla 3/1/0 y estadísticas en backend/tests/Unit/StandingsCalculationTests.cs
- [X] T028 [P] [US2] Crear prueba de integración de recálculo tras editar resultado en backend/tests/Integration/StandingsRecalculationTests.cs

### Implementation for User Story 2

- [X] T029 [US2] Implementar servicio de cálculo de standings por jornada en backend/src/Domain/Services/StandingsCalculator.cs
- [X] T030 [US2] Implementar caso de uso de recálculo cuando cambia resultado final en backend/src/Application/UseCases/RecalculateStandingsUseCase.cs
- [X] T031 [US2] Persistir snapshots StandingEntry por jornada en backend/src/Infrastructure/Repositories/StandingEntryRepository.cs
- [X] T032 [US2] Implementar caso de uso de consulta de standings por jornada en backend/src/Application/UseCases/GetStandingsByMatchdayUseCase.cs

### Quality Gate for User Story 2

- [X] T048 [US2] Ejecutar pruebas de US2 (T027, T028) y registrar evidencia en specs/001-league-standings/checklists/us2-quality-gate.md
- [X] T049 [US2] Verificar contrato OpenAPI de endpoints afectados por recálculo y registrar resultado en specs/001-league-standings/checklists/us2-quality-gate.md
- [X] T050 [US2] Ejecutar lint/format scoped a cambios de US2 y registrar estado en specs/001-league-standings/checklists/us2-quality-gate.md

**Checkpoint**: Puntos y estadísticas se calculan y recalculan automáticamente de forma consistente.

---

## Phase 5: User Story 3 - Ver ranking por jornada (Priority: P3)

**Goal**: Visualizar tabla ordenada por jornada con criterios de desempate.

**Independent Test**: Consultar dos jornadas y validar orden por puntos, diferencia y goles a favor.

### Tests for User Story 3

- [ ] T033 [P] [US3] Crear prueba de integración GET /standings?matchdayNumber= en backend/tests/Integration/StandingsQueryTests.cs
- [ ] T034 [P] [US3] Crear prueba de render de tabla por jornada en frontend/src/test/standings-page.test.tsx

### Implementation for User Story 3

- [ ] T035 [US3] Implementar endpoint GET /standings?matchdayNumber= en backend/src/Api/Controllers/StandingsController.cs
- [ ] T036 [US3] Aplicar ordenamiento de desempate (Points, GoalDifference, GoalsFor) en backend/src/Domain/Services/StandingsCalculator.cs
- [ ] T037 [P] [US3] Implementar página de ranking por jornada con selector en frontend/src/pages/StandingsPage.tsx
- [ ] T038 [P] [US3] Implementar componente reutilizable de tabla de posiciones en frontend/src/components/StandingsTable.tsx

### Quality Gate for User Story 3

- [ ] T051 [US3] Ejecutar pruebas de US3 (T033, T034) y registrar evidencia en specs/001-league-standings/checklists/us3-quality-gate.md
- [ ] T052 [US3] Verificar contrato OpenAPI de endpoint `/standings?matchdayNumber=` y registrar resultado en specs/001-league-standings/checklists/us3-quality-gate.md
- [ ] T053 [US3] Ejecutar prueba de rendimiento de GET `/standings?matchdayNumber=` y registrar evidencia de SC-003 en specs/001-league-standings/checklists/us3-quality-gate.md
- [ ] T054 [US3] Ejecutar lint/format scoped a cambios de US3 y registrar estado en specs/001-league-standings/checklists/us3-quality-gate.md

**Checkpoint**: Ranking por jornada visible y coherente con reglas del dominio.

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Consolidación final de documentación y evidencia global (con quality gates ya ejecutados por historia).

- [ ] T039 [P] Documentar setup y ejecución local en README.md
- [ ] T040 [P] Actualizar docs de flujo SDD y evidencia en docs/bitacora-sdd.md
- [ ] T041 Consolidar resultados de pruebas backend/frontend (US1-US3) y registrar resumen final en specs/001-league-standings/checklists/test-execution.md (sin re-ejecutar pruebas ya cerradas en quality gates, salvo cambios nuevos)
- [ ] T042 Consolidar estado de lint/format de historias (US1-US3) en specs/001-league-standings/checklists/code-quality.md (sin re-ejecutar validaciones ya cerradas, salvo cambios nuevos)
- [ ] T043 Consolidar verificación de contrato OpenAPI por historia en un reporte final en specs/001-league-standings/contracts/standings-api.yaml y specs/001-league-standings/checklists/code-quality.md
- [ ] T044 Consolidar evidencia final de performance y calidad de historias en specs/001-league-standings/checklists/performance.md (incluyendo resultados de T053/T055/T056/T057)
- [ ] T055 [P] Ejecutar medición de SC-001 (>=50 registros de resultados) y registrar tiempos/evidencia en specs/001-league-standings/checklists/sc-001-execution.md
- [ ] T056 [P] Ejecutar prueba operativa de SC-004 (>=10 ejecuciones de flujo end-to-end) y registrar resultados en specs/001-league-standings/checklists/sc-004-ux-execution.md
- [ ] T057 [P] Definir baseline de incidencias manuales y plan de seguimiento de 4 semanas para SC-005 en specs/001-league-standings/checklists/sc-005-baseline-and-followup.md

---

## Dependencies & Execution Order

### Phase Dependencies

- Phase 1 → sin dependencias.
- Phase 2 → depende de Phase 1.
- Phase 3, 4 y 5 → dependen de Phase 2.
- Phase 6 → depende de finalizar historias a entregar.

### User Story Dependencies

- US1 (P1) es el MVP inicial.
- US2 (P2) depende funcionalmente de US1 (requiere resultados cargados).
- US3 (P3) depende de US2 para ranking consistente por jornada.

### Parallel Opportunities

- T003, T004, T006, T007, T008 en paralelo dentro de setup.
- T012, T013, T014, T015 en paralelo en foundational.
- Pruebas y frontend de cada historia pueden correr en paralelo con tareas backend no bloqueantes.

---

## Parallel Example: User Story 1

- Ejecutar en paralelo:
  - T017, T018, T019
  - T025, T026
- Ejecutar secuencial:
  - T020 → T021 → T022 → T023 → T024

---

## Implementation Strategy

### MVP First

1. Completar Phase 1 y 2.
2. Completar US1 (Phase 3).
3. Validar flujo principal de registro de resultados.

### Incremental Delivery

1. Añadir US2 para cálculo/re-cálculo.
2. Añadir US3 para visualización y consulta por jornada.
3. Cerrar con Polish y evidencia en bitácora.

### Notes

- Todas las tareas siguen formato checklist ejecutable.
- Mantener trazabilidad a requisitos FR-001..FR-012 y criterios SC-001..SC-005.
- Marcar tareas completadas como [X] conforme avance implementación.
