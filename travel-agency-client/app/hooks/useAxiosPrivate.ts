import { axiosPrivate } from "~/api/axios";
import useAuth from "./useAuth";
import useRefreshToken from "./useRefreshToken";
import { useEffect } from "react";

// Use this hook to have the axios instance that has interceptors attached to the request and response
const useAxiosPrivate = () => {
  const refresh = useRefreshToken();
  const { auth } = useAuth();

  // useEffect(() => {
  //   // these interceptors are like event listeners,
  //   // we need to remove them after attaching and using them.
  //   // Otherwise, we'll attach an infinite number of them, and they will mess up our requests and responses

  //   const requestInterceptor = axiosPrivate.interceptors.request.use(
  //     (config) => {
  //       console.log("not have auth header", !config.headers["Authorization"]);

  //       // If Authorization header dne, then we know this is not a retry, this is the first request. We set the header wtih jwt to the request
  //       // If it's set, it must have been set by the responseInterceptor below, and it's a retry request.
  //       if (!config.headers["Authorization"]) {
  //         config.headers["Authorization"] = `Bearer ${auth?.accessToken}`;
  //       }
  //       return config;
  //     },
  //     (error) => Promise.reject(error)
  //   );

  //   const responseInterceptor = axiosPrivate.interceptors.response.use(
  //     // if the response is good, we return the response
  //     // otherwise, we do the async handler which includes retrying
  //     (response) => response,
  //     async (error) => {
  //       // TODO: DEL LATER
  //       console.log("error", error);

  //       // access the previous request
  //       const prevRequest = error?.config;
  //       // if fail due to expired access token
  //       // and we check a custom prop "sent" that we'll set to prevent infinite retry
  //       const status = error?.response?.status;

  //       console.log(
  //         "(status === 403 || status === 401) && !prevRequest.sent",
  //         (status === 403 || status === 401) && !prevRequest.sent
  //       );

  //       if ((status === 403 || status === 401) && !prevRequest.sent) {
  //         prevRequest.sent = true;
  //         const newAccessToken = await refresh();

  //         // TODO: DEL LATER
  //         console.log("newAccessToken", newAccessToken);

  //         prevRequest.headers["Authorization"] = `Bearer ${newAccessToken}`;

  //         // TODO: DEL LATER
  //         console.log("newAccessToken", newAccessToken);

  //         // update the request with the new access token and resend it
  //         return axiosPrivate(prevRequest);
  //       }
  //       return Promise.reject(error);
  //     }
  //   );

  //   // remove interceptors when cleanup function runs
  //   return () => {
  //     axiosPrivate.interceptors.request.eject(requestInterceptor);
  //     axiosPrivate.interceptors.response.eject(responseInterceptor);
  //   };
  // }, [auth, refresh]);

  return axiosPrivate;
};

export default useAxiosPrivate;
