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
import { PlaceData } from "@/data/mapBoxClient";
import { useQuery } from "@tanstack/react-query";
import React, { useCallback } from "react";
import { Skeleton } from "@/components/ui/skeleton";

function TripsSearchPage() {
	const [searchedPlace, setSearchedPlace] = React.useState<PlaceData>();
	const [page, setPage] = React.useState(1);
	const [tripType, setTripType] = React.useState<number>();

	const tripsQuery = useQuery({
		queryKey: ["bestRatedTrips", page, searchedPlace, tripType],
		queryFn: async () => {
			const res = await apiClient.tripsApi.apiTripsGet(
				tripType,
				undefined,
				undefined,
				undefined,
				page,
				20,
				searchedPlace?.center[0],
				searchedPlace?.center[1]
			);

			return res.data;
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

		return (
			<div className="grid grid-cols-3 gap-12">
				{tripsQuery.data?.map((trip) => (
					<TripCard trip={trip} />
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
					onSearch={(place, category) => {}}
					cardClassName="p-2"
				/>
			</div>
			<div className="mt-14">{buildTripsData()}</div>
			<Pagination className="mt-16">
				<PaginationContent>
					<PaginationItem
						className="cursor-pointer"
						onClick={() => setPage((prev) => prev - 1)}
					>
						<PaginationPrevious />
					</PaginationItem>
					<PaginationItem>
						<PaginationLink href="#">{page}</PaginationLink>
					</PaginationItem>
					<PaginationItem>
						<PaginationEllipsis />
					</PaginationItem>
					<PaginationItem>
						<PaginationNext
							className="cursor-pointer"
							onClick={() => setPage((prev) => prev + 1)}
						/>
					</PaginationItem>
				</PaginationContent>
			</Pagination>
		</main>
	);
}

export default TripsSearchPage;
