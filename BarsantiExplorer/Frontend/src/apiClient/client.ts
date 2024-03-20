import { CommentsApi, Configuration, TripsApi } from ".";
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
	commentsApi: CommentsApi;

	constructor(config: ApiConfig = {}) {
		this.apiBaseUrl = config.baseUrl;
		this.axiosInstance = axios.create({
			// useless since swagger code overrides it
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
	}
}

export default Api;
