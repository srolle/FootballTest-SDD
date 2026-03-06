import type { StandingEntry } from '../types/api'

type StandingsTableProps = {
  entries: StandingEntry[]
}

export function StandingsTable({ entries }: StandingsTableProps) {
  return (
    <table aria-label="Tabla de posiciones" style={{ width: '100%', borderCollapse: 'collapse' }}>
      <thead>
        <tr>
          <th>#</th>
          <th>Equipo</th>
          <th>PJ</th>
          <th>G</th>
          <th>E</th>
          <th>P</th>
          <th>GF</th>
          <th>GC</th>
          <th>DG</th>
          <th>Pts</th>
        </tr>
      </thead>
      <tbody>
        {entries.map((entry) => (
          <tr key={entry.teamId}>
            <td>{entry.position}</td>
            <td>{entry.teamName}</td>
            <td>{entry.played}</td>
            <td>{entry.won}</td>
            <td>{entry.drawn}</td>
            <td>{entry.lost}</td>
            <td>{entry.goalsFor}</td>
            <td>{entry.goalsAgainst}</td>
            <td>{entry.goalDifference}</td>
            <td>{entry.points}</td>
          </tr>
        ))}
      </tbody>
    </table>
  )
}