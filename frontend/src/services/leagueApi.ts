import { httpClient } from './httpClient'
import type { Match, Matchday, StandingEntry, Team } from '../types/api'

export const leagueApi = {
  async getTeams(): Promise<Team[]> {
    const response = await httpClient.get<Team[]>('/teams')
    return response.data
  },

  async createTeam(name: string): Promise<Team> {
    const response = await httpClient.post<Team>('/teams', { name })
    return response.data
  },

  async updateTeam(teamId: string, name: string): Promise<Team> {
    const response = await httpClient.put<Team>(`/teams/${teamId}`, { name })
    return response.data
  },

  async deactivateTeam(teamId: string): Promise<Team> {
    const response = await httpClient.patch<Team>(`/teams/${teamId}/deactivate`)
    return response.data
  },

  async createMatchday(number: number): Promise<Matchday> {
    const response = await httpClient.post<Matchday>('/matchdays', { number })
    return response.data
  },

  async createMatch(payload: {
    matchdayId: string
    homeTeamId: string
    awayTeamId: string
  }): Promise<Match> {
    const response = await httpClient.post<Match>('/matches', payload)
    return response.data
  },

  async updateResult(matchId: string, homeGoals: number, awayGoals: number): Promise<void> {
    await httpClient.put(`/matches/${matchId}/result`, { homeGoals, awayGoals })
  },

  async getStandings(matchdayNumber: number): Promise<StandingEntry[]> {
    const response = await httpClient.get<StandingEntry[]>('/standings', {
      params: { matchdayNumber },
    })
    return response.data
  },
}
