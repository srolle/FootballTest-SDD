# SC-004 UX Execution Evidence

Fecha: 2026-03-06 15:57:47 -03:00
Success criterion: at least 95% of 10 executions of "register result and see updated table" complete on first attempt without external assistance.

## Execution Method

- Automated operational flow simulation (10 first-attempt runs).
- Test: `Sc004OperationalFlowExecutionTests.TenOperationalFlows_ShouldSucceedOnFirstAttempt`.
- Command:
  - `dotnet test .\\backend\\tests\\Integration\\Integration.csproj --filter "FullyQualifiedName~Sc004OperationalFlowExecutionTests" --logger "console;verbosity=detailed"`

## Simulated Flow per Attempt

1. Create Team A.
2. Create Team B.
3. Create Matchday.
4. Create Match.
5. Register Result.
6. Query Standings for that matchday.

## Observed Metrics

- SC004_SAMPLE=10
- SC004_SUCCESS_RATE=100.00
- SC004_AVG_FLOW_MS=199.30
- SC004_P95_FLOW_MS=1485
- SC004_MIN_FLOW_MS=41
- SC004_MAX_FLOW_MS=1485

## Result

- Threshold check: PASS.
- 10/10 runs succeeded on first attempt (100%).

## Note

This evidence comes from automated flow execution in local environment and should be complemented with human-user observation in real operational context.
