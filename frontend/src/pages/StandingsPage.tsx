import { useEffect, useState } from 'react'
import { AxiosError } from 'axios'
import { PageTitle } from '../components/PageTitle'
import { StandingsTable } from '../components/StandingsTable'
import { leagueApi } from '../services/leagueApi'
import type { StandingEntry } from '../types/api'

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

  return 'Ocurrio un error inesperado.'
}

export default function StandingsPage() {
  const [matchdayNumber, setMatchdayNumber] = useState(1)
  const [standings, setStandings] = useState<StandingEntry[]>([])
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState('')

  const loadStandings = async (targetMatchdayNumber: number) => {
    setIsLoading(true)
    setError('')

    try {
      const response = await leagueApi.getStandings(targetMatchdayNumber)
      setStandings(response)
    } catch (loadError) {
      setError(getApiErrorMessage(loadError))
      setStandings([])
    } finally {
      setIsLoading(false)
    }
  }

  useEffect(() => {
    void loadStandings(1)
  }, [])

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    await loadStandings(matchdayNumber)
  }

  return (
    <section>
      <PageTitle title="Tabla por jornada" />
      <p>Consulta la clasificación acumulada por jornada con criterios de desempate.</p>

      <form onSubmit={handleSubmit}>
        <label>
          Jornada:
          <input
            type="number"
            min={1}
            value={matchdayNumber}
            onChange={(event) => setMatchdayNumber(Number(event.target.value))}
            required
          />
        </label>
        <button type="submit">Consultar tabla</button>
      </form>

      {isLoading && <p>Cargando tabla...</p>}
      {error && <p>{error}</p>}

      {!isLoading && !error && standings.length > 0 && <StandingsTable entries={standings} />}
      {!isLoading && !error && standings.length === 0 && <p>Sin datos para la jornada seleccionada.</p>}
    </section>
  )
}
