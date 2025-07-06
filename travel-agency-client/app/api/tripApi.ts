import { AxiosPrivate } from "./axios";

export interface TripDto {
  id: number;
  tripDetail: string;
  createdAt: string; // ISO date string
  tripImageUrls: string[];
}

export const GetTripById = async (id: number) => {
  const res = await AxiosPrivate.get<TripDto | undefined>(`/Trip/GetTripById`, {
    params: {
      id,
    },
  });
  const trip = res.data;
  if (!trip) console.log("Trip not found");
  return trip;
};

export const GetAllTrips = async (limit: number, offset: number) => {
  const res = await AxiosPrivate.get<TripDto[]>("/Trip/GetAllTrips", {
    params: {
      limit,
      offset,
    },
  });
  const allTrips = res.data;

  if (allTrips.length === 0) {
    console.error("No trips found");
    return { allTrips: [], total: 0 };
  }

  return { allTrips: allTrips, total: allTrips.length };
};
