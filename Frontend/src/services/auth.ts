import { AxiosConfig } from "config/AxiosConfig";
import { UserI } from "types/client";

export interface LoginData {
  email: string;
  password: string;
}

export const login = async (payload: LoginData): Promise<UserI> => {
  const res = await AxiosConfig.post<UserI>("/Accounts/authenticate", payload);

  return res.data;
};
