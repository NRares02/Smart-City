import { AxiosError } from 'axios';
import { useEffect, useState } from 'react';
import { getExternalNews } from '../services/externalNewsService';
import type { ExternalNews } from '../types/externalNews';

interface UseExternalNewsResult {
  news: ExternalNews[];
  isLoading: boolean;
  error: string | null;
}

function getExternalNewsErrorMessage(error: unknown): string {
  if (error instanceof AxiosError && !error.response) {
    return 'Unable to reach the server right now.';
  }
  return 'Unable to load news right now.';
}

export function useExternalNews(): UseExternalNewsResult {
  const [news, setNews] = useState<ExternalNews[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;

    async function load() {
      setIsLoading(true);
      setError(null);
      try {
        const data = await getExternalNews();
        if (cancelled) return;
        setNews(data);
      } catch (err) {
        if (cancelled) return;
        setNews([]);
        setError(getExternalNewsErrorMessage(err));
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

  return { news, isLoading, error };
}
