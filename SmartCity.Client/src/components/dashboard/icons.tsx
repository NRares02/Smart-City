interface IconProps {
  size?: number;
  className?: string;
}

const base = {
  viewBox: '0 0 24 24',
  fill: 'none' as const,
  'aria-hidden': true as const,
};

export function HomeIcon({ size = 20, className }: IconProps) {
  return (
    <svg {...base} width={size} height={size} className={className}>
      <path
        d="M4 10.5 12 4l8 6.5V19a1 1 0 0 1-1 1h-4.5v-6h-5v6H5a1 1 0 0 1-1-1v-8.5Z"
        stroke="currentColor"
        strokeWidth="1.7"
        strokeLinejoin="round"
      />
    </svg>
  );
}

export function ReportsIcon({ size = 20, className }: IconProps) {
  return (
    <svg {...base} width={size} height={size} className={className}>
      <path
        d="M7 3.5h7.5L19 8v12a1 1 0 0 1-1 1H7a1 1 0 0 1-1-1V4.5a1 1 0 0 1 1-1Z"
        stroke="currentColor"
        strokeWidth="1.7"
        strokeLinejoin="round"
      />
      <path d="M14 3.5V8h5M9 12.5h6M9 15.8h6M9 19h3.5" stroke="currentColor" strokeWidth="1.6" strokeLinecap="round" />
    </svg>
  );
}

export function MapIcon({ size = 20, className }: IconProps) {
  return (
    <svg {...base} width={size} height={size} className={className}>
      <path
        d="M9 4 4 6v14l5-2 6 2 5-2V4l-5 2-6-2Z"
        stroke="currentColor"
        strokeWidth="1.7"
        strokeLinejoin="round"
      />
      <path d="M9 4v14M15 6v14" stroke="currentColor" strokeWidth="1.6" />
    </svg>
  );
}

export function NewsIcon({ size = 20, className }: IconProps) {
  return (
    <svg {...base} width={size} height={size} className={className}>
      <path
        d="M4.5 5.5h12a1 1 0 0 1 1 1V17a2 2 0 0 0 2 2H6.5a2 2 0 0 1-2-2V5.5Z"
        stroke="currentColor"
        strokeWidth="1.7"
        strokeLinejoin="round"
      />
      <path d="M17.5 8.5H19a.5.5 0 0 1 .5.5v8a1 1 0 0 1-1 1" stroke="currentColor" strokeWidth="1.6" />
      <path d="M7.5 9h6M7.5 12h6M7.5 15h4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
    </svg>
  );
}

export function MegaphoneIcon({ size = 20, className }: IconProps) {
  return (
    <svg {...base} width={size} height={size} className={className}>
      <path
        d="M3.5 10v4a1 1 0 0 0 1 1H7l1.2 4.2a1 1 0 0 0 1 .8H10l-1-5h2l7 3.5V6L11 9.5H9.5L3.5 10Z"
        stroke="currentColor"
        strokeWidth="1.6"
        strokeLinejoin="round"
      />
    </svg>
  );
}

export function BellIcon({ size = 20, className }: IconProps) {
  return (
    <svg {...base} width={size} height={size} className={className}>
      <path
        d="M6 10a6 6 0 0 1 12 0v4l1.5 3h-15L6 14v-4Z"
        stroke="currentColor"
        strokeWidth="1.7"
        strokeLinejoin="round"
      />
      <path d="M9.5 19.5a2.5 2.5 0 0 0 5 0" stroke="currentColor" strokeWidth="1.7" strokeLinecap="round" />
    </svg>
  );
}

export function UserIcon({ size = 20, className }: IconProps) {
  return (
    <svg {...base} width={size} height={size} className={className}>
      <circle cx="12" cy="8" r="3.5" stroke="currentColor" strokeWidth="1.7" />
      <path d="M4.5 20a7.5 7.5 0 0 1 15 0" stroke="currentColor" strokeWidth="1.7" strokeLinecap="round" />
    </svg>
  );
}

export function SettingsIcon({ size = 20, className }: IconProps) {
  return (
    <svg {...base} width={size} height={size} className={className}>
      <circle cx="12" cy="12" r="3" stroke="currentColor" strokeWidth="1.7" />
      <path
        d="M19.4 13.5a1.7 1.7 0 0 0 .3 1.9l.1.1a2 2 0 1 1-2.8 2.8l-.1-.1a1.7 1.7 0 0 0-1.9-.3 1.7 1.7 0 0 0-1 1.6v.2a2 2 0 1 1-4 0v-.1a1.7 1.7 0 0 0-1.1-1.6 1.7 1.7 0 0 0-1.9.3l-.1.1a2 2 0 1 1-2.8-2.8l.1-.1a1.7 1.7 0 0 0 .3-1.9 1.7 1.7 0 0 0-1.6-1h-.2a2 2 0 1 1 0-4h.1a1.7 1.7 0 0 0 1.6-1.1 1.7 1.7 0 0 0-.3-1.9l-.1-.1a2 2 0 1 1 2.8-2.8l.1.1a1.7 1.7 0 0 0 1.9.3h.1a1.7 1.7 0 0 0 1-1.6v-.2a2 2 0 1 1 4 0v.1a1.7 1.7 0 0 0 1 1.6h.1a1.7 1.7 0 0 0 1.9-.3l.1-.1a2 2 0 1 1 2.8 2.8l-.1.1a1.7 1.7 0 0 0-.3 1.9v.1a1.7 1.7 0 0 0 1.6 1h.2a2 2 0 1 1 0 4h-.1a1.7 1.7 0 0 0-1.6 1Z"
        stroke="currentColor"
        strokeWidth="1.2"
        strokeLinejoin="round"
      />
    </svg>
  );
}

