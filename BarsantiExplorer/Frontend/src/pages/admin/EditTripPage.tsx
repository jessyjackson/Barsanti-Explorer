import TripForm, { TripFormData } from "@/components/TripForm";
import { useToast } from "@/components/ui/use-toast";
import apiClient from "@/data/apiClient";
import mapBoxClient from "@/data/mapBoxClient";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { useNavigate, useParams } from "react-router-dom";

function EditTripPage() {
	const params = useParams();
	const { id } = params;
	const navigate = useNavigate();
	const { toast } = useToast();
	const queryClient = useQueryClient();

	const tripDetailsQuery = useQuery({
		queryKey: ["trip", id],
		queryFn: async () => {
			if (!id) throw new Error("No trip id provided");

			const trip = await apiClient.tripsApi.apiTripsIdGet(+id);
			return trip.data;
		},
	});

	const onSubmit = async (values: TripFormData) => {
		try {
			console.log(values);

			let place = values.address;
			if (typeof place === "string") {
				const suggestions = await mapBoxClient.getPlaces(place);
				place = suggestions[0];
			}

			await apiClient.tripsApi.apiTripsIdPut(
				+id!,
				values.image instanceof File ? values.image : undefined,
				values.title,
				values.description,
				place.place_name,
				place.center[0].toString(),
				place.center[1].toString(),
				values.type
			);

			navigate(`/trips/${id}`);
			toast({
				title: "Trip created",
				description: "Your trip has been created successfully",
				variant: "success",
			});
		} catch (err) {
			console.error(err);
			toast({
				title: "Failed to create trip",
				description: "An error occurred while creating the trip",
				variant: "destructive",
			});
		}
	};

	if (tripDetailsQuery.isLoading) {
		return <div>Loading...</div>;
	}

	return (
		<main>
			<TripForm
				onSubmit={onSubmit}
				isEditing
				defaultValues={{
					title: tripDetailsQuery.data?.title ?? "",
					description: tripDetailsQuery.data?.description ?? "",
					address: {
						place_name: tripDetailsQuery.data?.address ?? "",
						center: [
							tripDetailsQuery.data?.longitude ?? 0,
							tripDetailsQuery.data?.latitude ?? 0,
						],
						text: tripDetailsQuery.data?.address ?? "",
					},
					type: tripDetailsQuery.data?.tripType?.id ?? 0,
					image: tripDetailsQuery.data?.image ?? "",
				}}
			/>
		</main>
	);
}

export default EditTripPage;
