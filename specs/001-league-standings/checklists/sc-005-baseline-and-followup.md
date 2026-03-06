# SC-005 Baseline and Follow-up Plan

Fecha: 2026-03-06 15:57:47 -03:00
Success criterion: weekly incorrect-classification incidents reduced by at least 70% after 4 weeks compared to previous 4-week manual baseline.

## Baseline Definition

Measurement channel: official support channel for classification incidents.

## Baseline Window (pre-release)

- Window length: 4 weeks.
- Scope: incidents explicitly tagged as "incorrect classification".
- Required fields per incident:
  - incidentId
  - createdAt
  - reporter
  - classificationType
  - rootCause
  - status

## Current Baseline Status

- Production/manual historical data is not available in local development workspace.
- Baseline capture template is defined and ready for data entry.
- Baseline value to be populated before release approval.

## Follow-up Window (post-release)

- Window length: 4 weeks after enabling standings workflow.
- Weekly tracking fields:
  - weekNumber
  - incidentCount
  - cumulativeIncidentCount
  - notes

## Reduction Formula

- baselineWeeklyAvg = baselineIncidents / 4
- followupWeeklyAvg = followupIncidents / 4
- reductionPct = ((baselineWeeklyAvg - followupWeeklyAvg) / baselineWeeklyAvg) * 100
- target: reductionPct >= 70

## Follow-up Cadence

- Week 1: collect incidents and validate tags.
- Week 2: review trend and adjust triage consistency.
- Week 3: confirm stability of standings behavior in support channel.
- Week 4: compute final reduction percentage and issue SC-005 verdict.
