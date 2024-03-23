import TripForm, { TripFormData } from "@/components/TripForm";
import React from "react";

function CreateTripPage() {
	function onSubmit(values: TripFormData) {
		console.log(values);
	}

	return (
		<main className="page">
			<TripForm onSubmit={onSubmit} />
		</main>
	);
}

export default CreateTripPage;
