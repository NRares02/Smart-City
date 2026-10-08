import axiosClient from '../api/axiosClient';
import { ENDPOINTS } from '../api/endpoints';
import type { OfficialAnnouncement } from '../types/officialAnnouncement';

// GET /api/official-announcements is public (AllowAnonymous) and already returns only
// published, non-expired announcements sorted pinned-first then newest-first.
export async function getOfficialAnnouncements(): Promise<OfficialAnnouncement[]> {
  const { data } = await axiosClient.get<OfficialAnnouncement[]>(ENDPOINTS.officialAnnouncements);
  return data;
}
