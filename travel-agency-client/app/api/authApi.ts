import { AxiosPrivate } from "./axios";
import { ROLE_NORMALIZED_NAMES } from "../types/roleNames";

export type User = {
  id: string;
  name: string;
  email: string;
  dateJoined: Date;
  itineraryCreated: number;
  roles: ROLE_NORMALIZED_NAMES[];
};

export const GetCurrentUser = async () => {
  const res = await AxiosPrivate.get<User>("/me");
  return res.data;
};
