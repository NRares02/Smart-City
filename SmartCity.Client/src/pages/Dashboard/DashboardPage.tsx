import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { clearSession } from '../../services/authService';
import { getStoredUser } from '../../utils/storage';
import DashboardSidebar from '../../components/dashboard/DashboardSidebar';
import DashboardHeader from '../../components/dashboard/DashboardHeader';
import StatCard from '../../components/dashboard/StatCard';
import QuickActions from '../../components/dashboard/QuickActions';
import RecentReports from '../../components/dashboard/RecentReports';
import OfficialAnnouncements from '../../components/dashboard/OfficialAnnouncements';
import ActivityPanel from '../../components/dashboard/ActivityPanel';
import CityMapPreview from '../../components/dashboard/CityMapPreview';
import { BellRingIcon, CheckCircleIcon, ProgressIcon, ReportsIcon } from '../../components/dashboard/icons';
import './DashboardPage.css';

function DashboardPage() {
  const navigate = useNavigate();
  const user = getStoredUser();
  const [isSidebarOpen, setIsSidebarOpen] = useState(false);

  function handleLogout() {
    clearSession();
    navigate('/auth', { replace: true });
  }

  return (
    <div className="dashboard">
      <DashboardSidebar
        user={user}
        isOpen={isSidebarOpen}
        onClose={() => setIsSidebarOpen(false)}
        onLogout={handleLogout}
      />

      <div className="dashboard__main">
        <DashboardHeader
          user={user}
          onMenuClick={() => setIsSidebarOpen((open) => !open)}
          onLogout={handleLogout}
        />

        <div className="dashboard__content">
          <section className="stat-cards">
            {/* Temporary placeholder values until backend metrics are wired up */}
            <StatCard icon={<ReportsIcon size={22} />} value={12} label="My Reports" helperText="Total submitted" accent="navy" />
            <StatCard icon={<ProgressIcon size={22} />} value={4} label="In Progress" helperText="Being handled" accent="blue" />
            <StatCard icon={<CheckCircleIcon size={22} />} value={6} label="Resolved" helperText="Closed successfully" accent="green" />
            <StatCard icon={<BellRingIcon size={22} />} value={3} label="New Notifications" helperText="Unread updates" accent="amber" />
          </section>

          <QuickActions />
          <RecentReports />
          <OfficialAnnouncements />
          <CityMapPreview />
        </div>
      </div>

      <ActivityPanel />
    </div>
  );
}

export default DashboardPage;
