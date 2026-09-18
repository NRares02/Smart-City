import { BellIcon, MapIcon, MegaphoneIcon, NewsIcon, ProgressIcon, ReportsIcon } from '../dashboard/icons';

const FEATURES = [
  {
    icon: ReportsIcon,
    title: 'Report City Issues',
    description: 'Report infrastructure, lighting, sanitation and other local problems in minutes.',
  },
  {
    icon: ProgressIcon,
    title: 'Track Your Reports',
    description: 'Follow status changes from submission all the way through to resolution.',
  },
  {
    icon: MapIcon,
    title: 'Explore the City',
    description: 'Discover important places, services and points of interest around you.',
  },
  {
    icon: NewsIcon,
    title: 'Local News',
    description: 'Stay informed with relevant, up-to-date news from your community.',
  },
  {
    icon: MegaphoneIcon,
    title: 'Official Announcements',
    description: 'Receive important municipal updates and notices as soon as they are published.',
  },
  {
    icon: BellIcon,
    title: 'Smart Notifications',
    description: 'Get notified the moment something important changes near you.',
  },
];

function FeatureSection() {
  return (
    <section id="features" className="features">
      <div className="features__header">
        <p className="features__eyebrow">Platform</p>
        <h2>Built for a more connected city</h2>
      </div>

      <div className="features__grid">
        {FEATURES.map(({ icon: Icon, title, description }) => (
          <article className="feature-card" key={title}>
            <span className="feature-card__icon">
              <Icon size={22} />
            </span>
            <h3>{title}</h3>
            <p>{description}</p>
          </article>
        ))}
      </div>
    </section>
  );
}

export default FeatureSection;
