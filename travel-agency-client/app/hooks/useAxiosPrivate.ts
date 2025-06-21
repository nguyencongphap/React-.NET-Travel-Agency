import { AxiosPrivate } from "~/api/axios";
import useAuth from "./useAuth";
import useRefreshToken from "./useRefreshToken";
import { useEffect } from "react";

// Use this hook to have the axios instance that has interceptors attached to the request and response
const useAxiosPrivate = () => {
  const refresh = useRefreshToken();
  const { auth } = useAuth();
  useEffect(() => {
    // these interceptors are like event listeners,
    // we need to remove them after attaching and using them.
    // Otherwise, we'll attach an infinite number of them, and they will mess up our requests and responses

    const responseInterceptor = AxiosPrivate.interceptors.response.use(
      // if the response is good, we return the response
      // otherwise, we do the async handler which includes retrying
      (response) => response,
      async (error) => {
        // access the previous request
        const prevRequest = error?.config;
        // if fail due to expired access token
        // and we check a custom prop "sent" that we'll set to prevent infinite retry
        const status = error?.response?.status;
        if ((status === 403 || status === 401) && !prevRequest.sent) {
          prevRequest.sent = true;
          const newAccessToken = await refresh();

          // update the request with the new access token and resend it
          return AxiosPrivate(prevRequest);
        }
        return Promise.reject(error);
      }
    );
    // remove interceptors when cleanup function runs
    return () => {
      AxiosPrivate.interceptors.response.eject(responseInterceptor);
    };
  }, [auth, refresh]);
  return AxiosPrivate;
};

export default useAxiosPrivate;
