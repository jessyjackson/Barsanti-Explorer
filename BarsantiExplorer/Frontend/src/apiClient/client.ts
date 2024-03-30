import { AuthApi, CommentsApi, Configuration, TripTypesApi, TripsApi } from ".";
import axios, { AxiosInstance } from "axios";

interface BaseConfig {
	config: Configuration;
	baseUrl?: string;
	axios: AxiosInstance;
}

interface ApiConfig {
	baseUrl?: string;
	onRefreshTokenError?: () => void;
}

class Api {
	apiBaseUrl?: string;
	axiosInstance: AxiosInstance;

	tripsApi: TripsApi;
	tripTypesApi: TripTypesApi;
	commentsApi: CommentsApi;
	authApi: AuthApi;

	constructor(config: ApiConfig = {}) {
		this.apiBaseUrl = config.baseUrl;
		this.axiosInstance = axios.create({
			url: this.apiBaseUrl,
		});
		// createAuthResponseInterceptor(
		// 	this.axiosInstance,
		// 	this,
		// 	config.onRefreshTokenError
		// );

		const baseConfig: BaseConfig = {
			config: {
				accessToken: () => {
					return localStorage.getItem("accessToken") as string;
				},
				isJsonMime: () => true,
			},
			baseUrl: this.apiBaseUrl,
			axios: this.axiosInstance,
		};

		this.tripsApi = new TripsApi(
			baseConfig.config,
			baseConfig.baseUrl,
			baseConfig.axios
		);
		this.commentsApi = new CommentsApi(
			baseConfig.config,
			baseConfig.baseUrl,
			baseConfig.axios
		);
		this.authApi = new AuthApi(
			baseConfig.config,
			baseConfig.baseUrl,
			baseConfig.axios
		);
		this.tripTypesApi = new TripTypesApi(
			baseConfig.config,
			baseConfig.baseUrl,
			baseConfig.axios
		);
	}
}

export default Api;
