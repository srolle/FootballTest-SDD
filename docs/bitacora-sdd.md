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

- Comando/prompt usado (`/speckit.implement`): No iniciado aún.
- Cambios de código relevantes: No aplica en esta iteración (solo artefactos SDD).
- Tests ejecutados y resultado: No aplica aún.
- Build/lint y resultado: No aplica aún.

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
