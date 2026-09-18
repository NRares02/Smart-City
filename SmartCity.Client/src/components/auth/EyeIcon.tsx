interface EyeIconProps {
  open: boolean;
}

// Single toggle icon: open eye when password is visible, crossed-out eye when hidden.
function EyeIcon({ open }: EyeIconProps) {
  if (open) {
    return (
      <svg viewBox="0 0 24 24" width="18" height="18" fill="none" aria-hidden="true">
        <path
          d="M1.5 12S5 5 12 5s10.5 7 10.5 7-3.5 7-10.5 7S1.5 12 1.5 12Z"
          stroke="currentColor"
          strokeWidth="1.6"
          strokeLinecap="round"
          strokeLinejoin="round"
        />
        <circle cx="12" cy="12" r="3" stroke="currentColor" strokeWidth="1.6" />
      </svg>
    );
  }

  return (
    <svg viewBox="0 0 24 24" width="18" height="18" fill="none" aria-hidden="true">
      <path
        d="M3.5 3.5l17 17M9.9 5.5A11.8 11.8 0 0 1 12 5c7 0 10.5 7 10.5 7a15.6 15.6 0 0 1-3.3 4.2M6.6 6.7C3.9 8.3 1.5 12 1.5 12s3.5 7 10.5 7a10.6 10.6 0 0 0 4.2-.9M14.1 14.1a3 3 0 0 1-4.2-4.2"
        stroke="currentColor"
        strokeWidth="1.6"
        strokeLinecap="round"
        strokeLinejoin="round"
      />
    </svg>
  );
}

export default EyeIcon;
