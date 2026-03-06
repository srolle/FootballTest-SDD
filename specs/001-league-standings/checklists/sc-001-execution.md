# SC-001 Execution Evidence

Fecha: 2026-03-06 15:57:47 -03:00
Success criterion: at least 95% of 50 valid result registrations complete in <= 60 seconds.

## Execution Method

- Automated integration execution in backend test host.
- Test: `Sc001ResultRegistrationExecutionTests.RegisteringFiftyResults_ShouldMeetSc001Threshold`.
- Command:
  - `dotnet test .\\backend\\tests\\Integration\\Integration.csproj --filter "FullyQualifiedName~Sc001ResultRegistrationExecutionTests" --logger "console;verbosity=detailed"`

## Observed Metrics

- SC001_SAMPLE=50
- SC001_SUCCESS_RATE=100.00
- SC001_AVG_MS=28.54
- SC001_P95_MS=57
- SC001_MIN_MS=13
- SC001_MAX_MS=414

## Result

- Threshold check: PASS.
- 100% of measured registrations completed under 60 seconds.
