import { AxiosError } from 'axios';
import { useEffect, useState } from 'react';
import { getOfficialAnnouncements } from '../services/officialAnnouncementService';
import type { OfficialAnnouncement } from '../types/officialAnnouncement';

interface UseOfficialAnnouncementsResult {
  announcements: OfficialAnnouncement[];
  isLoading: boolean;
  error: string | null;
}

function getOfficialAnnouncementsErrorMessage(error: unknown): string {
  if (error instanceof AxiosError && !error.response) {
    return 'Unable to reach the server right now.';
  }
  return 'Unable to load announcements right now.';
}

export function useOfficialAnnouncements(): UseOfficialAnnouncementsResult {
  const [announcements, setAnnouncements] = useState<OfficialAnnouncement[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;

    async function load() {
      setIsLoading(true);
      setError(null);
      try {
        const data = await getOfficialAnnouncements();
        if (cancelled) return;
        setAnnouncements(data);
      } catch (err) {
        if (cancelled) return;
        setAnnouncements([]);
        setError(getOfficialAnnouncementsErrorMessage(err));
      } finally {
        if (!cancelled) {
          setIsLoading(false);
        }
      }
    }

    void load();
    return () => {
      cancelled = true;
    };
  }, []);

  return { announcements, isLoading, error };
}
