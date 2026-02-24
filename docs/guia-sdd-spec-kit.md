# Guía paso a paso: SDD con Spec Kit (GitHub)

Esta guía te enseña a aplicar **Spec-Driven Development (SDD)** en una app real, usando `spec-kit` y GitHub Copilot dentro de este repositorio.

## 0) Objetivo de esta guía

- Aprender el flujo SDD completo: **constitución → especificación → plan → tareas → implementación**.
- Dejar trazabilidad en artefactos versionables (Markdown en el repo).
- Evitar "vibe coding" sin criterios de calidad.

## 1) Prerrequisitos

Ya verificados en este entorno:

- `git`
- `python` (3.11+ recomendado por spec-kit)
- `uv`
- `specify` (instalado con `uv tool install`)

Comando útil:

```powershell
specify check
```

## 2) Inicialización del proyecto Spec Kit

Este repo ya fue inicializado para Copilot y PowerShell.

Referencia del comando usado:

```powershell
specify init --here --ai copilot --script ps --ignore-agent-tools --force
```

Archivos importantes generados:

- `.github/prompts/` (prompts de comandos `/speckit.*`)
- `.github/agents/` (agentes auxiliares)
- `.specify/templates/` (plantillas base)
- `.specify/scripts/powershell/` (scripts de soporte)
- `.specify/memory/constitution.md` (constitución del proyecto)

## 3) Flujo SDD recomendado (ciclo completo)

## Fase A — Constitución del proyecto

En Copilot Chat ejecuta:

```text
/speckit.constitution Define principios para una app de fútbol: calidad de código, pruebas automatizadas, UX consistente, rendimiento y seguridad básica.
```

Qué debe salir:

- Principios claros y medibles.
- Restricciones técnicas (stack, estándares, testing, linting, CI).
- Reglas de gobernanza (cuándo se puede excepcionar una regla).

Resultado esperado:

- `.specify/memory/constitution.md` completado (sin placeholders).

---

## Fase B — Especificación funcional

En Copilot Chat ejecuta:

```text
/speckit.specify Construir una app web para gestionar equipos, partidos y tabla de posiciones. Debe permitir registrar resultados, calcular puntos y mostrar ranking por jornada.
```

Buenas prácticas:

- Describe **qué** problema resuelve y **por qué**.
- Evita definir tecnología en esta fase.
- Incluye criterios de aceptación y casos borde.

Opcional (muy recomendado):

```text
/speckit.clarify
```

Usa esta fase para resolver ambigüedades antes de planificar.

---

## Fase C — Plan técnico

En Copilot Chat ejecuta:

```text
/speckit.plan App con stack mínimo (por ejemplo, React + TypeScript + tests), arquitectura simple por módulos, validación de entrada y cobertura mínima de pruebas.
```

Qué validar:

- Arquitectura coherente con la constitución.
- Riesgos técnicos identificados.
- Estrategia de pruebas por niveles (unit, integración, e2e si aplica).

---

## Fase D — Desglose en tareas

En Copilot Chat ejecuta:

```text
/speckit.tasks
```

Qué debe generar:

- Tareas pequeñas, ejecutables y verificables.
- Orden lógico (fundaciones → dominio → UI/API → pruebas → hardening).
- Dependencias explícitas entre tareas.

Opcional de calidad:

```text
/speckit.analyze
/speckit.checklist
```

---

## Fase E — Implementación guiada

En Copilot Chat ejecuta:

```text
/speckit.implement
```

Sugerencia práctica:

- Implementa por lotes pequeños.
- Corre pruebas al cerrar cada lote.
- Si falla algo, ajusta primero especificación/plan antes de parchear sin criterio.

## 4) Cómo documentar TODO el proceso (evidencia)

Usa la plantilla `docs/bitacora-sdd.md` y completa:

- Prompt usado en cada comando `/speckit.*`.
- Artefacto generado (archivo o sección).
- Decisiones tomadas y trade-offs.
- Riesgos detectados + mitigación.
- Evidencias: tests, build, screenshots (si aplica).

## 5) Cadencia recomendada para aprender rápido

- Iteración 1 (MVP): alcance mínimo funcional.
- Iteración 2: robustez + cobertura.
- Iteración 3: observabilidad/performance.

Regla de oro SDD:

- Si hay dudas de implementación, vuelve a **spec/plan**.
- No escales complejidad sin una necesidad explícita en los artefactos.

## 6) Definición de "hecho" (DoD) para tu guía

Una feature se considera cerrada cuando:

- Cumple constitución y criterios de aceptación.
- Tiene tareas cerradas y evidencia de validación.
- Pruebas y checks de calidad pasan.
- La bitácora registra decisiones y resultados.

## 7) Comandos de referencia rápida

```powershell
specify check
```

```text
/speckit.constitution
/speckit.specify
/speckit.clarify
/speckit.plan
/speckit.tasks
/speckit.analyze
/speckit.checklist
/speckit.implement
```

## 8) Siguiente paso sugerido (ahora)

1. Ejecutar `/speckit.constitution` con tus principios reales del proyecto.
2. Definir la primera feature con `/speckit.specify`.
3. Completar `docs/bitacora-sdd.md` a medida que avances.
