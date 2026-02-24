import { Link, Outlet } from 'react-router-dom'

export function AppLayout() {
  return (
    <div style={{ maxWidth: 960, margin: '0 auto', padding: '1rem' }}>
      <header>
        <h1>Football League Standings</h1>
        <nav style={{ display: 'flex', gap: '1rem', marginBottom: '1rem' }}>
          <Link to="/">Inicio</Link>
          <Link to="/results">Resultados</Link>
          <Link to="/standings">Tabla</Link>
        </nav>
      </header>
      <main>
        <Outlet />
      </main>
    </div>
  )
}
