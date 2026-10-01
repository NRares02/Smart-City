import { useMemo } from 'react';
import { APIProvider, Map, useApiIsLoaded } from '@vis.gl/react-google-maps';
import PoiMarker from './PoiMarker';
import MapLoadingState from './MapLoadingState';
import type { PointOfInterest, PointOfInterestCategory } from '../../types/pointOfInterest';
import './CityMap.css';

const GOOGLE_MAPS_API_KEY = import.meta.env.VITE_GOOGLE_MAPS_API_KEY as string | undefined;
// Public Google demo Map ID — required for AdvancedMarker rendering without a custom cloud style.
const DEMO_MAP_ID = 'DEMO_MAP_ID';

export const BUCHAREST_CENTER = { lat: 44.4268, lng: 26.1025 };
export const DEFAULT_ZOOM = 13;

export interface LatLng {
  lat: number;
  lng: number;
}

export interface CityMapProps {
  pointsOfInterest?: PointOfInterest[];
  categories?: PointOfInterestCategory[];
  center?: LatLng;
  zoom?: number;
  showPoiMarkers?: boolean;
  className?: string;
  /** Compact preview mode (smaller height, cooperative gestures, minimal UI) for embedding in marketing pages. */
  compact?: boolean;
  isLoading?: boolean;
  errorMessage?: string | null;
}

function CityMap({
  pointsOfInterest = [],
  categories = [],
  center = BUCHAREST_CENTER,
  zoom = DEFAULT_ZOOM,
  showPoiMarkers = true,
  className,
  compact = false,
  isLoading = false,
  errorMessage = null,
}: CityMapProps) {
  const wrapperClassName = ['city-map', compact ? 'city-map--compact' : '', className].filter(Boolean).join(' ');

  if (!GOOGLE_MAPS_API_KEY) {
    return (
      <div className={wrapperClassName}>
        <MapLoadingState status="error" message="Map is currently unavailable. Please try again later." />
      </div>
    );
  }

  return (
    <div className={wrapperClassName}>
      <APIProvider apiKey={GOOGLE_MAPS_API_KEY}>
        <CityMapContent
          pointsOfInterest={pointsOfInterest}
          categories={categories}
          center={center}
          zoom={zoom}
          showPoiMarkers={showPoiMarkers}
          compact={compact}
          isLoading={isLoading}
          errorMessage={errorMessage}
        />
      </APIProvider>
    </div>
  );
}

interface CityMapContentProps {
  pointsOfInterest: PointOfInterest[];
  categories: PointOfInterestCategory[];
  center: LatLng;
  zoom: number;
  showPoiMarkers: boolean;
  compact: boolean;
  isLoading: boolean;
  errorMessage: string | null;
}

function CityMapContent({
  pointsOfInterest,
  categories,
  center,
  zoom,
  showPoiMarkers,
  compact,
  isLoading,
  errorMessage,
}: CityMapContentProps) {
  const isApiLoaded = useApiIsLoaded();

  const categoryById = useMemo(() => {
    const record: Record<string, PointOfInterestCategory> = {};
    categories.forEach((category) => {
      if (category.id) {
        record[category.id] = category;
      }
    });
    return record;
  }, [categories]);

  const validPois = useMemo(
    () =>
      pointsOfInterest.filter(
        (poi) =>
          poi.is_active &&
          Number.isFinite(poi.location?.latitude) &&
          Number.isFinite(poi.location?.longitude),
      ),
    [pointsOfInterest],
  );

  if (!isApiLoaded) {
    return <MapLoadingState status="loading" message="Loading map…" />;
  }

  const isEmpty = !isLoading && !errorMessage && showPoiMarkers && validPois.length === 0;

  return (
    <div className="city-map__viewport">
      <Map
        mapId={DEMO_MAP_ID}
        defaultCenter={center}
        defaultZoom={zoom}
        gestureHandling={compact ? 'cooperative' : 'greedy'}
        disableDefaultUI={compact}
        zoomControl={!compact}
        fullscreenControl={false}
        streetViewControl={false}
        mapTypeControl={false}
      >
        {showPoiMarkers &&
          validPois.map((poi) => (
            <PoiMarker
              key={poi.id}
              pointOfInterest={poi}
              categoryName={categoryById[poi.category_id]?.name}
              markerColor={categoryById[poi.category_id]?.marker_color ?? undefined}
            />
          ))}
      </Map>

      {(isLoading || errorMessage) && (
        <div className="city-map__overlay">
          <MapLoadingState status={errorMessage ? 'error' : 'loading'} message={errorMessage ?? 'Loading points of interest…'} />
        </div>
      )}

      {isEmpty && <div className="city-map__empty-badge">No points of interest available yet.</div>}
    </div>
  );
}

export default CityMap;
