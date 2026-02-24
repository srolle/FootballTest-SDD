import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import ResultsPage from '../pages/ResultsPage'

vi.mock('../services/leagueApi', () => ({
  leagueApi: {
    getTeams: vi.fn(),
    createTeam: vi.fn(),
    updateTeam: vi.fn(),
    deactivateTeam: vi.fn(),
    createMatchday: vi.fn(),
    createMatch: vi.fn(),
    updateResult: vi.fn(),
  },
}))

import { leagueApi } from '../services/leagueApi'

const leagueApiMock = vi.mocked(leagueApi)

describe('ResultsPage form flow', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    leagueApiMock.getTeams.mockResolvedValue([
      { id: 'team-1', name: 'Team A', isActive: true, createdAt: '2026-01-01T00:00:00Z' },
      { id: 'team-2', name: 'Team B', isActive: true, createdAt: '2026-01-01T00:00:00Z' },
    ])
  })

  it('creates a team and submits final result', async () => {
    leagueApiMock.createTeam.mockResolvedValue({
      id: 'team-3',
      name: 'Team C',
      isActive: true,
      createdAt: '2026-01-01T00:00:00Z',
    })
    leagueApiMock.createMatchday.mockResolvedValue({ id: 'md-1', number: 1, createdAt: '2026-01-01T00:00:00Z' })
    leagueApiMock.createMatch.mockResolvedValue({
      id: 'match-1',
      matchdayId: 'md-1',
      homeTeamId: 'team-1',
      awayTeamId: 'team-2',
      homeGoals: null,
      awayGoals: null,
      status: 'Scheduled',
      updatedAt: '2026-01-01T00:00:00Z',
    })
    leagueApiMock.updateResult.mockResolvedValue(undefined)

    render(<ResultsPage />)

    await waitFor(() => expect(leagueApiMock.getTeams).toHaveBeenCalled())

    fireEvent.change(screen.getByLabelText('Nombre:'), { target: { value: 'Team C' } })
    fireEvent.click(screen.getByRole('button', { name: 'Crear equipo' }))

    await waitFor(() => expect(leagueApiMock.createTeam).toHaveBeenCalledWith('Team C'))

    fireEvent.click(screen.getByRole('button', { name: 'Crear jornada' }))
    await waitFor(() => expect(leagueApiMock.createMatchday).toHaveBeenCalledWith(1))

    fireEvent.click(screen.getByRole('button', { name: 'Crear partido' }))
    await waitFor(() => {
      expect(leagueApiMock.createMatch).toHaveBeenCalledWith({
        matchdayId: 'md-1',
        homeTeamId: 'team-1',
        awayTeamId: 'team-2',
      })
    })

    const matchIdInput = screen.getByLabelText('Match ID:') as HTMLInputElement
    expect(matchIdInput.value).toBe('match-1')

    fireEvent.change(screen.getByLabelText('Goles local:'), { target: { value: '3' } })
    fireEvent.change(screen.getByLabelText('Goles visitante:'), { target: { value: '1' } })
    fireEvent.click(screen.getByRole('button', { name: 'Guardar resultado' }))

    await waitFor(() => expect(leagueApiMock.updateResult).toHaveBeenCalledWith('match-1', 3, 1))
  })
})
