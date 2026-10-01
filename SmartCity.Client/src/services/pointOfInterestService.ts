import axiosClient from '../api/axiosClient';
import { ENDPOINTS } from '../api/endpoints';
import type { PointOfInterest, PointOfInterestCategory } from '../types/pointOfInterest';

export async function getPointsOfInterest(): Promise<PointOfInterest[]> {
  const { data } = await axiosClient.get<PointOfInterest[]>(ENDPOINTS.pointsOfInterest);
  return data;
}

export async function getPointOfInterestCategories(): Promise<PointOfInterestCategory[]> {
  const { data } = await axiosClient.get<PointOfInterestCategory[]>(ENDPOINTS.pointOfInterestCategories);
  return data;
}
