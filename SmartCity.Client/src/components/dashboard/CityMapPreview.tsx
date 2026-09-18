import { useNavigate } from 'react-router-dom';
import { MapPinIcon } from './icons';

function CityMapPreview() {
  const navigate = useNavigate();

  return (
    <section className="dashboard-panel city-map">
      <div className="dashboard-panel__header">
        <h2 className="dashboard-panel__title">Explore the City</h2>
      </div>

      <div className="city-map__preview">
        <div className="city-map__grid" aria-hidden="true" />
        <MapPinIcon size={34} className="city-map__pin" />
        <p className="city-map__hint">Interactive city map coming soon</p>
      </div>

      <button type="button" className="city-map__button" onClick={() => navigate('/map')}>
        Open City Map
      </button>
    </section>
  );
}

export default CityMapPreview;
