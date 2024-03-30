import React from "react";
import { Card } from "./ui/card";
import { Trip } from "@/apiClient";
import { LuMapPin } from "react-icons/lu";
import { useNavigate } from "react-router-dom";
import { Badge } from "./ui/badge";
import { FaStar, FaStarHalf, FaRegStar } from "react-icons/fa6";
import Rating from "./Rating";

interface TripCardProps {
	trip: Trip;
}

function TripCard(props: TripCardProps) {
	const navigate = useNavigate();

	return (
		<Card
			className="flex flex-col cursor-pointer overflow-hidden"
			onClick={() => navigate(`/trips/${props.trip.id}`)}
		>
			<img src={props.trip.image!} alt="location place" className="h-72" />
			<div className="p-4">
				<h3 className="text-xl font-semibold">{props.trip.title}</h3>
				<Rating rating={props.trip.averageRating ?? 0} showNumber />
				<div className="flex items-center gap-2 mt-1 text-muted-foreground">
					<LuMapPin className="flex-shrink-0" />
					<span>{props.trip.address}</span>
				</div>
				<Badge className="mt-4 text-sm">{props.trip.tripType?.name}</Badge>
			</div>
		</Card>
	);
}

export default TripCard;
