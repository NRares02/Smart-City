export type MapStatus = 'loading' | 'error';

interface MapLoadingStateProps {
  status: MapStatus;
  message: string;
}

function MapLoadingState({ status, message }: MapLoadingStateProps) {
  return (
    <div className={`map-loading-state map-loading-state--${status}`} role="status">
      {status === 'loading' && <span className="map-loading-state__spinner" aria-hidden="true" />}
      <p>{message}</p>
    </div>
  );
}

export default MapLoadingState;
