import { useNavigate } from "react-router";
import { AxiosPrivate } from "~/api/axios";

const PageLayout = () => {
  const navigate = useNavigate();

  const handleLogout = async () => {
    await AxiosPrivate.post("Auth/logout");
    navigate("/sign-in");
  };

  return (
    <div>
      <button onClick={handleLogout} className="cursor-pointer">
        <img src="/assets/icons/logout.svg" alt="/logout" className="size-6" />
      </button>

      <button
        onClick={() => {
          navigate("/dashboard");
        }}
      >
        Dashboard
      </button>
    </div>
  );
};

export default PageLayout;
