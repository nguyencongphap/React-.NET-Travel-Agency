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

type UserKeysType = {
  readonly [K in keyof User]: K;
};

export const USER_KEYS_TYPED: UserKeysType = {
  id: "id",
  name: "name",
  email: "email",
  dateJoined: "dateJoined",
  itineraryCreated: "itineraryCreated",
  roles: "roles",
};

export const getCurrentUser = async () => {
  const res = await AxiosPrivate.get<User>("/me");
  return res.data;
};
