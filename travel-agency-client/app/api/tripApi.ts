import { AxiosPrivate } from "./axios";

export interface TripDto {
  id: number;
  tripDetail: string;
  createdAt: string; // ISO date string
  tripImageUrls: string[];
}

type TripDtoKeysType = {
  readonly [K in keyof TripDto]: K;
};

export const TRIP_DTO_KEYS_TYPED: TripDtoKeysType = {
  id: "id",
  tripDetail: "tripDetail",
  createdAt: "createdAt",
  tripImageUrls: "tripImageUrls",
};

export const getTripById = async (id: number) => {
  const res = await AxiosPrivate.get<TripDto | undefined>(`/Trip/GetTripById`, {
    params: {
      id,
    },
  });
  const trip = res.data;
  if (!trip) console.log("Trip not found");
  return trip;
};

export const getTotalTripsCount = async () => {
  const res = await AxiosPrivate.get<number>("/Trip/GetTotalTripsCount");
  return res.data;
};

export const getAllTrips = async (limit: number, offset: number) => {
  const res = await AxiosPrivate.get<TripDto[]>("/Trip/GetAllTrips", {
    params: {
      limit,
      offset,
    },
  });
  const allTrips = res.data;

  if (allTrips.length === 0) {
    console.error("No trips found");
    return { allTrips: [] };
  }

  return {
    allTrips: allTrips,
  };
};
