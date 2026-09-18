import type { ReactNode } from 'react';

export type StatAccent = 'blue' | 'amber' | 'green' | 'navy';

interface StatCardProps {
  icon: ReactNode;
  value: string | number;
  label: string;
  helperText: string;
  accent: StatAccent;
}

function StatCard({ icon, value, label, helperText, accent }: StatCardProps) {
  return (
    <div className="stat-card">
      <div className={`stat-card__icon stat-card__icon--${accent}`}>{icon}</div>
      <div className="stat-card__body">
        <p className="stat-card__value">{value}</p>
        <p className="stat-card__label">{label}</p>
        <p className="stat-card__helper">{helperText}</p>
      </div>
    </div>
  );
}

export default StatCard;
