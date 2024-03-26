import React from "react";
import { Card } from "./ui/card";
import { Trip } from "@/apiClient";
import { LuMapPin } from "react-icons/lu";

interface TripCardProps {
	trip: Trip;
}

function TripCard(props: TripCardProps) {
	return (
		<Card className="h-96 flex flex-col overflow-hidden cursor-pointer">
			<div className="h-full relative">
				<img src={props.trip.image!} alt="location place" className="h-full" />
			</div>
			<div className="p-4 mb-4">
				<h3 className="text-xl font-semibold">{props.trip.title}</h3>
				<div className="flex items-center gap-2 mt-2 text-muted-foreground">
					<LuMapPin />
					<span>{props.trip.address}</span>
				</div>
			</div>
		</Card>
	);
}

export default TripCard;
