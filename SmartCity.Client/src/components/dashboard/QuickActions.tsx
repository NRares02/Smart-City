import { useNavigate } from 'react-router-dom';
import { ClipboardIcon, MapIcon, NewsIcon, ReportsIcon } from './icons';

const SECONDARY_ACTIONS = [
  { to: '/reports', label: 'View My Reports', icon: ReportsIcon },
  { to: '/map', label: 'Explore City Map', icon: MapIcon },
  { to: '/news', label: 'Read Latest News', icon: NewsIcon },
];

function QuickActions() {
  const navigate = useNavigate();

  return (
    <section className="dashboard-panel quick-actions">
      <h2 className="dashboard-panel__title">Quick Actions</h2>
      <div className="quick-actions__grid">
        <button
          type="button"
          className="quick-actions__primary"
          onClick={() => navigate('/reports/new')}
        >
          <ClipboardIcon size={22} />
          <span>Report a Problem</span>
        </button>

        {SECONDARY_ACTIONS.map(({ to, label, icon: Icon }) => (
          <button
            key={to}
            type="button"
            className="quick-actions__secondary"
            onClick={() => navigate(to)}
          >
            <Icon size={20} />
            <span>{label}</span>
          </button>
        ))}
      </div>
    </section>
  );
}

export default QuickActions;
