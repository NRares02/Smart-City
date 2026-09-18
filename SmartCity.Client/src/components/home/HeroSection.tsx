import { useNavigate } from 'react-router-dom';
import { isAuthenticated } from '../../utils/storage';

function HeroSection() {
  const navigate = useNavigate();
  const authed = isAuthenticated();

  function handleGetStarted() {
    navigate(authed ? '/dashboard' : '/auth');
  }

  return (
    <section id="top" className="hero">
      <div className="hero__stars" aria-hidden="true" />
      <div className="hero__overlay" />
      <div className="hero__content">
        <p className="hero__eyebrow">Smart City Platform</p>
        <h1 className="hero__title">A smarter city, connected to you.</h1>
        <p className="hero__subtitle">
          Report local issues, explore city services and stay informed with the latest updates
          from your community.
        </p>
        <div className="hero__actions">
          <button type="button" className="hero__primary-btn" onClick={handleGetStarted}>
            Get Started
          </button>
          <a href="#explore" className="hero__secondary-btn">
            Explore the City
          </a>
        </div>
      </div>
    </section>
  );
}

export default HeroSection;
