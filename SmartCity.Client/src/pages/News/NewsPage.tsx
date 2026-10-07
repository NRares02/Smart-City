import { useMemo, useState } from 'react';
import PublicNavbar from '../../components/home/PublicNavbar';
import PublicFooter from '../../components/home/PublicFooter';
import { useExternalNews } from '../../hooks/useExternalNews';
import type { ExternalNews } from '../../types/externalNews';
import { NewsIcon } from '../../components/dashboard/icons';
import './NewsPage.css';

const MAX_ARTICLES = 9;

function formatDate(isoDate: string): string {
  return new Date(isoDate).toLocaleDateString('en-US', {
    month: 'short',
    day: 'numeric',
    year: 'numeric',
  });
}

function NewsThumbnail({ item, className }: { item: ExternalNews; className: string }) {
  const [imageFailed, setImageFailed] = useState(false);
  const showImage = Boolean(item.image_url) && !imageFailed;

  return (
    <div className={className}>
      {showImage ? (
        <img src={item.image_url ?? undefined} alt={item.title} onError={() => setImageFailed(true)} />
      ) : (
        <NewsIcon size={26} />
      )}
    </div>
  );
}

function ArticleMeta({ item }: { item: ExternalNews }) {
  return (
    <p className="news-article__meta">
      {formatDate(item.published_at)}
      {item.source_name ? ` · ${item.source_name}` : ''}
    </p>
  );
}

function FeaturedArticle({ item }: { item: ExternalNews }) {
  return (
    <a className="news-featured" href={item.source_url} target="_blank" rel="noopener noreferrer">
      <NewsThumbnail item={item} className="news-featured__image" />
      <div className="news-featured__body">
        <span className="news-article__category">{item.category}</span>
        <h2>{item.title}</h2>
        {item.summary && <p className="news-featured__summary">{item.summary}</p>}
        <ArticleMeta item={item} />
      </div>
    </a>
  );
}

function SecondaryArticle({ item }: { item: ExternalNews }) {
  return (
    <a className="news-secondary" href={item.source_url} target="_blank" rel="noopener noreferrer">
      <NewsThumbnail item={item} className="news-secondary__image" />
      <div className="news-secondary__body">
        <span className="news-article__category">{item.category}</span>
        <h3>{item.title}</h3>
        {item.summary && <p className="news-secondary__summary">{item.summary}</p>}
        <ArticleMeta item={item} />
      </div>
    </a>
  );
}

function GridArticle({ item }: { item: ExternalNews }) {
  return (
    <a className="news-grid-card" href={item.source_url} target="_blank" rel="noopener noreferrer">
      <NewsThumbnail item={item} className="news-grid-card__image" />
      <div className="news-grid-card__body">
        <span className="news-article__category">{item.category}</span>
        <h3>{item.title}</h3>
        <ArticleMeta item={item} />
      </div>
    </a>
  );
}

function NewsPageSkeleton() {
  return (
    <div className="news-page__skeleton">
      <div className="news-skeleton news-skeleton--featured" />
      <div className="news-skeleton-row">
        <div className="news-skeleton" />
        <div className="news-skeleton" />
      </div>
      <div className="news-skeleton-grid">
        {[0, 1, 2, 3, 4, 5].map((placeholder) => (
          <div className="news-skeleton" key={placeholder} />
        ))}
      </div>
    </div>
  );
}

function NewsPage() {
  const { news, isLoading, error } = useExternalNews();

  // Backend already sorts newest-first, but re-sort defensively before trimming to 9.
  const articles = useMemo(
    () =>
      [...news]
        .sort((a, b) => new Date(b.published_at).getTime() - new Date(a.published_at).getTime())
        .slice(0, MAX_ARTICLES),
    [news],
  );

  const [featured, ...rest] = articles;
  const secondary = rest.slice(0, 2);
  const gridItems = rest.slice(2, 8);

  return (
    <div className="news-page">
      <PublicNavbar solid />
      <main className="news-page__main">
        <header className="news-page__header">
          <h1>Latest News</h1>
        </header>

        {isLoading ? (
          <NewsPageSkeleton />
        ) : error ? (
          <p className="news-page__status news-page__status--error">{error}</p>
        ) : articles.length === 0 ? (
          <p className="news-page__status">No news available at the moment.</p>
        ) : (
          <>
            {featured && <FeaturedArticle item={featured} />}

            {secondary.length > 0 && (
              <div className="news-secondary-row">
                {secondary.map((item) => (
                  <SecondaryArticle item={item} key={item.id} />
                ))}
              </div>
            )}

            {gridItems.length > 0 && (
              <div className="news-grid">
                {gridItems.map((item) => (
                  <GridArticle item={item} key={item.id} />
                ))}
              </div>
            )}
          </>
        )}
      </main>
      <PublicFooter />
    </div>
  );
}

export default NewsPage;
