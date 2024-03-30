import RefetchingIndicator from "@/components/RefetchingIndicator";
import SearchBar from "@/components/SearchBar/SearchBar";
import TripCard from "@/components/TripCard";
import { Skeleton } from "@/components/ui/skeleton";
import apiClient from "@/data/apiClient";
import { useQuery } from "@tanstack/react-query";
import React, { useCallback } from "react";
import { useNavigate } from "react-router-dom";

function LandingPage() {
	const navigate = useNavigate();

	const bestRatedTripsQuery = useQuery({
		queryKey: ["bestRatedTrips"],
		queryFn: async () => {
			const res = await apiClient.tripsApi.apiTripsGet(
				undefined,
				undefined,
				"AverageRating",
				"desc",
				0,
				3
			);

			return res.data;
		},
	});

	const buildBestRatedTrips = useCallback(() => {
		if (bestRatedTripsQuery.isLoading) {
			return (
				<div className="grid grid-cols-3 gap-12 mt-8">
					<Skeleton className="w-full h-96" />
					<Skeleton className="w-full h-96" />
					<Skeleton className="w-full h-96" />
				</div>
			);
		}

		return (
			<div className="grid grid-cols-3 gap-12 mt-8">
				{bestRatedTripsQuery.data?.map((trip) => (
					<TripCard trip={trip} key={`trip-${trip.id}`} />
				))}
			</div>
		);
	}, [bestRatedTripsQuery]);

	return (
		<main className="page mt-16">
			<h1 className="text-6xl font-extrabold text-center">
				Discover new places
			</h1>
			<div className="max-w-3xl mx-auto mt-10">
				<SearchBar
					onSearch={(place, category) => {
						navigate("/trips", {
							state: { place },
						});
					}}
				/>
			</div>
			<div className="mt-20">
				<h2 className="text-3xl font-bold">Best Rated</h2>
				{buildBestRatedTrips()}
			</div>
			<RefetchingIndicator isShown={bestRatedTripsQuery.isRefetching} />
		</main>
	);
}

export default LandingPage;
