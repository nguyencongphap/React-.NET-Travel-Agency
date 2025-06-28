import type { User } from "~/api/authApi";
import { AxiosPrivate } from "~/api/axios";

export const GetAllUsers = async (
  limit: number,
  offset: number
): Promise<{ users: User[]; total: number }> => {
  try {
    const res = await AxiosPrivate.get<{ users: User[]; total: number }>(
      `User/GetAllUsers?limit=${limit}&offset=${offset}`
    );
    return res.data;
  } catch (error) {
    console.log("Error fetching users:", error);

    return { users: [], total: 0 };
  }
};
