import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import { useExternalNews } from '../../hooks/useExternalNews';
import type { ExternalNews } from '../../types/externalNews';
import { MegaphoneIcon, NewsIcon } from '../dashboard/icons';

// Temporary presentation content — replace with real API-backed announcements later.
const ANNOUNCEMENT_ITEMS = [
  { category: 'Roads', title: 'Temporary road closure on Elm Street', date: 'Sep 17, 2026' },
  { category: 'Utilities', title: 'Scheduled water supply maintenance', date: 'Sep 15, 2026' },
  { category: 'Community', title: 'Public city event this weekend', date: 'Sep 12, 2026' },
];

function formatDate(isoDate: string): string {
  return new Date(isoDate).toLocaleDateString('en-US', {
    month: 'short',
    day: 'numeric',
    year: 'numeric',
  });
}

function NewsCard({ item }: { item: ExternalNews }) {
  const [imageFailed, setImageFailed] = useState(false);
  const showImage = Boolean(item.image_url) && !imageFailed;

  const content = (
    <>
      <div className="news-card__image">
        {showImage ? (
          <img
            src={item.image_url ?? undefined}
            alt={item.title}
            className="news-card__image-img"
            onError={() => setImageFailed(true)}
          />
        ) : (
          <NewsIcon size={22} />
        )}
      </div>
      <span className="news-card__category">{item.category}</span>
      <h3>{item.title}</h3>
      <p>
        {formatDate(item.published_at)}
        {item.source_name ? ` · ${item.source_name}` : ''}
      </p>
    </>
  );

  if (item.source_url) {
    return (
      <a
        className="news-card news-card--link"
        href={item.source_url}
        target="_blank"
        rel="noopener noreferrer"
      >
        {content}
      </a>
    );
  }

  return <article className="news-card">{content}</article>;
}

function NewsPreview() {
  const navigate = useNavigate();
  const { news, isLoading, error } = useExternalNews();
  const latestNews = news.slice(0, 3);

  return (
    <section id="news" className="news-preview">
      <div className="news-preview__group">
        <div className="news-preview__header">
          <h2>Latest News</h2>
          <button type="button" onClick={() => navigate('/news')}>
            View all news →
          </button>
        </div>
        {isLoading ? (
          <div className="news-preview__grid">
            {[0, 1, 2].map((placeholder) => (
              <div className="news-card news-card--skeleton" key={placeholder} />
            ))}
          </div>
        ) : error ? (
          <p className="news-preview__status">Unable to load news right now.</p>
        ) : latestNews.length === 0 ? (
          <p className="news-preview__status">No news available at the moment.</p>
        ) : (
          <div className="news-preview__grid">
            {latestNews.map((item) => (
              <NewsCard item={item} key={item.id} />
            ))}
          </div>
        )}
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
