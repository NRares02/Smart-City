import { useEffect, useRef, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import type { AuthUser } from '../../types/auth';
import { BellIcon, MenuIcon, SearchIcon, SettingsIcon, UserIcon } from './icons';

interface DashboardHeaderProps {
  user: AuthUser | null;
  onMenuClick: () => void;
  onLogout: () => void;
}

function firstName(name: string | undefined): string {
  if (!name) {
    return 'Citizen';
  }
  return name.trim().split(/\s+/)[0];
}

function DashboardHeader({ user, onMenuClick, onLogout }: DashboardHeaderProps) {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const menuRef = useRef<HTMLDivElement>(null);
  const navigate = useNavigate();

  useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (menuRef.current && !menuRef.current.contains(event.target as Node)) {
        setIsMenuOpen(false);
      }
    }
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  return (
    <header className="dashboard-header">
      <div className="dashboard-header__left">
        <button
          type="button"
          className="dashboard-header__menu-btn"
          onClick={onMenuClick}
          aria-label="Toggle navigation"
        >
          <MenuIcon size={22} />
        </button>
        <div>
          <h1 className="dashboard-header__greeting">Hello, {firstName(user?.name)}</h1>
          <p className="dashboard-header__subtitle">
            Track your reports, explore city services and stay informed.
          </p>
        </div>
      </div>

      <div className="dashboard-header__right">
        <button type="button" className="dashboard-header__icon-btn" aria-label="Search">
          <SearchIcon size={19} />
        </button>
        <button type="button" className="dashboard-header__icon-btn" aria-label="Notifications">
          <BellIcon size={19} />
          <span className="dashboard-header__badge">3</span>
        </button>

        <div className="dashboard-header__user" ref={menuRef}>
          <button
            type="button"
            className="dashboard-header__user-btn"
            onClick={() => setIsMenuOpen((open) => !open)}
          >
            <span className="dashboard-header__avatar">
              <UserIcon size={17} />
            </span>
          </button>

          {isMenuOpen && (
            <div className="dashboard-header__dropdown">
              <p className="dashboard-header__dropdown-name">{user?.name ?? 'Citizen'}</p>
              <p className="dashboard-header__dropdown-email">{user?.email ?? ''}</p>
              <button type="button" onClick={() => { setIsMenuOpen(false); navigate('/profile'); }}>
                <UserIcon size={16} /> Profile
              </button>
              <button type="button" onClick={() => { setIsMenuOpen(false); navigate('/settings'); }}>
                <SettingsIcon size={16} /> Settings
              </button>
              <button type="button" className="is-danger" onClick={onLogout}>
                Logout
              </button>
            </div>
          )}
        </div>
      </div>
    </header>
  );
}

export default DashboardHeader;
