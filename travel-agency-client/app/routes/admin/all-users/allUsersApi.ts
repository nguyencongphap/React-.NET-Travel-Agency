import type { User } from "~/api/authApi";
import { AxiosPrivate } from "~/api/axios";

// TODO: Finish this
export const GetAllUsers = async (limit: number, offset: number) => {
  const res = await AxiosPrivate.get<User[]>("User/GetAllUsers");
};
