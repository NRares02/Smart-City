import { useNavigate } from 'react-router-dom';
import { MapPinIcon } from '../dashboard/icons';

function ExploreCitySection() {
  const navigate = useNavigate();

  return (
    <section id="explore" className="explore-city">
      <div className="explore-city__text">
        <p className="features__eyebrow">City Map</p>
        <h2>Explore your city</h2>
        <p>
          Discover useful places, public services and important locations around you, all in one
          place.
        </p>
        <button type="button" className="explore-city__button" onClick={() => navigate('/map')}>
          Explore City Map
        </button>
      </div>

      <div className="explore-city__visual">
        <div className="explore-city__grid" aria-hidden="true" />
        <MapPinIcon size={40} className="explore-city__pin" />
        <MapPinIcon size={24} className="explore-city__pin explore-city__pin--sm explore-city__pin--a" />
        <MapPinIcon size={24} className="explore-city__pin explore-city__pin--sm explore-city__pin--b" />
        <p>Interactive city map coming soon</p>
      </div>
    </section>
  );
}

export default ExploreCitySection;
