import { PlaceData } from "@/data/mapBoxClient";
import React from "react";
import { LuMapPin } from "react-icons/lu";

interface SearchSuggestionProps {
	place: PlaceData;
	onClick: (place: PlaceData) => void;
}

function SearchSuggestion({ place, ...props }: SearchSuggestionProps) {
	return (
		<div
			key={place.place_name}
			className="p-4 flex gap-2 items-center hover:bg-muted cursor-pointer"
			onClick={() => props.onClick(place)}
		>
			<LuMapPin className="text-2xl text-primary" />
			<div>
				<p>{place.text}</p>
				<p className="text-sm text-muted-foreground">{place.place_name}</p>
			</div>
		</div>
	);
}

export default SearchSuggestion;
