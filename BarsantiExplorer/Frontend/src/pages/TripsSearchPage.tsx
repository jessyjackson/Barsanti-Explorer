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
import React from "react";

function TripsSearchPage() {
	return (
		<main className="page">
			<h1 className="text-3xl font-bold">Search Trips</h1>
			<div className="grid grid-cols-3 gap-12 mt-8">
				<Card className="h-96" />
				<Card className="h-96" />
				<Card className="h-96" />
			</div>
			<Pagination className="mt-16">
				<PaginationContent>
					<PaginationItem>
						<PaginationPrevious href="#" />
					</PaginationItem>
					<PaginationItem>
						<PaginationLink href="#">1</PaginationLink>
					</PaginationItem>
					<PaginationItem>
						<PaginationEllipsis />
					</PaginationItem>
					<PaginationItem>
						<PaginationNext href="#" />
					</PaginationItem>
				</PaginationContent>
			</Pagination>
		</main>
	);
}

export default TripsSearchPage;
