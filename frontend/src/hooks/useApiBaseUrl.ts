export const useApiBaseUrl = (): string => {
  return import.meta.env.VITE_API_BASE_URL ?? 'https://localhost:5001'
}
