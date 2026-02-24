# Feature Specification: Football League Standings Management

**Feature Branch**: `001-league-standings`  
**Created**: 2026-02-24  
**Status**: Draft  
**Input**: User description: "Construir una app web para gestionar equipos, partidos y tabla de posiciones. Debe permitir registrar resultados, calcular puntos y mostrar ranking por jornada."

## User Scenarios & Testing *(mandatory)*

<!--
  IMPORTANT: User stories should be PRIORITIZED as user journeys ordered by importance.
  Each user story/journey must be INDEPENDENTLY TESTABLE - meaning if you implement just ONE of them,
  you should still have a viable MVP (Minimum Viable Product) that delivers value.
  
  Assign priorities (P1, P2, P3, etc.) to each story, where P1 is the most critical.
  Think of each story as a standalone slice of functionality that can be:
  - Developed independently
  - Tested independently
  - Deployed independently
  - Demonstrated to users independently
-->

### User Story 1 - Registrar resultados de partidos (Priority: P1)

Como usuario que administra una liga, quiero registrar el resultado de cada partido para que la competencia tenga datos oficiales y trazables.

**Why this priority**: Sin resultados confiables no se puede calcular ningún ranking ni mostrar avance por jornada.

**Independent Test**: Puede probarse creando equipos, registrando un partido con marcador final y verificando que el sistema guarda resultado, jornada y estado del partido.

**Acceptance Scenarios**:

1. **Given** equipos válidos y un partido pendiente en una jornada, **When** el usuario registra un marcador final válido, **Then** el partido queda marcado como finalizado con su resultado persistido.
2. **Given** un partido ya finalizado, **When** el usuario intenta registrar un resultado inválido (por ejemplo, marcador negativo), **Then** el sistema rechaza la operación con un mensaje claro y no altera datos existentes.

---

### User Story 2 - Calcular puntos automáticamente (Priority: P2)

Como usuario de la liga, quiero que los puntos se calculen automáticamente a partir de resultados para evitar errores manuales.

**Why this priority**: Asegura consistencia y confianza en la clasificación, reduciendo esfuerzo operativo.

**Independent Test**: Puede probarse con una jornada completa donde existan victorias, empates y derrotas, verificando puntos acumulados por equipo según reglas definidas.

**Acceptance Scenarios**:

1. **Given** un partido finalizado, **When** el sistema procesa el resultado, **Then** asigna puntos al equipo ganador/perdedor o a ambos en caso de empate según reglas de puntuación de la liga.
2. **Given** múltiples partidos de un mismo equipo en distintas jornadas, **When** se consulta su resumen, **Then** el total de puntos coincide con la suma de resultados válidos registrados.

---

### User Story 3 - Ver ranking por jornada (Priority: P3)

Como usuario final, quiero ver la tabla de posiciones por jornada para entender el rendimiento y evolución de los equipos.

**Why this priority**: Entrega el valor visible principal para usuarios de negocio y audiencia de la competición.

**Independent Test**: Puede probarse consultando la tabla en dos jornadas distintas y validando orden, puntos y desempates definidos.

**Acceptance Scenarios**:

1. **Given** resultados registrados en una jornada, **When** el usuario visualiza la tabla de esa jornada, **Then** ve equipos ordenados por puntaje con criterios de desempate aplicados.
2. **Given** varias jornadas con resultados, **When** el usuario cambia de jornada en la vista de ranking, **Then** la tabla refleja correctamente el estado acumulado hasta la jornada seleccionada.

---

### Edge Cases

- ¿Qué ocurre si se intenta registrar un partido con el mismo equipo como local y visitante?
- ¿Qué ocurre si se intenta registrar un resultado para equipos no existentes o inactivos?
- ¿Cómo se maneja una jornada sin partidos finalizados al momento de consultar ranking?
- ¿Qué ocurre cuando dos o más equipos tienen los mismos puntos en una jornada?
- ¿Cómo se evita la doble carga accidental del mismo resultado para un partido?

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: El sistema MUST permitir crear, editar y desactivar equipos con nombre único dentro de la liga.
- **FR-002**: El sistema MUST permitir crear jornadas identificadas por número secuencial, evitando duplicados.
- **FR-003**: El sistema MUST permitir registrar partidos por jornada con equipo local, equipo visitante y estado del partido.
- **FR-004**: El sistema MUST validar que un partido no pueda existir con el mismo equipo en ambos lados.
- **FR-005**: El sistema MUST permitir registrar y actualizar el marcador final solo para partidos válidos y existentes.
- **FR-006**: El sistema MUST calcular automáticamente puntos por partido finalizado con la regla: victoria = 3, empate = 1, derrota = 0.
- **FR-007**: El sistema MUST recalcular puntajes de equipos cuando un resultado finalizado sea modificado.
- **FR-008**: El sistema MUST mostrar tabla de posiciones por jornada con al menos: equipo, partidos jugados, ganados, empatados, perdidos, goles a favor, goles en contra, diferencia de gol y puntos.
- **FR-009**: El sistema MUST ordenar la tabla por puntos descendentes y, en caso de empate, por diferencia de gol y luego por goles a favor.
- **FR-010**: El sistema MUST permitir consultar el ranking de cualquier jornada existente de la temporada.
- **FR-011**: El sistema MUST informar errores de validación con mensajes claros y sin pérdida de datos previamente válidos.
- **FR-012**: El sistema MUST mantener trazabilidad mínima de cambios de resultados (fecha y tipo de cambio).

### Key Entities *(include if feature involves data)*

- **Team**: Representa un equipo de la liga; atributos clave: identificador, nombre, estado (activo/inactivo), fecha de creación.
- **Matchday**: Representa una jornada del torneo; atributos clave: identificador, número de jornada, estado, rango de fechas.
- **Match**: Representa un partido entre dos equipos en una jornada; atributos clave: local, visitante, jornada, estado, marcador final.
- **StandingEntry**: Representa el acumulado de un equipo hasta una jornada; atributos clave: equipo, jornada, PJ, G, E, P, GF, GC, DG, puntos, posición.
- **ResultChangeLog**: Representa la trazabilidad de cambios de resultados; atributos clave: partido, tipo de cambio, fecha de cambio.

### Assumptions

- La app gestiona una sola liga por entorno de trabajo.
- Los usuarios que acceden tienen permiso para operar resultados y consultar tablas.
- La temporada activa utiliza jornadas numeradas en orden ascendente.

### Dependencies

- Definición de calendario inicial de jornadas.
- Carga inicial de equipos participantes.
- Reglas de puntuación estándar (3/1/0) aprobadas por negocio para la primera versión.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: El 95% de los resultados de partidos se registran correctamente en menos de 60 segundos desde el inicio de la carga.
- **SC-002**: El 100% de los partidos finalizados impacta el puntaje de la tabla sin discrepancias frente a la regla 3/1/0.
- **SC-003**: Al menos el 90% de las consultas de ranking por jornada se completan en menos de 2 segundos percibidos por el usuario.
- **SC-004**: Al menos el 95% de usuarios de operación logra completar el flujo "registrar resultado y ver tabla actualizada" sin asistencia externa en el primer intento.
- **SC-005**: Las incidencias de clasificación incorrecta reportadas por usuarios se reducen al menos 70% frente al proceso manual previo.
