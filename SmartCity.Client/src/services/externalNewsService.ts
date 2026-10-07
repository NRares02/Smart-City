import axiosClient from '../api/axiosClient';
import { ENDPOINTS } from '../api/endpoints';
import type { ExternalNews } from '../types/externalNews';

// GET /api/external-news is public (AllowAnonymous) and already returns only
// visible, non-expired items sorted newest-first (published_at desc).
export async function getExternalNews(): Promise<ExternalNews[]> {
  const { data } = await axiosClient.get<ExternalNews[]>(ENDPOINTS.externalNews);
  return data;
}
