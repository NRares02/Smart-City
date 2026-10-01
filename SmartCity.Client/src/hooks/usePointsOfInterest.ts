import { AxiosError } from 'axios';
import { useEffect, useState } from 'react';
import { getPointOfInterestCategories, getPointsOfInterest } from '../services/pointOfInterestService';
import type { PointOfInterest, PointOfInterestCategory } from '../types/pointOfInterest';

interface UsePointsOfInterestResult {
  pointsOfInterest: PointOfInterest[];
  categories: PointOfInterestCategory[];
  isLoading: boolean;
  error: string | null;
}

// GET /api/points-of-interest and /api/point-of-interest-categories are public (AllowAnonymous),
// so this only surfaces real infrastructure failures — never an auth prompt.
function getPoiErrorMessage(error: unknown): string {
  if (error instanceof AxiosError && !error.response) {
    return 'Unable to reach the server right now.';
  }
  return 'Unable to load points of interest right now.';
}

export function usePointsOfInterest(): UsePointsOfInterestResult {
  const [pointsOfInterest, setPointsOfInterest] = useState<PointOfInterest[]>([]);
  const [categories, setCategories] = useState<PointOfInterestCategory[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    let cancelled = false;

    async function load() {
      setIsLoading(true);
      setError(null);
      try {
        const [poiData, categoryData] = await Promise.all([
          getPointsOfInterest(),
          getPointOfInterestCategories(),
        ]);
        if (cancelled) return;
        setPointsOfInterest(poiData);
        setCategories(categoryData);
      } catch (err) {
        if (cancelled) return;
        setPointsOfInterest([]);
        setCategories([]);
        setError(getPoiErrorMessage(err));
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

  return { pointsOfInterest, categories, isLoading, error };
}
