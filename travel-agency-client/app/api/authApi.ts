import { AxiosPrivate } from "./axios";
import { ROLE_NAMES } from "../types/roleNames";

export const GetCurrentUser = async () => {
  const res = await AxiosPrivate.get<{ id: string; roles: ROLE_NAMES[] }>(
    "/me"
  );
  return res.data;
};
