import { useCallback, useEffect, useState } from 'react'
import { AxiosError } from 'axios'
import { PageTitle } from '../components/PageTitle'
import { leagueApi } from '../services/leagueApi'
import type { Team } from '../types/api'

type ProblemDetailsPayload = {
  title?: string
  detail?: string
  errors?: Record<string, string[]>
}

function getApiErrorMessage(error: unknown): string {
  if (error instanceof AxiosError) {
    const payload = error.response?.data as ProblemDetailsPayload | undefined
    const fieldErrors = payload?.errors
      ? Object.values(payload.errors).flat().join(' ')
      : ''
    return fieldErrors || payload?.detail || payload?.title || error.message
  }
  if (error instanceof Error) {
    return error.message
  }
  return 'Ocurrió un error inesperado.'
}

export default function ResultsPage() {
  const [teams, setTeams] = useState<Team[]>([])

  const [teamName, setTeamName] = useState('')
  const [matchdayNumber, setMatchdayNumber] = useState(1)
  const [homeTeamId, setHomeTeamId] = useState('')
  const [awayTeamId, setAwayTeamId] = useState('')
  const [matchId, setMatchId] = useState('')
  const [homeGoals, setHomeGoals] = useState(0)
  const [awayGoals, setAwayGoals] = useState(0)

  const [createdMatchdayId, setCreatedMatchdayId] = useState('')
  const [createdMatchId, setCreatedMatchId] = useState('')

  const [message, setMessage] = useState('')
  const [error, setError] = useState('')
  const [isLoading, setIsLoading] = useState(false)

  const refreshTeams = useCallback(async () => {
    const teamList = await leagueApi.getTeams()
    setTeams(teamList)
    if (!homeTeamId && teamList.length > 0) {
      setHomeTeamId(teamList[0].id)
    }
    if (!awayTeamId && teamList.length > 1) {
      setAwayTeamId(teamList[1].id)
    }
  }, [awayTeamId, homeTeamId])

  useEffect(() => {
    const loadTeams = async () => {
      setIsLoading(true)
      setError('')
      try {
        await refreshTeams()
      } catch (loadError) {
        setError(getApiErrorMessage(loadError))
      } finally {
        setIsLoading(false)
      }
    }

    void loadTeams()
  }, [refreshTeams])

  const handleCreateTeam = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    setMessage('')
    setError('')

    try {
      const createdTeam = await leagueApi.createTeam(teamName)
      setTeamName('')
      setMessage(`Equipo creado: ${createdTeam.name}`)
      await refreshTeams()
    } catch (createError) {
      setError(getApiErrorMessage(createError))
    }
  }

  const handleCreateMatchday = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    setMessage('')
    setError('')

    try {
      const createdMatchday = await leagueApi.createMatchday(matchdayNumber)
      setCreatedMatchdayId(createdMatchday.id)
      setMessage(`Jornada creada: #${createdMatchday.number}`)
    } catch (createError) {
      setError(getApiErrorMessage(createError))
    }
  }

  const handleCreateMatch = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    setMessage('')
    setError('')

    try {
      const createdMatch = await leagueApi.createMatch({
        matchdayId: createdMatchdayId,
        homeTeamId,
        awayTeamId,
      })
      setCreatedMatchId(createdMatch.id)
      setMatchId(createdMatch.id)
      setMessage(`Partido creado: ${createdMatch.id}`)
    } catch (createError) {
      setError(getApiErrorMessage(createError))
    }
  }

  const handleUpdateResult = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    setMessage('')
    setError('')

    try {
      await leagueApi.updateResult(matchId, homeGoals, awayGoals)
      setMessage('Resultado actualizado correctamente.')
    } catch (updateError) {
      setError(getApiErrorMessage(updateError))
    }
  }

  const activeTeams = teams.filter((team) => team.isActive)

  return (
    <section>
      <PageTitle title="Registrar resultados" />

      <p>Flujo US1: crear equipos, crear jornada, crear partido y registrar marcador final.</p>

      {isLoading && <p>Cargando equipos...</p>}
      {message && <p>{message}</p>}
      {error && <p>{error}</p>}

      <form onSubmit={handleCreateTeam}>
        <h3>1) Crear equipo</h3>
        <label>
          Nombre:
          <input value={teamName} onChange={(event) => setTeamName(event.target.value)} required />
        </label>
        <button type="submit">Crear equipo</button>
      </form>

      <form onSubmit={handleCreateMatchday}>
        <h3>2) Crear jornada</h3>
        <label>
          Número:
          <input
            type="number"
            min={1}
            value={matchdayNumber}
            onChange={(event) => setMatchdayNumber(Number(event.target.value))}
            required
          />
        </label>
        <button type="submit">Crear jornada</button>
      </form>

      <form onSubmit={handleCreateMatch}>
        <h3>3) Crear partido</h3>
        <p>Jornada seleccionada: {createdMatchdayId || 'No creada aún'}</p>

        <label>
          Local:
          <select value={homeTeamId} onChange={(event) => setHomeTeamId(event.target.value)} required>
            <option value="">Selecciona equipo local</option>
            {activeTeams.map((team) => (
              <option key={team.id} value={team.id}>
                {team.name}
              </option>
            ))}
          </select>
        </label>

        <label>
          Visitante:
          <select value={awayTeamId} onChange={(event) => setAwayTeamId(event.target.value)} required>
            <option value="">Selecciona equipo visitante</option>
            {activeTeams.map((team) => (
              <option key={team.id} value={team.id}>
                {team.name}
              </option>
            ))}
          </select>
        </label>

        <button type="submit" disabled={!createdMatchdayId}>
          Crear partido
        </button>
      </form>

      <form onSubmit={handleUpdateResult}>
        <h3>4) Registrar resultado</h3>
        <label>
          Match ID:
          <input value={matchId} onChange={(event) => setMatchId(event.target.value)} required />
        </label>
        <p>Último match creado: {createdMatchId || 'No creado aún'}</p>

        <label>
          Goles local:
          <input
            type="number"
            min={0}
            value={homeGoals}
            onChange={(event) => setHomeGoals(Number(event.target.value))}
            required
          />
        </label>

        <label>
          Goles visitante:
          <input
            type="number"
            min={0}
            value={awayGoals}
            onChange={(event) => setAwayGoals(Number(event.target.value))}
            required
          />
        </label>

        <button type="submit">Guardar resultado</button>
      </form>
    </section>
  )
}
