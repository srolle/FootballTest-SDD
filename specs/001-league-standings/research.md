# Research: Football League Standings Management

## Decision 1: Arquitectura web separada (React + API REST .NET)
- Decision: Usar frontend React y backend ASP.NET Core Web API en soluciones separadas dentro del mismo repositorio.
- Rationale: Facilita separación de responsabilidades, pruebas independientes y evolución del dominio sin acoplar UI y reglas de negocio.
- Alternatives considered:
  - Full-stack en un único runtime: descartado por menor claridad de responsabilidades.
  - Server-rendering completo: descartado para mantener foco en API reutilizable y UI desacoplada.

## Decision 2: Persistencia con SQLite para MVP
- Decision: Usar SQLite como base de datos principal en esta fase.
- Rationale: Alineado con simplicidad de despliegue local y baja fricción para aprendizaje SDD.
- Alternatives considered:
  - PostgreSQL desde inicio: descartado por complejidad operativa inicial en un MVP de aprendizaje.
  - Persistencia en memoria: descartada por pérdida de trazabilidad y consistencia.

## Decision 3: Regla de puntuación centralizada en backend
- Decision: Implementar cálculo 3/1/0 y desempates en el backend como única fuente de verdad.
- Rationale: Evita inconsistencias entre clientes y asegura trazabilidad en recálculo al editar resultados.
- Alternatives considered:
  - Cálculo en frontend: descartado por riesgo de divergencia de reglas.
  - Cálculo híbrido frontend/backend: descartado por complejidad innecesaria.

## Decision 4: Estrategia de pruebas por riesgo
- Decision: Pruebas unitarias para reglas de dominio y pruebas de integración para flujos API + SQLite.
- Rationale: Cobertura efectiva en puntos de mayor riesgo (validaciones, cálculos, recálculos, ranking por jornada).
- Alternatives considered:
  - Solo unit tests: descartado por no validar persistencia y contratos reales.
  - Solo e2e: descartado por costo de mantenimiento para MVP.

## Decision 5: Trazabilidad mínima de cambios de resultados
- Decision: Registrar tipo de cambio y timestamp cuando se modifica resultado de partido finalizado.
- Rationale: Cumple requisito de trazabilidad y facilita auditoría operativa básica.
- Alternatives considered:
  - Sin historial: descartado por incumplir FR-012.
  - Historial completo con actor y diff detallado: pospuesto para iteración posterior.
