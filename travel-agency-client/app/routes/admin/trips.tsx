import Header from "components/Header";
import { useSearchParams, type LoaderFunctionArgs } from "react-router";
import { getAllTrips, getTotalTripsCount } from "~/api/tripApi";
import { parseTripData } from "~/lib/utils";
import type { Route } from "./+types/trips";
import { TripCard } from "components";
import type { Trip } from "~/index";
import { useState } from "react";
import { PagerComponent } from "@syncfusion/ej2-react-grids";

const PAGE_SIZE = 8;

// we use request here because we implement pagination
export const clientLoader = async ({ request }: LoaderFunctionArgs) => {
  const limit = PAGE_SIZE;
  const url = new URL(request.url);
  const page = parseInt(url.searchParams.get("page") || "1", 10);
  const offset = (page - 1) * limit;

  // fire all async fetch calls at once
  const [{ allTrips }, total] = await Promise.all([
    await getAllTrips(limit, offset),
    await getTotalTripsCount(),
  ]);

  return {
    trips: allTrips.map((trip) => ({
      id: trip?.id,
      ...parseTripData(trip?.tripDetail ?? ""),
      imageUrls: trip?.tripImageUrls ?? [],
    })),
    total,
  };
};

const Trips = ({ loaderData }: Route.ComponentProps) => {
  const allTrips = (loaderData.trips as Trip[]) || [];

  const [searchParams, setSearchParams] = useSearchParams();
  const initialPage = Number(searchParams.get("page") || "1");

  const [currentPage, setCurrentPage] = useState(initialPage);

  const handlePageChange = (page: number) => {
    setCurrentPage(page);
    setSearchParams({ page: page.toString() });
  };

  return (
    <main className="all-users wrapper">
      <Header
        title="Trips"
        description="View and edit AI-generated travel plans"
        ctaText="Create a trip"
        ctaUrl="/trips/create"
      />

      <section>
        <h1 className="p-24-semibold text-dark-100 mb-4">
          Manage Created Trips
        </h1>

        <div className="trip-grid">
          {allTrips.map(
            ({
              id,
              name,
              imageUrls,
              itinerary,
              interests,
              travelStyle,
              estimatedPrice,
            }) => (
              <TripCard
                id={id}
                key={id}
                name={name}
                location={
                  itinerary && itinerary.length > 0 ? itinerary[0].location : ""
                }
                imageUrl={imageUrls ? imageUrls[0] : ""}
                tags={[interests, travelStyle]}
                price={estimatedPrice}
              />
            )
          )}
        </div>

        <PagerComponent
          totalRecordsCount={loaderData.total}
          pageSize={PAGE_SIZE}
          currentPage={currentPage}
          click={(args) => handlePageChange(args.currentPage)}
          cssClass="!mb-4"
        />
      </section>
    </main>
  );
};

export default Trips;
