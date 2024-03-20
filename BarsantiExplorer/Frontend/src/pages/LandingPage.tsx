import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import apiClient from "@/data/apiClient";
import { useQuery } from "@tanstack/react-query";
import React from "react";

function LandingPage() {
	const bestRatedTripsQuery = useQuery({
		queryKey: ["bestRatedTrips"],
		queryFn: async () => {
			const res = await apiClient.tripsApi.apiTripsGet();
			return res.data;
		},
	});

	return (
		<main className="page mt-16">
			<h1 className="text-6xl font-extrabold text-center">
				Discover new places
			</h1>
			<div className="flex justify-center mt-10">
				<Card className="max-w-3xl w-full p-3 rounded-full flex items-center">
					<Input
						placeholder="Search for a place or a trip"
						className="max-w-lg w-full p-4 border-none shadow-none focus-visible:ring-0 text-lg"
					/>
					<Button className="text-lg p-6 bg-primary text-white rounded-full ml-auto">
						Search
					</Button>
				</Card>
			</div>
			<div className="mt-20">
				<h2 className="text-3xl font-bold">Best Rated</h2>
				<div className="grid grid-cols-3 gap-12 mt-8">
					<Card className="h-96" />
					<Card className="h-96" />
					<Card className="h-96" />
				</div>
			</div>
		</main>
	);
}

export default LandingPage;
