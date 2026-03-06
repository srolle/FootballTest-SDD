const DEFAULT_API_BASE_URL = 'https://localhost:5001'

const normalizeApiBaseUrl = (rawValue: string | undefined): string => {
  const trimmedValue = rawValue?.trim() ?? ''
  const unquotedValue = trimmedValue.replace(/^['\"]+|['\"]+$/g, '')

  if (unquotedValue.length === 0) {
    return DEFAULT_API_BASE_URL
  }

  // Accept host:port values by adding a default http scheme.
  const candidate = /^[a-zA-Z][a-zA-Z\d+\-.]*:/.test(unquotedValue)
    ? unquotedValue
    : `http://${unquotedValue}`

  let parsedUrl: URL
  try {
    parsedUrl = new URL(candidate)
  } catch {
    throw new Error(
      `Invalid VITE_API_BASE_URL value "${rawValue}". Use an absolute URL such as "http://localhost:5080".`,
    )
  }

  if (parsedUrl.protocol !== 'http:' && parsedUrl.protocol !== 'https:') {
    throw new Error(
      `Invalid VITE_API_BASE_URL protocol "${parsedUrl.protocol}". Use http:// or https://.`,
    )
  }

  if (parsedUrl.search || parsedUrl.hash) {
    throw new Error('VITE_API_BASE_URL must not include query parameters or hash fragments.')
  }

  return `${parsedUrl.origin}${parsedUrl.pathname}`.replace(/\/+$/, '')
}

export const apiBaseURL = normalizeApiBaseUrl(import.meta.env.VITE_API_BASE_URL)
