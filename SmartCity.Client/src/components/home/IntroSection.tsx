import { BellIcon, MapIcon } from '../dashboard/icons';

const HIGHLIGHTS = [
  {
    icon: MapIcon,
    title: 'City services at a glance',
    description: 'Find points of interest, public services and useful locations near you.',
  },
  {
    icon: BellIcon,
    title: 'Always up to date',
    description: 'Get official announcements and smart notifications as things happen.',
  },
];

function IntroSection() {
  return (
    <section id="about" className="intro">
      <div className="intro__col">
        <p className="intro__eyebrow">Smarter Urban Living</p>
        <h2 className="intro__heading">Everything you need to stay connected with your city.</h2>
        <p className="intro__text">
          Smart City brings citizen reporting, official updates, local news and city services
          together in one simple platform, so you always know what is happening around you.
        </p>
      </div>

      <div className="intro__col intro__highlights">
        {HIGHLIGHTS.map(({ icon: Icon, title, description }) => (
          <div className="intro-highlight" key={title}>
            <span className="intro-highlight__icon">
              <Icon size={20} />
            </span>
            <div>
              <h3>{title}</h3>
              <p>{description}</p>
            </div>
          </div>
        ))}
      </div>
    </section>
  );
}

export default IntroSection;
