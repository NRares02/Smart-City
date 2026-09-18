import { Link } from 'react-router-dom';
import { ChevronRightIcon } from './icons';

type ReportStatus = 'Pending' | 'In Progress' | 'Resolved' | 'Rejected';

interface RecentReport {
  id: string;
  title: string;
  category: string;
  date: string;
  status: ReportStatus;
}

// Temporary placeholder data until the reports list endpoint is wired into the dashboard.
const RECENT_REPORTS: RecentReport[] = [
  { id: '1', title: 'Pothole on Main Street', category: 'Road Infrastructure', date: 'Sep 18, 2026', status: 'In Progress' },
  { id: '2', title: 'Broken streetlight near Central Park', category: 'Public Lighting', date: 'Sep 16, 2026', status: 'Pending' },
  { id: '3', title: 'Overflowing trash bin on 5th Ave', category: 'Sanitation', date: 'Sep 12, 2026', status: 'Resolved' },
  { id: '4', title: 'Damaged bus stop shelter', category: 'Public Transport', date: 'Sep 9, 2026', status: 'In Progress' },
];

function StatusBadge({ status }: { status: ReportStatus }) {
  const className = `status-badge status-badge--${status.toLowerCase().replace(' ', '-')}`;
  return <span className={className}>{status}</span>;
}

function RecentReports() {
  return (
    <section className="dashboard-panel">
      <div className="dashboard-panel__header">
        <h2 className="dashboard-panel__title">Recent Reports</h2>
        <Link to="/reports" className="dashboard-panel__link">
          View all reports <ChevronRightIcon size={15} />
        </Link>
      </div>

      <div className="report-list">
        <div className="report-list__row report-list__row--head">
          <span>Title</span>
          <span>Category</span>
          <span>Date</span>
          <span>Status</span>
        </div>
        {RECENT_REPORTS.map((report) => (
          <div className="report-list__row" key={report.id}>
            <span className="report-list__title">{report.title}</span>
            <span className="report-list__muted">{report.category}</span>
            <span className="report-list__muted">{report.date}</span>
            <span>
              <StatusBadge status={report.status} />
            </span>
          </div>
        ))}
      </div>
    </section>
  );
}

export default RecentReports;
