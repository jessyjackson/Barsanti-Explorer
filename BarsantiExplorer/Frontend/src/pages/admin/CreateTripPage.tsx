import TripForm, { TripFormData } from "@/components/TripForm";
import { useToast } from "@/components/ui/use-toast";
import apiClient from "@/data/apiClient";
import mapBoxClient from "@/data/mapBoxClient";
import { useNavigate } from "react-router-dom";

function CreateTripPage() {
	const { toast } = useToast();
	const navigate = useNavigate();

	const onSubmit = async (values: TripFormData) => {
		try {
			console.log(values);

			let place = values.address;
			if (typeof place === "string") {
				const suggestions = await mapBoxClient.getPlaces(place);
				place = suggestions[0];
			}

			const res = await apiClient.tripsApi.apiTripsPost(
				values.image as File,
				values.title,
				values.description,
				place.place_name,
				place.center[0].toString(),
				place.center[1].toString(),
				values.type
			);
			const newTrip = res.data;

			navigate(`/trips/${newTrip.id}`);
			toast({
				title: "Trip created",
				description: "Your trip has been created successfully",
				variant: "success",
			});
		} catch (error) {
			console.error(error);
			toast({
				title: "Failed to create trip",
				description: "An error occurred while creating the trip",
				variant: "destructive",
			});
		}
	};

	return (
		<main className="page">
			<TripForm onSubmit={onSubmit} />
		</main>
	);
}

export default CreateTripPage;
