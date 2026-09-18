import { BellRingIcon, CalendarIcon, CheckCircleIcon, MegaphoneIcon, ProgressIcon } from './icons';

const ACTIVITY_ITEMS = [
  { id: '1', icon: ProgressIcon, text: 'Your report status changed to In Progress', time: '2 hours ago' },
  { id: '2', icon: MegaphoneIcon, text: 'New official announcement published', time: '5 hours ago' },
  { id: '3', icon: CheckCircleIcon, text: 'A nearby city issue was resolved', time: 'Yesterday' },
];

const UPCOMING_EVENTS = [
  { id: '1', title: 'Community meeting', date: 'Sep 22, 2026' },
  { id: '2', title: 'Road maintenance', date: 'Sep 24, 2026' },
  { id: '3', title: 'City event at Riverside Park', date: 'Sep 27, 2026' },
];

function ActivityPanel() {
  return (
    <aside className="dashboard-activity">
      <section className="dashboard-panel">
        <div className="dashboard-panel__header">
          <h2 className="dashboard-panel__title">Recent Activity</h2>
          <BellRingIcon size={18} className="dashboard-panel__title-icon" />
        </div>
        <ul className="activity-list">
          {ACTIVITY_ITEMS.map(({ id, icon: Icon, text, time }) => (
            <li className="activity-list__item" key={id}>
              <span className="activity-list__icon">
                <Icon size={16} />
              </span>
              <span className="activity-list__body">
                <span className="activity-list__text">{text}</span>
                <span className="activity-list__time">{time}</span>
              </span>
            </li>
          ))}
        </ul>
      </section>

      <section className="dashboard-panel">
        <div className="dashboard-panel__header">
          <h2 className="dashboard-panel__title">Upcoming City Events</h2>
          <CalendarIcon size={18} className="dashboard-panel__title-icon" />
        </div>
        <ul className="events-list">
          {UPCOMING_EVENTS.map((event) => (
            <li className="events-list__item" key={event.id}>
              <span className="events-list__title">{event.title}</span>
              <span className="events-list__date">{event.date}</span>
            </li>
          ))}
        </ul>
      </section>
    </aside>
  );
}

export default ActivityPanel;
