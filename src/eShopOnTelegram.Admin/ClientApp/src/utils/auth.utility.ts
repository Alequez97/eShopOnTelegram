import axios from "axios";
import {
  ACCESS_TOKEN_LOCAL_STORAGE_KEY,
  LoginRequest,
  LoginResponse,
  REFRESH_TOKEN_LOCAL_STORAGE_KEY,
} from "../types/auth.type";

export const login = async (request: LoginRequest) => {
  const response = await axios.post("/auth/login", request);

  const auth = response.data as LoginResponse;
  localStorage.setItem(ACCESS_TOKEN_LOCAL_STORAGE_KEY, auth.accessToken);
  localStorage.setItem(REFRESH_TOKEN_LOCAL_STORAGE_KEY, auth.refreshToken);

  return auth;
};

export const refreshAccessToken = async (refreshToken: string) => {
  const response = await axios.post("/auth/token/refresh", {
    refreshToken,
  });

  const newAccessToken = response.data.accessToken;
  const newRefreshToken = response.data.refreshToken;
  localStorage.setItem(ACCESS_TOKEN_LOCAL_STORAGE_KEY, newAccessToken);
  localStorage.setItem(REFRESH_TOKEN_LOCAL_STORAGE_KEY, newRefreshToken);

  return newAccessToken;
};
