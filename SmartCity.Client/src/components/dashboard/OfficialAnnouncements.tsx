import { Link } from 'react-router-dom';
import { ChevronRightIcon, MegaphoneIcon } from './icons';

interface Announcement {
  id: string;
  title: string;
  summary: string;
  date: string;
  category: string;
}

// Temporary placeholder data until official announcements are wired into the dashboard.
const ANNOUNCEMENTS: Announcement[] = [
  {
    id: '1',
    title: 'Temporary road closure on Elm Street',
    summary: 'Roadworks scheduled between Sep 20–22 for resurfacing. Expect detours.',
    date: 'Sep 17, 2026',
    category: 'Roads',
  },
  {
    id: '2',
    title: 'Water supply maintenance',
    summary: 'Scheduled maintenance may cause brief interruptions in the downtown area.',
    date: 'Sep 15, 2026',
    category: 'Utilities',
  },
  {
    id: '3',
    title: 'Public city event this weekend',
    summary: 'Join the community fair at Riverside Park starting Saturday morning.',
    date: 'Sep 12, 2026',
    category: 'Community',
  },
];

function OfficialAnnouncements() {
  return (
    <section className="dashboard-panel">
      <div className="dashboard-panel__header">
        <h2 className="dashboard-panel__title">Official Announcements</h2>
        <Link to="/official-announcements" className="dashboard-panel__link">
          View all announcements <ChevronRightIcon size={15} />
        </Link>
      </div>

      <div className="announcement-grid">
        {ANNOUNCEMENTS.map((item) => (
          <article className="announcement-card" key={item.id}>
            <div className="announcement-card__icon">
              <MegaphoneIcon size={18} />
            </div>
            <span className="announcement-card__category">{item.category}</span>
            <h3 className="announcement-card__title">{item.title}</h3>
            <p className="announcement-card__summary">{item.summary}</p>
            <p className="announcement-card__date">{item.date}</p>
          </article>
        ))}
      </div>
    </section>
  );
}

export default OfficialAnnouncements;
