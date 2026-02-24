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
