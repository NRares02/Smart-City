import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import smartCityLogo from '../../images/logo_smartcity.png';
import { isAuthenticated } from '../../utils/storage';
import { CloseIcon, MenuIcon } from '../dashboard/icons';

const NAV_LINKS = [
  { href: '#top', label: 'Home' },
  { href: '#explore', label: 'Explore' },
  { href: '#news', label: 'News' },
  { href: '#about', label: 'About' },
];

interface PublicNavbarProps {
  // For pages with no hero image behind the nav (e.g. /news) — forces the opaque/dark-text style.
  solid?: boolean;
}

function PublicNavbar({ solid = false }: PublicNavbarProps) {
  const navigate = useNavigate();
  const authed = isAuthenticated();
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const [isScrolled, setIsScrolled] = useState(solid);

  useEffect(() => {
    if (solid) return;
    function handleScroll() {
      setIsScrolled(window.scrollY > 40);
    }
    handleScroll();
    window.addEventListener('scroll', handleScroll, { passive: true });
    return () => window.removeEventListener('scroll', handleScroll);
  }, [solid]);

  function handleNavClick() {
    setIsMenuOpen(false);
  }

  return (
    <header className={isScrolled ? 'public-nav is-scrolled' : 'public-nav'}>
      <div className="public-nav__inner">
        <a href="#top" className="public-nav__brand" onClick={handleNavClick}>
          <img src={smartCityLogo} alt="Smart City" />
          <span>Smart City</span>
        </a>

        <nav className="public-nav__links">
          {NAV_LINKS.map((link) => (
            <a key={link.href} href={link.href} onClick={handleNavClick}>
              {link.label}
            </a>
          ))}
        </nav>

        <div className="public-nav__actions">
          {authed ? (
            <button type="button" className="public-nav__cta" onClick={() => navigate('/dashboard')}>
              Go to Dashboard
            </button>
          ) : (
            <>
              <button type="button" className="public-nav__ghost" onClick={() => navigate('/auth')}>
                Sign In
              </button>
              <button type="button" className="public-nav__cta" onClick={() => navigate('/auth')}>
                Get Started
              </button>
            </>
          )}
        </div>

        <button
          type="button"
          className="public-nav__menu-btn"
          aria-label="Toggle navigation"
          onClick={() => setIsMenuOpen((open) => !open)}
        >
          {isMenuOpen ? <CloseIcon size={22} /> : <MenuIcon size={22} />}
        </button>
      </div>

      {isMenuOpen && (
        <div className="public-nav__mobile">
          {NAV_LINKS.map((link) => (
            <a key={link.href} href={link.href} onClick={handleNavClick}>
              {link.label}
            </a>
          ))}
          <div className="public-nav__mobile-actions">
            {authed ? (
              <button
                type="button"
                className="public-nav__cta"
                onClick={() => {
                  handleNavClick();
                  navigate('/dashboard');
                }}
              >
                Go to Dashboard
              </button>
            ) : (
              <>
                <button
                  type="button"
                  className="public-nav__ghost"
                  onClick={() => {
                    handleNavClick();
                    navigate('/auth');
                  }}
                >
                  Sign In
                </button>
                <button
                  type="button"
                  className="public-nav__cta"
                  onClick={() => {
                    handleNavClick();
                    navigate('/auth');
                  }}
                >
                  Get Started
                </button>
              </>
            )}
          </div>
        </div>
      )}
    </header>
  );
}

export default PublicNavbar;
