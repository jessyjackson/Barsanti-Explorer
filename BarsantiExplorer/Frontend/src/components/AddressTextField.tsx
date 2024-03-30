import mapBoxClient, { PlaceData } from "@/data/mapBoxClient";
import useComponentVisible from "@/hooks/useComponentVisible";
import { useQuery } from "@tanstack/react-query";
import React from "react";
import { useDebouncedCallback } from "use-debounce";
import { Input } from "./ui/input";
import { Card } from "./ui/card";
import { LuMapPin } from "react-icons/lu";

interface AddressTextFieldProps {
	defaultPlace?: PlaceData;
	onPlaceChange: (place: PlaceData) => void;
}

function AddressTextField(props: AddressTextFieldProps) {
	const [placeText, setPlaceText] = React.useState(
		props.defaultPlace?.text ?? ""
	);
	const [debouncedPlaceText, setDebouncedPlaceText] = React.useState("");
	const [showSuggestions, setShowSuggestions] = React.useState(false);

	const suggestionsVisibility = useComponentVisible(false);

	const updateDebouncedVal = useDebouncedCallback((value) => {
		setDebouncedPlaceText(value);
	}, 1000);

	const placeInputRef = React.useRef<HTMLInputElement>(null);

	const placesSuggestions = useQuery({
		queryKey: ["places", debouncedPlaceText],
		queryFn: async () => {
			const suggestions = await mapBoxClient.getPlaces(debouncedPlaceText);
			return suggestions;
		},
		enabled: debouncedPlaceText !== "",
		refetchOnMount: false,
		refetchOnReconnect: false,
		refetchOnWindowFocus: false,
	});

	const onSuggestionClick = (place: PlaceData) => {
		setPlaceText(place.text);
		props.onPlaceChange(place);

		suggestionsVisibility.setIsComponentVisible(false);
		placeInputRef.current?.blur();
	};

	return (
		<div className="relative w-full" ref={suggestionsVisibility.ref}>
			<Input
				value={placeText}
				ref={placeInputRef}
				onFocus={() => {
					setShowSuggestions(true);
					suggestionsVisibility.setIsComponentVisible(true);
				}}
				placeholder="Search for a place..."
				className="w-full"
				onChange={(e) => {
					setPlaceText(e.target.value);
					updateDebouncedVal(e.target.value);
				}}
			/>
			{placesSuggestions.data &&
				showSuggestions &&
				suggestionsVisibility.isComponentVisible && (
					<Card className="absolute bottom-0 left-0 translate-y-[calc(100%+0.5rem)] w-full shadow-lg rounded-lg z-10">
						{placesSuggestions.data.map((place) => (
							<div
								key={place.place_name}
								className="px-4 py-2 flex gap-2 items-center hover:bg-muted cursor-pointer"
								onClick={() => onSuggestionClick(place)}
							>
								<LuMapPin className="text-primary" />
								<div>
									<p>{place.text}</p>
									<p className="text-sm text-muted-foreground">
										{place.place_name}
									</p>
								</div>
							</div>
						))}
					</Card>
				)}
		</div>
	);
}

export default AddressTextField;
