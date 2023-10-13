// AuthProvider.ts
import {
  ACCESS_TOKEN_LOCAL_STORAGE_KEY,
  LoginRequest,
  LoginResponse,
  REFRESH_TOKEN_LOCAL_STORAGE_KEY,
} from "./types/auth.type";
import axios from "axios";
import { login } from "./utils/auth.utility";

export const authProvider = {
  login: async (request: LoginRequest) => {
    try {
      await login(request);
    } catch (error: any) {
      if (error?.response?.status === 400) {
        throw new Error("Wrong credentials");
      }

      throw new Error("Network error");
    }
  },

  logout: () => {
    const accessToken = localStorage.getItem(ACCESS_TOKEN_LOCAL_STORAGE_KEY);
    const refreshToken = localStorage.getItem(REFRESH_TOKEN_LOCAL_STORAGE_KEY);

    if (accessToken) {
      localStorage.removeItem(ACCESS_TOKEN_LOCAL_STORAGE_KEY);
    }

    if (refreshToken) {
      localStorage.removeItem(REFRESH_TOKEN_LOCAL_STORAGE_KEY);
    }

    return Promise.resolve();
  },

  checkAuth: (_: LoginRequest) => {
    const accessToken = localStorage.getItem(ACCESS_TOKEN_LOCAL_STORAGE_KEY);
    const refreshToken = localStorage.getItem(REFRESH_TOKEN_LOCAL_STORAGE_KEY);

    if (accessToken && refreshToken) {
      return Promise.resolve();
    }

    return Promise.reject();
  },

  // when the dataProvider returns an error, check if this is an authentication error
  checkError: (error: any) => {
    return Promise.resolve();
  },

  // get the user permissions (optional)
  getPermissions: () => {
    return Promise.resolve();
  },
};
