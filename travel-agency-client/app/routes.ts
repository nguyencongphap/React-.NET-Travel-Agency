import { type RouteConfig, layout, route } from "@react-router/dev/routes";

export default [
  route("register", "routes/root/register.tsx"),
  route("sign-in", "routes/root/sign-in.tsx"),

  // API backend route (using server actions withini React is made possible by react router)
  route("api/create-trip", "routes/api/create-trip.ts"),

  // file path passed to layout must not have / in front of it
  layout("routes/admin/admin-layout.tsx", [
    route("dashboard", "routes/admin/dashboard/dashboard.tsx"),
    route("all-users", "routes/admin/all-users.tsx"),
    route("trips", "routes/admin/trips.tsx"),
    route("trips/create", "routes/admin/create-trip.tsx"),
    route("trips/:tripId", "routes/admin/trip-detail.tsx"),
  ]),
] satisfies RouteConfig;
