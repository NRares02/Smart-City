import { useNavigate } from 'react-router-dom';
import CityMap, { BUCHAREST_CENTER } from '../map/CityMap';
import { usePointsOfInterest } from '../../hooks/usePointsOfInterest';

function ExploreCitySection() {
  const navigate = useNavigate();
  const { pointsOfInterest, categories, isLoading, error } = usePointsOfInterest();

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
        <CityMap
          className="explore-city__map"
          compact
          pointsOfInterest={pointsOfInterest}
          categories={categories}
          center={BUCHAREST_CENTER}
          isLoading={isLoading}
          errorMessage={error}
        />
      </div>
    </section>
  );
}

export default ExploreCitySection;
