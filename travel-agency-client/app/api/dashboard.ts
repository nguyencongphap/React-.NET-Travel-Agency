import { getAllUsers } from "~/routes/admin/all-users/allUsersApi";
import type { DashboardStats } from "..";
import {
  getAllTrips,
  getTotalTripsCount,
  TRIP_DTO_KEYS_TYPED,
  type TripDto,
} from "./tripApi";
import { USER_KEYS_TYPED } from "./authApi";
import { ROLE_NORMALIZED_NAMES } from "~/types/roleNames";
import { parseTripData } from "~/lib/utils";

interface Document {
  [key: string]: any;
}

// helps us know how many of what has happened on which day
type FilterByDate = (
  items: Document[],
  key: string,
  start: string,
  end?: string
) => number;

export const getUsersAndTripsStats = async (): Promise<DashboardStats> => {
  const d = new Date();
  const startCurrent = new Date(d.getFullYear(), d.getMonth(), 1).toISOString();
  const startPrev = new Date(
    d.getFullYear(),
    d.getMonth() - 1,
    1
  ).toISOString();
  const endPrev = new Date(d.getFullYear(), d.getMonth(), 0).toISOString();

  const totalTrips = await getTotalTripsCount();

  const [{ users, total: totalUsers }, { allTrips: trips }] = await Promise.all(
    [await getAllUsers(), await getAllTrips(totalTrips, 0)]
  );

  // Filter functions
  const filterByDate: FilterByDate = (items, key, start, end) =>
    items.filter((item) => item[key] >= start && (!end || item[key] <= end))
      .length;

  const filterUsersByRole = (role: ROLE_NORMALIZED_NAMES) => {
    return users.filter((u) => u.roles.includes(role));
  };

  return {
    totalUsers: totalUsers,
    usersJoined: {
      currentMonth: filterByDate(
        users,
        USER_KEYS_TYPED.dateJoined,
        startCurrent,
        undefined // no end for this period of time
      ),
      lastMonth: filterByDate(
        users,
        USER_KEYS_TYPED.dateJoined,
        startPrev, // start of previous month
        endPrev
      ),
    },
    userRole: {
      total: filterUsersByRole(ROLE_NORMALIZED_NAMES.User).length,
      currentMonth: filterByDate(
        filterUsersByRole(ROLE_NORMALIZED_NAMES.User),
        USER_KEYS_TYPED.dateJoined,
        startCurrent,
        undefined // no end for this period of time
      ),
      lastMonth: filterByDate(
        filterUsersByRole(ROLE_NORMALIZED_NAMES.User),
        USER_KEYS_TYPED.dateJoined,
        startPrev, // start of previous month
        endPrev
      ),
    },
    totalTrips: totalTrips,
    tripsCreated: {
      currentMonth: filterByDate(
        trips,
        TRIP_DTO_KEYS_TYPED.createdAt,
        startCurrent,
        undefined // no end for this period of time
      ),
      lastMonth: filterByDate(
        trips,
        TRIP_DTO_KEYS_TYPED.createdAt,
        startPrev, // start of previous month
        endPrev
      ),
    },
  };
};

export const getUserGrowthPerDay = async () => {
  const { users } = await getAllUsers();

  const userGrowth = users.reduce(
    (acc: { [key: string]: number }, user: Document) => {
      const date = new Date(user.dateJoined);
      const day = date.toLocaleDateString("en-US", {
        month: "short",
        day: "numeric",
      });
      acc[day] = (acc[day] || 0) + 1;
      return acc;
    },
    {}
  );

  return Object.entries(userGrowth).map(([day, count]) => ({
    count: Number(count),
    day,
  }));
};

export const getTripsCreatedPerDay = async () => {
  const totalTrips = await getTotalTripsCount();
  const { allTrips: trips } = await getAllTrips(totalTrips, 0);

  const tripsGrowth = trips.reduce(
    (acc: { [key: string]: number }, trip: Document) => {
      const date = new Date(trip.createdAt);
      const day = date.toLocaleDateString("en-US", {
        month: "short",
        day: "numeric",
      });
      acc[day] = (acc[day] || 0) + 1;
      return acc;
    },
    {}
  );

  return Object.entries(tripsGrowth).map(([day, count]) => ({
    count: Number(count),
    day,
  }));
};

export const getTripsByTravelStyle = async () => {
  const totalTrips = await getTotalTripsCount();
  const { allTrips: trips } = await getAllTrips(totalTrips, 0);

  const travelStyleCounts = trips.reduce(
    (acc: { [key: string]: number }, trip: TripDto) => {
      const tripDetail = parseTripData(trip.tripDetail);

      if (tripDetail && tripDetail.travelStyle) {
        const travelStyle = tripDetail.travelStyle;
        acc[travelStyle] = (acc[travelStyle] || 0) + 1;
      }
      return acc;
    },
    {}
  );

  return Object.entries(travelStyleCounts).map(([travelStyle, count]) => ({
    count: Number(count),
    travelStyle,
  }));
};
