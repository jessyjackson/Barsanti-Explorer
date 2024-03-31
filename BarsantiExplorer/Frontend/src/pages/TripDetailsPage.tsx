import { CreateCommentRequest } from "@/apiClient";
import Rating from "@/components/Rating";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import {
	Dialog,
	DialogContent,
	DialogDescription,
	DialogHeader,
	DialogTitle,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { useToast } from "@/components/ui/use-toast";
import apiClient from "@/data/apiClient";
import { useInfiniteQuery, useMutation, useQuery } from "@tanstack/react-query";
import React, { useState } from "react";
import { AiOutlineLoading3Quarters } from "react-icons/ai";
import { LuMapPin } from "react-icons/lu";
import { useNavigate, useParams } from "react-router-dom";
import InfiniteScroll from "react-infinite-scroller";
import { Card } from "@/components/ui/card";
import { useAuthStore } from "@/store/authStore";
import RefetchingIndicator from "@/components/RefetchingIndicator";
import {
	AlertDialog,
	AlertDialogCancel,
	AlertDialogContent,
	AlertDialogDescription,
	AlertDialogFooter,
	AlertDialogHeader,
	AlertDialogTitle,
} from "@/components/ui/alert-dialog";

function TripDetailsPage() {
	const [ratingDialogOpen, setRatingDialogOpen] = useState(false);
	const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
	const [givenRating, setGivenRating] = useState(0);
	const [author, setAuthor] = useState("");
	const [reviewText, setReviewText] = useState("");

	const { id } = useParams();
	const { toast } = useToast();
	const navigate = useNavigate();
	const auth = useAuthStore();

	const tripDetailsQuery = useQuery({
		queryKey: ["trip", id],
		queryFn: async () => {
			if (!id) throw new Error("No trip id provided");

			const trip = await apiClient.tripsApi.apiTripsIdGet(+id);
			return trip.data;
		},
	});

	const commentsQuery = useInfiniteQuery({
		queryKey: ["comments", "trip", id],
		queryFn: async ({ pageParam }) => {
			if (!id) throw new Error("No trip id provided");

			const comments = await apiClient.commentsApi.apiCommentsGet(
				+id,
				pageParam,
				20
			);
			return comments.data;
		},
		initialPageParam: 0,
		getNextPageParam: (lastPage, _, lastPageParam) => {
			if (lastPage && lastPage.length === 20) {
				return lastPageParam + 1;
			}
			return undefined;
		},
	});

	const reviewTripMutation = useMutation({
		mutationFn: async (review: CreateCommentRequest) => {
			if (!review.author || !review.text || !review.rating) {
				throw new Error("Please fill all the fields");
			}

			const response = await apiClient.commentsApi.apiCommentsPost(review);
			return response.data;
		},
		onSuccess: () => {
			setRatingDialogOpen(false);
			toast({
				title: "Review submitted",
				description: "Your review has been submitted successfully",
				variant: "success",
			});
		},
	});

	const deleteTripMutation = useMutation({
		mutationFn: async (tripId: number) => {
			await apiClient.tripsApi.apiTripsIdDelete(tripId);
		},
		onSuccess: () => {
			setDeleteDialogOpen(false);
			toast({
				title: "Trip deleted",
				description: "The trip has been deleted successfully",
				variant: "success",
			});
			navigate("/trips");
		},
		onError: (error) => {
			toast({
				title: "Error",
				description: error.message,
				variant: "destructive",
			});
		},
	});

	const editTrip = () => {
		navigate(`/admin/trips/${id}`);
	};

	const buildTripRatingDialog = () => {
		return (
			<Dialog open={ratingDialogOpen} onOpenChange={setRatingDialogOpen}>
				<DialogContent>
					<DialogHeader>
						<DialogTitle>Review the trip</DialogTitle>
						<DialogDescription>
							The review will be validated by the moderators before being
							published
						</DialogDescription>
					</DialogHeader>
					<div>
						<div className="flex items-center gap-4">
							<label>Rate: </label>
							<Rating
								rating={givenRating}
								className="text-2xl gap-3"
								onRatingChange={setGivenRating}
							/>
						</div>
						<Input
							placeholder="Your name"
							className="mt-4"
							value={author}
							onChange={(e) => setAuthor(e.target.value)}
						/>
						<Textarea
							placeholder="Write your review here..."
							className="mt-4"
							rows={6}
							value={reviewText}
							onChange={(e) => setReviewText(e.target.value)}
						/>
						<Button
							className="mt-4 w-full"
							loading={reviewTripMutation.isPending}
							onClick={() => {
								reviewTripMutation.mutate({
									author: author,
									text: reviewText,
									tripId: +id!,
									rating: givenRating,
								});
							}}
						>
							Submit
						</Button>
					</div>
				</DialogContent>
			</Dialog>
		);
	};

	const buildDeleteTripDialog = () => {
		return (
			<AlertDialog open={deleteDialogOpen} onOpenChange={setDeleteDialogOpen}>
				<AlertDialogContent>
					<AlertDialogHeader>
						<AlertDialogTitle>Delete trip</AlertDialogTitle>
					</AlertDialogHeader>
					<AlertDialogDescription>
						Are you sure you want to delete this trip?
					</AlertDialogDescription>
					<AlertDialogFooter>
						<AlertDialogCancel>Cancel</AlertDialogCancel>
						<Button
							variant="destructive"
							loading={deleteTripMutation.isPending}
							onClick={() => {
								deleteTripMutation.mutate(+id!);
							}}
						>
							Delete
						</Button>
					</AlertDialogFooter>
				</AlertDialogContent>
			</AlertDialog>
		);
	};

	if (tripDetailsQuery.isLoading) {
		return (
			<div className="flex items-center justify-center mt-32">
				<AiOutlineLoading3Quarters className="animate-spin text-4xl mr-4" />
				<span>Loading...</span>
			</div>
		);
	}

	if (tripDetailsQuery.isError) {
		return <div>Error: {tripDetailsQuery.error.message}</div>;
	}

	const trip = tripDetailsQuery.data!;

	return (
		<main className="page">
			<div className="flex gap-16">
				<img
					src={trip.image!}
					alt="location of the trip"
					className="flex-1 max-h-[30rem] rounded-lg object-cover"
				/>
				<div className="flex-1">
					<h1 className="text-4xl font-bold">{trip.title}</h1>
					<Rating
						showNumber
						rating={trip.averageRating ?? 0}
						className="text-xl mt-4 gap-3"
					/>
					<div className="flex items-center gap-2 mt-4 text-muted-foreground">
						<LuMapPin />
						<span>{trip.address}</span>
					</div>
					<div className="flex items-center gap-4 mt-4">
						<p>Type: </p>
						<Badge className="text-sm">{trip.tripType?.name}</Badge>
					</div>
					<Button
						className="mt-8 text-lg py-6 w-full"
						onClick={() => {
							setRatingDialogOpen(true);
							setGivenRating(0);
							setAuthor("");
							setReviewText("");
						}}
					>
						Rate Trip
					</Button>
					{auth.user && (
						<div className="flex gap-4 items-center">
							<Button
								variant="outline"
								className="mt-4 text-lg py-6 w-full"
								onClick={editTrip}
							>
								Edit
							</Button>
							<Button
								variant="destructive"
								className="mt-4 text-lg py-6 w-full"
								onClick={() => setDeleteDialogOpen(true)}
							>
								Delete
							</Button>
						</div>
					)}
				</div>
			</div>
			<h2 className="text-2xl font-semibold mt-10">Description</h2>
			<p className="mt-4">{trip.description}</p>
			<h2 className="text-2xl font-semibold mt-10">Comments</h2>
			<InfiniteScroll
				loadMore={() => commentsQuery.fetchNextPage()}
				hasMore={commentsQuery.hasNextPage}
				loader={<div>Loading...</div>}
			>
				<div className="grid grid-cols-3 gap-8">
					{commentsQuery.data?.pages.map((page, index) => (
						<React.Fragment key={index}>
							{page.map((comment) => (
								<Card key={comment.id} className="mt-4 p-4">
									<h3 className="text-xl font-semibold">{comment.author}</h3>
									<Rating rating={comment.rating ?? 0} className="mt-2 gap-3" />
									<p className="mt-2">{comment.text}</p>
								</Card>
							))}
						</React.Fragment>
					))}
					{commentsQuery.data?.pages &&
						commentsQuery.data.pages[0].length === 0 && (
							<div className="mt-4">No comments yet</div>
						)}
				</div>
			</InfiniteScroll>
			{buildTripRatingDialog()}
			{buildDeleteTripDialog()}
			<RefetchingIndicator isShown={tripDetailsQuery.isRefetching} />
		</main>
	);
}

export default TripDetailsPage;
