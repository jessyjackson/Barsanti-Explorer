import SearchBar from "@/components/SearchBar/SearchBar";
import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Skeleton } from "@/components/ui/skeleton";
import apiClient from "@/data/apiClient";
import { useQuery } from "@tanstack/react-query";
import React from "react";
import { useNavigate } from "react-router-dom";

function LandingPage() {
	const navigate = useNavigate();

	const bestRatedTripsQuery = useQuery({
		queryKey: ["bestRatedTrips"],
		queryFn: async () => {
			const res = await apiClient.tripsApi.apiTripsGet(
				undefined,
				undefined,
				"rating",
				"desc",
				0,
				5
			);

			return res.data;
		},
	});

	return (
		<main className="page mt-16">
			<h1 className="text-6xl font-extrabold text-center">
				Discover new places
			</h1>
			<div className="max-w-3xl mx-auto mt-10">
				<SearchBar
					onSearch={(place, category) => {
						navigate("/trips", {
							state: { place, category },
						});
					}}
				/>
			</div>
			<div className="mt-20">
				<h2 className="text-3xl font-bold">Best Rated</h2>
				<div className="grid grid-cols-3 gap-12 mt-8">
					<Skeleton className="w-full h-96" />
					<Skeleton className="w-full h-96" />
					<Skeleton className="w-full h-96" />
				</div>
			</div>
		</main>
	);
}

export default LandingPage;
