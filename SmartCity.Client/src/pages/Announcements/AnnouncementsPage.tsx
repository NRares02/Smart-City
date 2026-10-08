import { useMemo, useState } from 'react';
import PublicNavbar from '../../components/home/PublicNavbar';
import PublicFooter from '../../components/home/PublicFooter';
import { useOfficialAnnouncements } from '../../hooks/useOfficialAnnouncements';
import type { OfficialAnnouncement } from '../../types/officialAnnouncement';
import { MegaphoneIcon } from '../../components/dashboard/icons';
import './AnnouncementsPage.css';

const MAX_ANNOUNCEMENTS = 9;

function formatDate(isoDate: string): string {
  return new Date(isoDate).toLocaleDateString('en-US', {
    month: 'short',
    day: 'numeric',
    year: 'numeric',
  });
}

function AnnouncementImage({ item }: { item: OfficialAnnouncement }) {
  const [imageFailed, setImageFailed] = useState(false);
  const showImage = Boolean(item.image_url) && !imageFailed;

  return (
    <div className="announcement-card__image">
      {showImage ? (
        <img src={item.image_url ?? undefined} alt={item.title} onError={() => setImageFailed(true)} />
      ) : (
        <MegaphoneIcon size={28} />
      )}
    </div>
  );
}

function AnnouncementCard({ item }: { item: OfficialAnnouncement }) {
  const [isExpanded, setIsExpanded] = useState(false);
  const hasMoreContent = item.content.trim().length > item.summary.trim().length;

  return (
    <article className="announcement-card">
      <AnnouncementImage item={item} />
      <div className="announcement-card__body">
        <div className="announcement-card__tags">
          {item.is_pinned && <span className="announcement-card__pinned">Pinned</span>}
          <span className="announcement-card__category">{item.category}</span>
        </div>
        <h3>{item.title}</h3>
        <p className="announcement-card__date">{formatDate(item.inserted_at)}</p>
        <p className="announcement-card__summary">{item.summary}</p>

        {isExpanded && <p className="announcement-card__content">{item.content}</p>}

        {hasMoreContent && (
          <button
            type="button"
            className="announcement-card__toggle"
            onClick={() => setIsExpanded((expanded) => !expanded)}
          >
            {isExpanded ? 'Show less' : 'Read more'}
          </button>
        )}
      </div>
    </article>
  );
}

function AnnouncementsPageSkeleton() {
  return (
    <div className="announcements-page__grid">
      {[0, 1, 2, 3, 4, 5].map((placeholder) => (
        <div className="announcement-skeleton" key={placeholder} />
      ))}
    </div>
  );
}

function AnnouncementsPage() {
  const { announcements, isLoading, error } = useOfficialAnnouncements();

  // Backend already sorts pinned-first then newest-first; just cap at 9.
  const items = useMemo(() => announcements.slice(0, MAX_ANNOUNCEMENTS), [announcements]);

  return (
    <div className="announcements-page">
      <PublicNavbar solid />
      <main className="announcements-page__main">
        <header className="announcements-page__header">
          <h1>Official Announcements</h1>
        </header>

        {isLoading ? (
          <AnnouncementsPageSkeleton />
        ) : error ? (
          <p className="announcements-page__status announcements-page__status--error">{error}</p>
        ) : items.length === 0 ? (
          <p className="announcements-page__status">No announcements available at the moment.</p>
        ) : (
          <div className="announcements-page__grid">
            {items.map((item) => (
              <AnnouncementCard item={item} key={item.id} />
            ))}
          </div>
        )}
      </main>
      <PublicFooter />
    </div>
  );
}

export default AnnouncementsPage;
