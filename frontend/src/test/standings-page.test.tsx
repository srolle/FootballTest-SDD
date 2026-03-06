import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import StandingsPage from '../pages/StandingsPage'

vi.mock('../services/leagueApi', () => ({
  leagueApi: {
    getStandings: vi.fn(),
  },
}))

import { leagueApi } from '../services/leagueApi'

const leagueApiMock = vi.mocked(leagueApi)

describe('StandingsPage', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    leagueApiMock.getStandings.mockResolvedValue([
      {
        teamId: 'team-1',
        teamName: 'Team C',
        played: 1,
        won: 1,
        drawn: 0,
        lost: 0,
        goalsFor: 2,
        goalsAgainst: 0,
        goalDifference: 2,
        points: 3,
        position: 1,
      },
      {
        teamId: 'team-2',
        teamName: 'Team A',
        played: 1,
        won: 1,
        drawn: 0,
        lost: 0,
        goalsFor: 1,
        goalsAgainst: 0,
        goalDifference: 1,
        points: 3,
        position: 2,
      },
    ])
  })

  it('loads standings and allows querying another matchday', async () => {
    render(<StandingsPage />)

    await waitFor(() => expect(leagueApiMock.getStandings).toHaveBeenCalledWith(1))

    expect(screen.getByRole('table', { name: 'Tabla de posiciones' })).toBeInTheDocument()
    expect(screen.getByText('Team C')).toBeInTheDocument()
    expect(screen.getByText('Team A')).toBeInTheDocument()

    fireEvent.change(screen.getByLabelText('Jornada:'), { target: { value: '2' } })
    fireEvent.click(screen.getByRole('button', { name: 'Consultar tabla' }))

    await waitFor(() => expect(leagueApiMock.getStandings).toHaveBeenCalledWith(2))
  })
})