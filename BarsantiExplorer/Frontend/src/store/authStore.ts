import { UserResponse } from "@/apiClient";
import apiClient from "@/data/apiClient";
import { AxiosError } from "axios";
import { create } from "zustand";

interface AuthStore {
	/**
	 *  The user object is null if the user is not logged in
	 */
	user: UserResponse | null;
	/**
	 * Used to know if the server has responded about the initial user status
	 */
	isUserLoading: boolean;
	/**
	 * Used to know if the user is logging in
	 */
	isLogging: boolean;
	/**
	 * Fetch the user related to the current access token
	 */
	fetchUser: () => Promise<void>;
	/**
	 * Login the user with the given credentials and store the tokens in the localstorage
	 */
	login: (username: string, password: string) => Promise<void>;
	/**
	 * Logout the user and remove the tokens from the localstorage
	 */
	logout: () => Promise<void>;
}

export const useAuthStore = create<AuthStore>((set) => ({
	user: null,
	isUserLoading: true,
	isLogging: false,

	fetchUser: async () => {
		try {
			set({ isUserLoading: true });
			const res = await apiClient.authApi.authMeGet();
			set({ user: res.data, isUserLoading: false });
		} catch (err) {
			console.log(err);
			const error = err as AxiosError;

			if (error.response?.status === 401) {
				set({ user: null, isUserLoading: false, isLogging: false });
				// localStorage.removeItem("accessToken");
			}

			throw error; // rethrow the error to the component to add UI feedback
		}
	},
	login: async (email, password) => {
		set({ isLogging: true });
		try {
			const res = await apiClient.authApi.authLoginPost({
				email: email,
				password: password,
			});

			const { user, token } = res.data;
			if (!user || !token) return;

			localStorage.setItem("accessToken", token);
			set({ user: user, isUserLoading: false, isLogging: false });
		} catch (err) {
			const error = err as AxiosError;

			if (error.response?.status === 401 || error.response?.status === 400) {
				set({ user: null, isUserLoading: false, isLogging: false });
				localStorage.removeItem("accessToken");
			}

			throw error; // rethrow the error to the component to add UI feedback
		}
	},
	logout: async () => {
		localStorage.removeItem("accessToken");
		localStorage.removeItem("refreshToken");
		set({ user: null, isLogging: false, isUserLoading: false });
	},
}));
