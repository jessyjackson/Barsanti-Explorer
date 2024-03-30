import React, { useCallback } from "react";
import { Card } from "../ui/card";
import { Input } from "../ui/input";
import { Button } from "../ui/button";
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";
import { Label } from "../ui/label";
import { useDebouncedCallback } from "use-debounce";
import { useQuery } from "@tanstack/react-query";
import mapBoxClient, { PlaceData } from "@/data/mapBoxClient";
import SearchSuggestion from "./SearchSuggestion";
import useComponentVisible from "@/hooks/useComponentVisible";
import { cn } from "@/lib/utils";
import apiClient from "@/data/apiClient";

interface SearchBarProps {
	onSearch: (place: PlaceData | string | null, tripType: string) => void;
	tripTypesEnabled?: boolean;
	cardClassName?: string;
	defaultPlace?: PlaceData;
	loading?: boolean;
}

function SearchBar(props: SearchBarProps) {
	const [placeText, setPlaceText] = React.useState(
		props.defaultPlace?.text ?? ""
	);
	const [debouncedPlaceText, setDebouncedPlaceText] = React.useState("");
	const [selectedSuggestion, setSelectedSuggestion] =
		React.useState<PlaceData | null>(props.defaultPlace ?? null);
	const [selectedTripType, setSelectedTripType] = React.useState<string>("all");

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

	const tripTypesQuery = useQuery({
		queryKey: ["tripTypes"],
		queryFn: async () => {
			const tripTypes = await apiClient.tripTypesApi.apiTripTypesGet();
			return tripTypes.data;
		},
	});

	const onSuggestionClick = (place: PlaceData) => {
		console.log(place);

		setPlaceText(place.text);
		setSelectedSuggestion(place);

		suggestionsVisibility.setIsComponentVisible(false);
		placeInputRef.current?.blur();
	};

	const onSearch = async () => {
		props.onSearch(selectedSuggestion ?? placeText, selectedTripType);
	};

	const buildTripTypesSelect = useCallback(() => {
		return (
			<>
				<div className="border-r border-border h-full" />
				<div className="flex items-center gap-4">
					<Label>Type: </Label>
					<Select value={selectedTripType} onValueChange={setSelectedTripType}>
						<SelectTrigger className="w-[180px]">
							<SelectValue />
						</SelectTrigger>
						<SelectContent>
							<SelectItem value="all">All</SelectItem>
							{tripTypesQuery.isLoading && (
								<SelectItem value="loading" disabled>
									Loading...
								</SelectItem>
							)}
							{tripTypesQuery.data?.map((tripType) => (
								<SelectItem key={tripType.id} value={tripType.id!.toString()}>
									{tripType.name}
								</SelectItem>
							))}
						</SelectContent>
					</Select>
				</div>
			</>
		);
	}, [tripTypesQuery, selectedTripType]);

	return (
		<div className="flex justify-center bg-card">
			<Card
				className={cn(
					"w-full p-3 rounded-full flex items-center gap-8",
					props.cardClassName
				)}
			>
				<div className="relative w-full" ref={suggestionsVisibility.ref}>
					<Input
						value={placeText}
						ref={placeInputRef}
						onFocus={() => {
							suggestionsVisibility.setIsComponentVisible(true);
						}}
						placeholder="Search for a place..."
						className="w-full p-4 border-none shadow-none focus-visible:ring-0 text-lg"
						onChange={(e) => {
							setPlaceText(e.target.value);
							setSelectedSuggestion(null);
							updateDebouncedVal(e.target.value);
						}}
					/>
					{placesSuggestions.data &&
						suggestionsVisibility.isComponentVisible && (
							<Card className="absolute bottom-0 left-0 translate-y-[calc(100%+1.25rem)] w-full shadow-lg rounded-lg z-10">
								{placesSuggestions.data.map((place) => (
									<SearchSuggestion
										key={place.place_name}
										place={place}
										onClick={onSuggestionClick}
									/>
								))}
							</Card>
						)}
				</div>
				{props.tripTypesEnabled && buildTripTypesSelect()}
				<Button
					className="text-lg p-6 bg-primary text-primary-foreground rounded-full ml-auto"
					onClick={onSearch}
					loading={props.loading}
				>
					Search
				</Button>
			</Card>
		</div>
	);
}

export default SearchBar;
