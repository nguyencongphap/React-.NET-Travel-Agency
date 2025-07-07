import type { User } from "~/api/authApi";
import { AxiosPrivate } from "~/api/axios";

export const getAllUsers = async (
  limit?: number,
  offset: number = 0
): Promise<{ users: User[]; total: number }> => {
  try {
    // Build query parameters conditionally
    const params = new URLSearchParams();
    if (limit !== undefined) {
      params.append("limit", limit.toString());
    }
    params.append("offset", offset.toString());

    const res = await AxiosPrivate.get<{ users: User[]; total: number }>(
      `User/GetAllUsers?${params.toString()}`
    );
    return res.data;
  } catch (error) {
    console.log("Error fetching users:", error);
    return { users: [], total: 0 };
  }
};