export function LogoutIcon({ size = 20, className }: IconProps) {
  return (
    <svg {...base} width={size} height={size} className={className}>
      <path
        d="M9 4.5H6a1 1 0 0 0-1 1V18.5a1 1 0 0 0 1 1h3"
        stroke="currentColor"
        strokeWidth="1.7"
        strokeLinecap="round"
      />
      <path d="M14 8.5 18 12l-4 3.5M18 12H9" stroke="currentColor" strokeWidth="1.7" strokeLinecap="round" strokeLinejoin="round" />
    </svg>
  );
}

export function SearchIcon({ size = 20, className }: IconProps) {
  return (
    <svg {...base} width={size} height={size} className={className}>
      <circle cx="11" cy="11" r="6.5" stroke="currentColor" strokeWidth="1.7" />
      <path d="m20 20-3.8-3.8" stroke="currentColor" strokeWidth="1.7" strokeLinecap="round" />
    </svg>
  );
}

export function ChevronRightIcon({ size = 16, className }: IconProps) {
  return (
    <svg {...base} width={size} height={size} className={className}>
      <path d="m9 5 7 7-7 7" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round" />
    </svg>
  );
}

export function ClipboardIcon({ size = 20, className }: IconProps) {
  return (
    <svg {...base} width={size} height={size} className={className}>
      <path d="M8 4.5h8v3H8z" stroke="currentColor" strokeWidth="1.6" strokeLinejoin="round" />
      <path
        d="M8 6H6a1 1 0 0 0-1 1v12a1 1 0 0 0 1 1h12a1 1 0 0 0 1-1V7a1 1 0 0 0-1-1h-2"
        stroke="currentColor"
        strokeWidth="1.6"
        strokeLinejoin="round"
      />
      <path d="M8.5 12.5h7M8.5 15.8h7M8.5 19h4" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
    </svg>
  );
}

export function ProgressIcon({ size = 20, className }: IconProps) {
  return (
    <svg {...base} width={size} height={size} className={className}>
      <circle cx="12" cy="12" r="8" stroke="currentColor" strokeWidth="1.7" />
      <path d="M12 8v4l3 2" stroke="currentColor" strokeWidth="1.7" strokeLinecap="round" strokeLinejoin="round" />
    </svg>
  );
}

export function CheckCircleIcon({ size = 20, className }: IconProps) {
  return (
    <svg {...base} width={size} height={size} className={className}>
      <circle cx="12" cy="12" r="8" stroke="currentColor" strokeWidth="1.7" />
      <path d="m8.5 12.3 2.4 2.4 4.6-5" stroke="currentColor" strokeWidth="1.7" strokeLinecap="round" strokeLinejoin="round" />
    </svg>
  );
}

export function BellRingIcon({ size = 20, className }: IconProps) {
  return (
    <svg {...base} width={size} height={size} className={className}>
      <path
        d="M6 10a6 6 0 0 1 12 0v4l1.5 3h-15L6 14v-4Z"
        stroke="currentColor"
        strokeWidth="1.7"
        strokeLinejoin="round"
      />
      <path d="M4 5.5c-.9.9-1.5 1.9-1.8 3M20 5.5c.9.9 1.5 1.9 1.8 3" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
    </svg>
  );
}

export function MapPinIcon({ size = 20, className }: IconProps) {
  return (
    <svg {...base} width={size} height={size} className={className}>
      <path
        d="M12 21s7-6.6 7-11.5A7 7 0 0 0 5 9.5C5 14.4 12 21 12 21Z"
        stroke="currentColor"
        strokeWidth="1.7"
        strokeLinejoin="round"
      />
      <circle cx="12" cy="9.5" r="2.3" stroke="currentColor" strokeWidth="1.6" />
    </svg>
  );
}

export function CalendarIcon({ size = 20, className }: IconProps) {
  return (
    <svg {...base} width={size} height={size} className={className}>
      <rect x="4" y="5.5" width="16" height="14.5" rx="2" stroke="currentColor" strokeWidth="1.6" />
      <path d="M4 9.5h16M8 3.5v3M16 3.5v3" stroke="currentColor" strokeWidth="1.6" strokeLinecap="round" />
    </svg>
  );
}

export function MenuIcon({ size = 20, className }: IconProps) {
  return (
    <svg {...base} width={size} height={size} className={className}>
      <path d="M4 6.5h16M4 12h16M4 17.5h16" stroke="currentColor" strokeWidth="1.7" strokeLinecap="round" />
    </svg>
  );
}

export function CloseIcon({ size = 20, className }: IconProps) {
  return (
    <svg {...base} width={size} height={size} className={className}>
      <path d="m6 6 12 12M18 6 6 18" stroke="currentColor" strokeWidth="1.8" strokeLinecap="round" />
    </svg>
  );
}
