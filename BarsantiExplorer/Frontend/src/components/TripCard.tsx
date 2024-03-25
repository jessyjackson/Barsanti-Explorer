import React from "react";
import { Card } from "./ui/card";
import { Trip } from "@/apiClient";

interface TripCardProps {
	trip: Trip;
}

function TripCard(props: TripCardProps) {
	return (
		<Card className="h-96">
			<div></div>
		</Card>
	);
}

export default TripCard;
