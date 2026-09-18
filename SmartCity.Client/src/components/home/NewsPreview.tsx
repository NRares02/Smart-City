import { useNavigate } from 'react-router-dom';
import { MegaphoneIcon, NewsIcon } from '../dashboard/icons';

// Temporary presentation content — replace with real API-backed articles/announcements later.
const NEWS_ITEMS = [
  { category: 'City Council', title: 'New budget approved for park renovations', date: 'Sep 15, 2026' },
  { category: 'Transport', title: 'Bus line 12 extended to the northern district', date: 'Sep 11, 2026' },
  { category: 'Community', title: 'Local library launches weekend reading program', date: 'Sep 6, 2026' },
];

const ANNOUNCEMENT_ITEMS = [
  { category: 'Roads', title: 'Temporary road closure on Elm Street', date: 'Sep 17, 2026' },
  { category: 'Utilities', title: 'Scheduled water supply maintenance', date: 'Sep 15, 2026' },
  { category: 'Community', title: 'Public city event this weekend', date: 'Sep 12, 2026' },
];

function NewsPreview() {
  const navigate = useNavigate();

  return (
    <section id="news" className="news-preview">
      <div className="news-preview__group">
        <div className="news-preview__header">
          <h2>Latest News</h2>
          <button type="button" onClick={() => navigate('/news')}>
            View all news →
          </button>
        </div>
        <div className="news-preview__grid">
          {NEWS_ITEMS.map((item) => (
            <article className="news-card" key={item.title}>
              <div className="news-card__image">
                <NewsIcon size={22} />
              </div>
              <span className="news-card__category">{item.category}</span>
              <h3>{item.title}</h3>
              <p>{item.date}</p>
            </article>
          ))}
        </div>
      </div>

      <div className="news-preview__group">
        <div className="news-preview__header">
          <h2>Official Announcements</h2>
          <button type="button" onClick={() => navigate('/official-announcements')}>
            View all announcements →
          </button>
        </div>
        <div className="news-preview__grid">
          {ANNOUNCEMENT_ITEMS.map((item) => (
            <article className="news-card" key={item.title}>
              <div className="news-card__image news-card__image--accent">
                <MegaphoneIcon size={22} />
              </div>
              <span className="news-card__category">{item.category}</span>
              <h3>{item.title}</h3>
              <p>{item.date}</p>
            </article>
          ))}
        </div>
      </div>
    </section>
  );
}

export default NewsPreview;
