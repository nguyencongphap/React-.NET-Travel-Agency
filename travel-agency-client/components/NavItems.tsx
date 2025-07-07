import { Link, NavLink, useLoaderData, useNavigate } from "react-router";
import { getCurrentUser } from "~/api/authApi";
import { AxiosPrivate } from "~/api/axios";
import { sidebarItems } from "~/constants";
import { cn } from "~/lib/utils";

type NavItemsProps = { handleClick?: () => void };

const NavItems = ({ handleClick }: NavItemsProps) => {
  const user = useLoaderData(); // useLoaderData gets the data from the loader function of the nearest route
  const navigate = useNavigate();

  const handleLogout = async () => {
    await AxiosPrivate.post("Auth/logout");
    navigate("/sign-in");
  };

  return (
    <section className="nav-items">
      <Link to="/" className="link-logo">
        <img src="/assets/icons/logo.svg" alt="logo" className="size-[30px]" />
        <h1>Tourvisto</h1>
      </Link>

      <div className="container">
        <nav>
          {sidebarItems.map(({ id, href, icon, label }) => (
            <NavLink to={href} key={id}>
              {({ isActive }: { isActive: boolean }) => (
                <div
                  className={cn("group nav-item", {
                    "bg-primary-100 !text-white": isActive, // apply styles if active
                  })}
                  onClick={handleClick}
                >
                  <img
                    src={icon}
                    alt={label}
                    className={`group-hover:brightness-0 size-5 group-hover:invert ${
                      isActive ? "brightness-0 invert" : "text-dark-200"
                    }`}
                  />
                  {label}
                </div>
              )}
            </NavLink>
          ))}
        </nav>

        <footer className="nav-footer">
          {/* Implement profile picture later */}
          {/* <img
            src={user?.imageUrl || "/assets/images/david.webp"}
            alt={user?.name || "David"}
          /> */}

          {/* article is like a div but it says that the content within it are related*/}
          <article>
            <h2>{user?.name}</h2>
            <p>{user?.email}</p>
          </article>
          <button onClick={handleLogout} className="cursor-pointer">
            <img
              src="/assets/icons/logout.svg"
              alt="/logout"
              className="size-6"
            />
          </button>
        </footer>
      </div>
    </section>
  );
};

export default NavItems;
