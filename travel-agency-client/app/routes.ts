import { type RouteConfig, layout, route } from "@react-router/dev/routes";

export default [
  // file path passed to layout must not have / in front of it
  layout("routes/admin/admin-layout.tsx", [
    route("dashboard", "routes/admin/dashboard.tsx"),
    route("all-users", "routes/admin/all-users.tsx"),
  ]),
] satisfies RouteConfig;
