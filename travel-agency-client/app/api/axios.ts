import axios from "axios";
const BASE_URL = "http://localhost:5198";

export default axios.create({
  baseURL: BASE_URL,
});

// we attach to this the interceptors that will attach
// the jwt tokens for us
// and trigger retry when we get failure with status 403 Forbidden  the first time
export const axiosPrivate = axios.create({
  baseURL: BASE_URL,
  headers: { "Content-Type": "applciation/json" },
  withCredentials: true,
});
