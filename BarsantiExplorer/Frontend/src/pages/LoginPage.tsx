import { Button } from "@/components/ui/button";
import {
	FormControl,
	FormField,
	FormItem,
	FormLabel,
	FormMessage,
	Form,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { zodResolver } from "@hookform/resolvers/zod";
import { Label } from "@/components/ui/label";
import { useForm } from "react-hook-form";
import { z } from "zod";

const formSchema = z.object({
	email: z.string().email(),
	password: z.string().min(8),
});

function Login() {
	const form = useForm<z.infer<typeof formSchema>>({
		resolver: zodResolver(formSchema),
	});

	function onSubmit(values: z.infer<typeof formSchema>) {
		console.log(values);
	}

	return (
		<main className="page flex items-center">
			<Form {...form}>
				<form
					onSubmit={form.handleSubmit(onSubmit)}
					className="max-w-sm w-full mx-auto mt-16"
				>
					<Label className="text-3xl font-bold">Login</Label>
					<FormField
						control={form.control}
						name="email"
						render={({ field }) => (
							<FormItem className="w-full mt-6">
								<FormLabel>Email</FormLabel>
								<FormControl>
									<Input placeholder="Enter email" {...field} />
								</FormControl>
								<FormMessage />
							</FormItem>
						)}
					/>
					<FormField
						control={form.control}
						name="password"
						render={({ field }) => (
							<FormItem className="w-full mt-6">
								<FormLabel>Password</FormLabel>
								<FormControl>
									<Input
										placeholder="Enter password"
										{...field}
										type="password"
									/>
								</FormControl>
								<FormMessage />
							</FormItem>
						)}
					/>
					<Button type="submit" className="w-full mt-6">
						Login
					</Button>
				</form>
			</Form>
		</main>
	);
}

export default Login;
