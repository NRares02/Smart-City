// Mirrors OfficialAnnouncementsController's ToResponse() shape (GET /api/official-announcements)
export interface OfficialAnnouncement {
  id: string;
  title: string;
  summary: string;
  content: string;
  category: string;
  image_url: string | null;
  author_id: string;
  is_published: boolean;
  is_pinned: boolean;
  expires_at: string | null;
  inserted_at: string;
}
