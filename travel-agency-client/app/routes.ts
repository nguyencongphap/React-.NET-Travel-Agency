import { type RouteConfig, layout, route } from "@react-router/dev/routes";

export default [
  route("register", "routes/root/register.tsx"),
  route("sign-in", "routes/root/sign-in.tsx"),

  // file path passed to layout must not have / in front of it
  layout("routes/admin/admin-layout.tsx", [
    route("dashboard", "routes/admin/dashboard/dashboard.tsx"),
    route("all-users", "routes/admin/all-users.tsx"),
  ]),
] satisfies RouteConfig;
