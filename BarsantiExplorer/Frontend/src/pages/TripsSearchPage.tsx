import SearchBar from "@/components/SearchBar/SearchBar";
import TripCard from "@/components/TripCard";
import { Card } from "@/components/ui/card";
import {
	Pagination,
	PaginationContent,
	PaginationEllipsis,
	PaginationItem,
	PaginationLink,
	PaginationNext,
	PaginationPrevious,
} from "@/components/ui/pagination";
import apiClient from "@/data/apiClient";
import mapBoxClient, { PlaceData } from "@/data/mapBoxClient";
import { useMutation, useQuery } from "@tanstack/react-query";
import React, { useCallback } from "react";
import { Skeleton } from "@/components/ui/skeleton";
import { useLocation } from "react-router-dom";
import { Button } from "@/components/ui/button";

interface TripsSearchPageState {
	place?: PlaceData;
}

function TripsSearchPage() {
	const { state } = useLocation();
	const { place } = (state ?? {}) as TripsSearchPageState;

	const [searchedPlace, setSearchedPlace] = React.useState<PlaceData | null>(
		place ?? null
	);
	const [page, setPage] = React.useState(1);
	const [tripType, setTripType] = React.useState<number>();
	const [isFetchingMissingPlace, setIsFetchingMissingPlace] =
		React.useState(false);

	const tripsQuery = useQuery({
		queryKey: ["bestRatedTrips", page, searchedPlace, tripType],
		queryFn: async () => {
			const res = await apiClient.tripsApi.apiTripsGet(
				tripType,
				undefined,
				undefined,
				undefined,
				page - 1,
				20,
				searchedPlace?.center[0],
				searchedPlace?.center[1]
			);

			return res.data;
		},
	});

	const searchBarMutation = useMutation({
		mutationFn: async (data: {
			place: PlaceData | string | null;
			tripType: string;
		}) => {
			console.log(data);
			const { place, tripType } = data;

			let selectedPlace = null;
			if (typeof place === "string") {
				if (place.length > 0) {
					setIsFetchingMissingPlace(true);
					const suggestions = await mapBoxClient.getPlaces(place);
					if (suggestions.length > 0) {
						selectedPlace = suggestions[0];
					}
					setIsFetchingMissingPlace(false);
				}
			} else {
				selectedPlace = place;
			}

			setPage(1);
			setSearchedPlace(selectedPlace);
			setTripType(tripType === "all" ? undefined : +tripType);
		},
	});

	const buildTripsData = useCallback(() => {
		if (tripsQuery.error) {
			return <div className="text-center">Unexpected Error</div>;
		}

		if (tripsQuery.isLoading || tripsQuery.isRefetching) {
			return (
				<div className="grid grid-cols-3 gap-12">
					<Skeleton className="w-full h-96" />
					<Skeleton className="w-full h-96" />
					<Skeleton className="w-full h-96" />
				</div>
			);
		}

		if (tripsQuery.data?.length === 0) {
			return <div className="text-center">No trips found</div>;
		}

		return (
			<div className="grid grid-cols-3 gap-12">
				{tripsQuery.data?.map((trip) => (
					<TripCard trip={trip} key={`trip-${trip.id}`} />
				))}
			</div>
		);
	}, [tripsQuery]);

	return (
		<main className="page">
			<h1 className="text-3xl font-bold">Search Trips</h1>
			<div className="mt-6">
				<SearchBar
					tripTypesEnabled
					onSearch={(place, tripType) =>
						searchBarMutation.mutateAsync({ place, tripType })
					}
					cardClassName="p-2"
					defaultPlace={place}
					loading={isFetchingMissingPlace}
				/>
			</div>
			<div className="mt-14">{buildTripsData()}</div>
			<Pagination className="mt-16">
				<PaginationContent>
					<Button
						className="p-2"
						variant="ghost"
						disabled={page === 1}
						onClick={() => setPage((prev) => prev - 1)}
					>
						<PaginationPrevious />
					</Button>
					<PaginationItem>
						<PaginationLink href="#">Page {page}</PaginationLink>
					</PaginationItem>
					<Button
						variant="ghost"
						disabled={tripsQuery.data?.length !== 20}
						onClick={() => setPage((prev) => prev + 1)}
					>
						<PaginationNext />
					</Button>
				</PaginationContent>
			</Pagination>
		</main>
	);
}

export default TripsSearchPage;
