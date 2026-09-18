// Presentation placeholders only — replace with real API-backed metrics later.
const STATS = [
  { value: '1,250+', label: 'Reports Submitted' },
  { value: '870+', label: 'Issues Resolved' },
  { value: '120+', label: 'City Points of Interest' },
  { value: '5,000+', label: 'Connected Citizens' },
];

function CityStats() {
  return (
    <section className="city-stats">
      <div className="city-stats__grid">
        {STATS.map((stat) => (
          <div className="city-stats__item" key={stat.label}>
            <span className="city-stats__value">{stat.value}</span>
            <span className="city-stats__label">{stat.label}</span>
          </div>
        ))}
      </div>
    </section>
  );
}

export default CityStats;
