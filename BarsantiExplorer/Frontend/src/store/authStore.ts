import { create } from "zustand";

interface AuthStore {
	isLogging: boolean;
	user: string | null;
	login: (user: string) => void;
	logout: () => void;
}

export const useAuthStore = create<AuthStore>((set, get) => ({
	isLogging: false,
	user: null,
	login: (user) => {},
	logout: () => {
		localStorage.removeItem("accessToken");
		set({ user: null });
	},
}));
