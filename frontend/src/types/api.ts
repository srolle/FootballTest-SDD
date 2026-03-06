export type Team = {
  id: string
  name: string
  isActive: boolean
  createdAt: string
}

export type Matchday = {
  id: string
  number: number
  createdAt: string
}

export type Match = {
  id: string
  matchdayId: string
  homeTeamId: string
  awayTeamId: string
  homeGoals: number | null
  awayGoals: number | null
  status: string
  updatedAt: string
}

export type StandingEntry = {
  teamId: string
  teamName: string
  played: number
  won: number
  drawn: number
  lost: number
  goalsFor: number
  goalsAgainst: number
  goalDifference: number
  points: number
  position: number
}
