import axios from "axios";

// const BASE_URL = "https://localhost:7105";
const BASE_URL = "http://localhost:5164";

const Axios = axios.create({
  baseURL: BASE_URL,
  withCredentials: true, // bc we're using cookies
  headers: { "Content-Type": "application/json" },
});

export default Axios;

// we attach to this the interceptors that will attach
// the jwt tokens for us
// and trigger retry when we get failure with status 403 Forbidden  the first time
export const AxiosPrivate = axios.create({
  baseURL: BASE_URL,
  withCredentials: true, // bc we're using cookies
  headers: { "Content-Type": "application/json" },
});
