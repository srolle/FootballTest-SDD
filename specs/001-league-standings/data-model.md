# Data Model: Football League Standings Management

## Entity: Team
- Purpose: Representa un equipo participante.
- Fields:
  - Id (GUID o entero único)
  - Name (string, único por liga)
  - IsActive (boolean)
  - CreatedAt (datetime)
- Rules:
  - Name es obligatorio y único.
  - Solo equipos activos pueden ser asignados a nuevos partidos.

## Entity: Matchday
- Purpose: Representa una jornada de competición.
- Fields:
  - Id
  - Number (entero secuencial)
  - Status (Scheduled, InProgress, Closed)
  - StartDate (date, opcional)
  - EndDate (date, opcional)
- Rules:
  - Number es único por temporada activa.

## Entity: Match
- Purpose: Representa un partido de una jornada.
- Fields:
  - Id
  - MatchdayId (FK)
  - HomeTeamId (FK)
  - AwayTeamId (FK)
  - HomeGoals (entero, nullable hasta finalizar)
  - AwayGoals (entero, nullable hasta finalizar)
  - Status (Scheduled, Finished)
  - UpdatedAt (datetime)
- Rules:
  - HomeTeamId y AwayTeamId deben ser distintos.
  - Goles no pueden ser negativos.
  - Solo partidos existentes pueden actualizar resultado.

## Entity: StandingEntry
- Purpose: Snapshot de tabla por jornada para un equipo.
- Fields:
  - Id
  - MatchdayId (FK)
  - TeamId (FK)
  - Played
  - Won
  - Drawn
  - Lost
  - GoalsFor
  - GoalsAgainst
  - GoalDifference
  - Points
  - Position
- Rules:
  - Points se calculan únicamente desde resultados válidos.
  - Orden: Points DESC, GoalDifference DESC, GoalsFor DESC.

## Entity: ResultChangeLog
- Purpose: Trazabilidad mínima de cambios en resultados.
- Fields:
  - Id
  - MatchId (FK)
  - ChangeType (CreatedResult, UpdatedResult)
  - ChangedAt (datetime)
- Rules:
  - Se crea registro en cada alta/modificación de resultado final.

## Relationships
- Matchday 1..N Match
- Team 1..N Match (como local o visitante)
- Matchday 1..N StandingEntry
- Team 1..N StandingEntry
- Match 1..N ResultChangeLog

## State Transitions
- Match.Status:
  - Scheduled -> Finished (al registrar resultado válido)
  - Finished -> Finished (al editar resultado, mantiene estado)

## Validation Summary
- Nombre de equipo único.
- Jornada única por número.
- Partido con equipos distintos.
- Goles no negativos.
- Recalcular tabla tras cambios de resultados finalizados.
