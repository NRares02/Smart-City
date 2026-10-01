// Response shapes mirror the anonymous objects returned by PointsOfInterestController /
// PointOfInterestCategoriesController exactly (snake_case keys).
export interface PointOfInterestLocation {
  longitude: number | null;
  latitude: number | null;
}

export interface PointOfInterest {
  id: string;
  category_id: string;
  name: string;
  description: string | null;
  location: PointOfInterestLocation;
  address: string | null;
  image_url: string | null;
  phone_number: string | null;
  website_url: string | null;
  opening_hours: string | null;
  valid_from: string | null;
  valid_until: string | null;
  is_temporary: boolean;
  is_active: boolean;
  inserted_at: string;
}

export interface PointOfInterestCategory {
  id: string;
  name: string;
  description: string | null;
  icon: string | null;
  marker_color: string | null;
  display_order: number;
}
