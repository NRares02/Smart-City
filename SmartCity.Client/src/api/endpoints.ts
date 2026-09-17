// Central registry of backend API routes (see SmartCity.Api Controllers)
export const ENDPOINTS = {
  auth: {
    register: '/api/auth/register',
    login: '/api/auth/login',
    me: '/api/auth/me',
  },
  citizenReports: '/api/citizen-reports',
  externalNews: '/api/external-news',
  notifications: '/api/notifications',
  officialAnnouncements: '/api/official-announcements',
  pointOfInterestCategories: '/api/point-of-interest-categories',
  pointsOfInterest: '/api/points-of-interest',
} as const;
