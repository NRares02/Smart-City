import { NavLink } from 'react-router-dom';
import type { AuthUser } from '../../types/auth';
import smartCityLogo from '../../images/logo2_smartcity.png';
import {
  BellIcon,
  HomeIcon,
  LogoutIcon,
  MapIcon,
  MegaphoneIcon,
  NewsIcon,
  ReportsIcon,
  SettingsIcon,
  UserIcon,
} from './icons';

interface DashboardSidebarProps {
  user: AuthUser | null;
  isOpen: boolean;
  onClose: () => void;
  onLogout: () => void;
}

const NAV_ITEMS = [
  { to: '/dashboard', label: 'Dashboard', icon: HomeIcon, end: true },
  { to: '/reports', label: 'My Reports', icon: ReportsIcon },
  { to: '/map', label: 'Map', icon: MapIcon },
  { to: '/news', label: 'News', icon: NewsIcon },
  { to: '/official-announcements', label: 'Official Announcements', icon: MegaphoneIcon },
  { to: '/notifications', label: 'Notifications', icon: BellIcon },
  { to: '/profile', label: 'Profile', icon: UserIcon },
  { to: '/settings', label: 'Settings', icon: SettingsIcon },
];

function initials(name: string | undefined): string {
  if (!name) {
    return 'SC';
  }
  const parts = name.trim().split(/\s+/);
  const letters = parts.slice(0, 2).map((part) => part[0]?.toUpperCase() ?? '');
  return letters.join('') || 'SC';
}

function DashboardSidebar({ user, isOpen, onClose, onLogout }: DashboardSidebarProps) {
  return (
    <>
      <div
        className={isOpen ? 'dashboard-sidebar__scrim is-visible' : 'dashboard-sidebar__scrim'}
        onClick={onClose}
        aria-hidden="true"
      />
      <aside className={isOpen ? 'dashboard-sidebar is-open' : 'dashboard-sidebar'}>
        <div className="dashboard-sidebar__brand">
          <img src={smartCityLogo} alt="Smart City" />
          <span>Smart City</span>
        </div>

        <div className="dashboard-sidebar__profile">
          <div className="dashboard-sidebar__avatar">{initials(user?.name)}</div>
          <div className="dashboard-sidebar__profile-info">
            <p className="dashboard-sidebar__name">{user?.name ?? 'Citizen'}</p>
            <p className="dashboard-sidebar__email">{user?.email ?? ''}</p>
            <span className="dashboard-sidebar__role">Citizen</span>
          </div>
        </div>

        <nav className="dashboard-sidebar__nav">
          {NAV_ITEMS.map(({ to, label, icon: Icon, end }) => (
            <NavLink
              key={to}
              to={to}
              end={end}
              onClick={onClose}
              className={({ isActive }) =>
                isActive ? 'dashboard-sidebar__link is-active' : 'dashboard-sidebar__link'
              }
            >
              <Icon size={18} />
              <span>{label}</span>
            </NavLink>
          ))}
        </nav>

        <button type="button" className="dashboard-sidebar__logout" onClick={onLogout}>
          <LogoutIcon size={18} />
          <span>Logout</span>
        </button>
      </aside>
    </>
  );
}

export default DashboardSidebar;
