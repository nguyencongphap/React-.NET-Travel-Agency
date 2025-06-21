import { Outlet, redirect } from "react-router";
import { SidebarComponent } from "@syncfusion/ej2-react-navigations";
import { MobileSidebar, NavItems } from "components";
import { GetCurrentUser } from "~/api/authApi";
import { ROLE_NAMES } from "~/types/roleNames";

export async function clientLoader() {
  try {
    const user = await GetCurrentUser();

    if (!user.id) return redirect("/sign-in");

    // Only admin can see the admin dashboard
    if (user.roles.find((x) => x === ROLE_NAMES.User)) {
      return redirect("/");
    }
  } catch (error) {
    console.log("Error in clientLoader", error);
    return redirect("/sign-in");
  }
}

const AdminLayout = () => {
  return (
    <div className="admin-layout">
      <MobileSidebar />

      <aside className="w-full max-w-[270px] hidden lg:block">
        <SidebarComponent
          width={270}
          enableGestures={false} // desktop sidebar
        >
          <NavItems />
        </SidebarComponent>
      </aside>

      <aside className="children">
        <Outlet />
      </aside>
    </div>
  );
};

export default AdminLayout;
