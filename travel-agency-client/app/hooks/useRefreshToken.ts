import Axios from "~/api/axios";
import useAuth from "./useAuth";

const useRefreshToken = () => {
  const { auth, setAuth } = useAuth();

  const refresh = async () => {
    if (auth) {
      const resp = await Axios.post("/refresh");

      const newAccessToken = resp?.data?.accessToken;

      // update access token in context
      setAuth((prev) => ({ ...prev, accessToken: newAccessToken }));

      return resp?.data?.accessToken;
    }
  };

  return refresh;
};

export default useRefreshToken;
