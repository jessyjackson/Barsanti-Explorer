import React from "react";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import {
	Select,
	SelectContent,
	SelectItem,
	SelectTrigger,
	SelectValue,
} from "@/components/ui/select";
import { Button } from "@/components/ui/button";
import {
	Form,
	FormControl,
	FormField,
	FormItem,
	FormLabel,
	FormMessage,
} from "@/components/ui/form";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useQuery } from "@tanstack/react-query";
import apiClient from "@/data/apiClient";
import AddressTextField from "./AddressTextField";
import { PlaceData } from "@/data/mapBoxClient";

const formSchema = z.object({
	title: z.string(),
	description: z.string(),
	address: z.union([
		z.string(),
		z.object({
			text: z.string(),
			place_name: z.string(),
			center: z.array(z.number()),
		}),
	]),
	type: z.number(),
	image: z.union([z.string(), z.instanceof(File)]),
});

export type TripFormData = z.infer<typeof formSchema>;

interface TripFormProps {
	onSubmit: (values: TripFormData) => void;
	defaultValues?: TripFormData;
	isEditing?: boolean;
}

function TripForm(props: TripFormProps) {
	const form = useForm<TripFormData>({
		resolver: zodResolver(formSchema),
		defaultValues: props.defaultValues,
	});

	const tripTypesQuery = useQuery({
		queryKey: ["tripTypes"],
		queryFn: async () => {
			const tripTypes = await apiClient.tripTypesApi.apiTripTypesGet();
			return tripTypes.data;
		},
	});

	return (
		<Form {...form}>
			<form
				onSubmit={form.handleSubmit(props.onSubmit)}
				className="max-w-2xl w-full mx-auto"
			>
				<h1 className="text-3xl font-bold">
					{props.isEditing ? "Edit" : "Create"} Trip
				</h1>
				<FormField
					control={form.control}
					name="title"
					render={({ field }) => (
						<FormItem className="w-full mt-6">
							<FormLabel>Title</FormLabel>
							<FormControl>
								<Input placeholder="Enter a title" {...field} />
							</FormControl>
							<FormMessage />
						</FormItem>
					)}
				/>
				<FormField
					control={form.control}
					name="description"
					render={({ field }) => (
						<FormItem className="mt-4">
							<FormLabel>Description</FormLabel>
							<FormControl>
								<Textarea
									placeholder="Enter a description"
									{...field}
									rows={5}
								/>
							</FormControl>
							<FormMessage />
						</FormItem>
					)}
				/>
				<FormField
					control={form.control}
					name="address"
					render={({ field }) => (
						<FormItem className="mt-4">
							<FormLabel>Address</FormLabel>
							<AddressTextField
								defaultPlace={
									props.defaultValues?.address instanceof Object
										? (props.defaultValues.address as PlaceData)
										: undefined
								}
								onPlaceChange={(place) => field.onChange(place)}
							/>
						</FormItem>
					)}
				/>
				<FormField
					control={form.control}
					name="type"
					render={({ field }) => {
						return (
							<FormItem className="mt-4">
								<FormLabel>Type</FormLabel>
								<FormControl>
									<Select
										onValueChange={(val) => field.onChange(+val)}
										defaultValue={field.value?.toString()}
									>
										<SelectTrigger className="w-full">
											<SelectValue placeholder="Select a type" />
										</SelectTrigger>
										<SelectContent>
											{tripTypesQuery.isLoading && (
												<SelectItem value="loading" disabled>
													Loading...
												</SelectItem>
											)}
											{tripTypesQuery.data?.map((tripType) => (
												<SelectItem
													key={tripType.id}
													value={tripType.id!.toString()}
												>
													{tripType.name}
												</SelectItem>
											))}
										</SelectContent>
									</Select>
								</FormControl>
								<FormMessage />
							</FormItem>
						);
					}}
				/>
				<FormField
					control={form.control}
					name="image"
					render={({ field }) => {
						return (
							<FormItem className="mt-4">
								<FormLabel>Image</FormLabel>
								<FormControl>
									<Input
										type="file"
										placeholder="Select an image"
										accept="image/*"
										onChange={(event) =>
											field.onChange(
												event.target.files && event.target.files[0]
											)
										}
									/>
								</FormControl>
								<FormMessage />
							</FormItem>
						);
					}}
				/>
				<Button
					type="submit"
					className="w-full mt-6"
					loading={form.formState.isSubmitting}
				>
					{props.isEditing ? "Edit" : "Create"} Trip
				</Button>
			</form>
		</Form>
	);
}

export default TripForm;
