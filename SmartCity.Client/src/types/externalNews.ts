// Mirrors ExternalNewsController's anonymous ToResponse() shape (GET /api/external-news)
export interface ExternalNews {
  id: string;
  external_id: string;
  title: string;
  summary: string;
  image_url: string | null;
  source_name: string;
  source_url: string;
  category: string;
  published_at: string;
  expires_at: string | null;
  is_visible: boolean;
  inserted_at: string;
}
