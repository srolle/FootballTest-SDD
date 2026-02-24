# Quickstart: Football League Standings Management

## Objetivo
Levantar backend API .NET y frontend React para validar los flujos principales:
1. Registrar resultado de partido.
2. Ver puntos recalculados.
3. Consultar ranking por jornada.

## Prerrequisitos
- .NET SDK (última versión estable)
- Node.js LTS + npm
- SQLite (o runtime integrado en la app)

## Pasos sugeridos
1. Restaurar dependencias de backend.
2. Crear/aplicar esquema SQLite inicial.
3. Ejecutar backend API.
4. Instalar dependencias frontend React.
5. Ejecutar frontend en modo desarrollo.
6. Configurar URL de API en frontend.

## Escenario de validación MVP
1. Crear 4 equipos activos.
2. Crear jornada 1.
3. Registrar 2 partidos de jornada 1.
4. Cargar resultados válidos (una victoria y un empate).
5. Consultar ranking de jornada 1.
6. Verificar:
   - Puntos asignados con regla 3/1/0.
   - Orden por puntos, diferencia de gol y goles a favor.

## Escenario de recálculo
1. Editar resultado de un partido finalizado.
2. Consultar nuevamente ranking de la misma jornada.
3. Confirmar recálculo de puntos y registro en trazabilidad de cambios.

## Señales de éxito
- API rechaza datos inválidos con mensaje claro.
- Ranking responde de forma consistente con los resultados.
- Existe evidencia de trazabilidad para cambios de resultados.
