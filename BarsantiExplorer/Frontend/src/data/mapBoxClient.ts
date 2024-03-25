import { Axios } from "axios";

export interface PlaceData {
	place_name: string;
	center: [number, number];
	text: string;
}

class MapBoxClient {
	private axios: Axios;

	constructor() {
		this.axios = new Axios({
			baseURL: "https://api.mapbox.com/geocoding/v5/mapbox.places",
			params: {
				access_token:
					"pk.eyJ1IjoibWFydGlub2dhcnJpenpvNSIsImEiOiJja245YncxcDAxNmdsMnJwOW4zeDNuMzl5In0._w2M_BglX-CcR36dLNWy2A",
				proximity: "ip",
			},
		});
	}

	async getPlaces(query: string) {
		const res = await this.axios.get(`${query}.json`);
		const data = JSON.parse(res.data);
		const places = data.features as PlaceData[];
		console.log(places);
		return places;
	}
}

const mapBoxClient = new MapBoxClient();
export default mapBoxClient;
