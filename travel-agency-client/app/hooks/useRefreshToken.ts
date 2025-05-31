import axios from "~/api/axios";
import useAuth from "./useAuth";

const useRefreshToken = () => {
  const { auth, setAuth } = useAuth();

  const refresh = async () => {
    if (auth) {
      const resp = await axios.post<{
        accessToken: string;
        refreshToken: string;
      }>("/refreshToken", {
        username: auth?.username,
        refreshToken: auth?.refreshToken,
      });
      const newAccessToken = resp?.data?.accessToken;
      const newRefreshToken = resp?.data?.refreshToken;

      console.log("refresh resp", resp);

      // TODO: remove this when  switch to storing refresh token in cookie
      // update refresh token in context
      setAuth((prev) => ({ ...prev, refreshToken: newRefreshToken }));

      return resp?.data?.accessToken;
    }
  };

  return refresh;
};

export default useRefreshToken;
