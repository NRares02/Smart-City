import { useCallback, useState } from 'react';
import { AdvancedMarker, InfoWindow, Pin, useAdvancedMarkerRef } from '@vis.gl/react-google-maps';
import type { PointOfInterest } from '../../types/pointOfInterest';

interface PoiMarkerProps {
  pointOfInterest: PointOfInterest;
  categoryName?: string;
  markerColor?: string;
}

function PoiMarker({ pointOfInterest, categoryName, markerColor }: PoiMarkerProps) {
  const [markerRef, marker] = useAdvancedMarkerRef();
  const [isInfoOpen, setIsInfoOpen] = useState(false);

  const handleClick = useCallback(() => setIsInfoOpen((open) => !open), []);
  const handleClose = useCallback(() => setIsInfoOpen(false), []);

  const { latitude, longitude } = pointOfInterest.location;
  if (latitude == null || longitude == null) {
    return null;
  }

  const position = { lat: latitude, lng: longitude };

  return (
    <>
      <AdvancedMarker ref={markerRef} position={position} title={pointOfInterest.name} onClick={handleClick}>
        <Pin background={markerColor ?? '#2381c4'} borderColor="#0f172a" glyphColor="#ffffff" />
      </AdvancedMarker>
      {isInfoOpen && marker && (
        <InfoWindow anchor={marker} onCloseClick={handleClose} maxWidth={260}>
          <div className="poi-marker__info">
            <h3>{pointOfInterest.name}</h3>
            {categoryName && <span className="poi-marker__category">{categoryName}</span>}
            {pointOfInterest.address && <p>{pointOfInterest.address}</p>}
            {pointOfInterest.opening_hours && <p>{pointOfInterest.opening_hours}</p>}
          </div>
        </InfoWindow>
      )}
    </>
  );
}

export default PoiMarker;
