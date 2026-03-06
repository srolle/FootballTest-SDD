import axios from 'axios'
import { apiBaseURL } from '../config/apiBaseUrl'

export const httpClient = axios.create({
  baseURL: apiBaseURL,
  headers: {
    'Content-Type': 'application/json',
  },
})
