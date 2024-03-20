import React from "react";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
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

const formSchema = z.object({
	title: z.string(),
	description: z.string(),
	address: z.string(),
	type: z.object({}).required(),
	image: z.string(),
});

function CreateTripPage() {
	const form = useForm<z.infer<typeof formSchema>>({
		resolver: zodResolver(formSchema),
	});

	function onSubmit(values: z.infer<typeof formSchema>) {
		console.log(values);
	}

	return (
		<main className="page">
			<Form {...form}>
				<form
					onSubmit={form.handleSubmit(onSubmit)}
					className="max-w-2xl w-full mx-auto"
				>
					<h1 className="text-3xl font-bold">Create a trip</h1>
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
							<FormItem className="w-full mt-6">
								<FormLabel>Address</FormLabel>
								<FormControl>
									<Input placeholder="Enter an address" {...field} />
								</FormControl>
								<FormMessage />
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
										<Select onValueChange={(val) => field.onChange(val)}>
											<SelectTrigger>
												<SelectValue placeholder="Select a type of trip" />
											</SelectTrigger>
											<SelectContent>
												<SelectItem value="light">Light</SelectItem>
												<SelectItem value="dark">Dark</SelectItem>
												<SelectItem value="system">System</SelectItem>
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
											{...field}
											placeholder="Select an image"
										/>
									</FormControl>
									<FormMessage />
								</FormItem>
							);
						}}
					/>
					<Button type="submit" className="w-full mt-6">
						Create Trip
					</Button>
				</form>
			</Form>
		</main>
	);
}

export default CreateTripPage;
