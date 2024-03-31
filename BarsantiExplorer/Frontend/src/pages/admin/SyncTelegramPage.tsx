import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { useToast } from "@/components/ui/use-toast";
import { useAuthStore } from "@/store/authStore";
import { useMutation } from "@tanstack/react-query";
import { useState } from "react";
import { IoSaveOutline } from "react-icons/io5";

function SyncTelegramPage() {
	const auth = useAuthStore();
	const { toast } = useToast();

	const [telegramId, setTelegramId] = useState(
		auth.user.telegramId?.toString() ?? ""
	);

	const saveTelegramId = useMutation({
		mutationFn: async (telegramId: string) => {
			await auth.setTelegramId(Number.parseInt(telegramId));
		},
		onSuccess: () => {
			toast({
				title: "Telegram Id saved",
				description: "Your telegram id has been saved successfully",
				duration: 1500,
				variant: "success",
			});
		},
		onError: (error) => {
			toast({
				title: "Error",
				description: error.message,
				duration: 1500,
				variant: "destructive",
			});
		},
	});

	return (
		<main className="page mt-20">
			<h1 className="text-3xl font-bold text-center">Sync Telegram</h1>
			<p className="text-center mt-4">
				Enter "<b>get id</b>" in the chat bot to get your telegram Id
			</p>
			<div className="max-w-sm mx-auto">
				<Input
					className="p-6 text-base mt-4"
					defaultValue={auth.user.telegramId ?? ""}
					placeholder="Enter your telegram Id"
					onChange={(e) => {
						if (Number.parseInt(e.target.value)) {
							setTelegramId(e.target.value);
						} else {
							setTelegramId("");
						}
					}}
					value={telegramId}
				/>
				<Button
					className="mx-auto mt-8 w-full flex gap-2"
					onClick={() => saveTelegramId.mutate(telegramId)}
					loading={saveTelegramId.isPending}
				>
					<IoSaveOutline className="text-xl" />
					<span>Save Id</span>
				</Button>
			</div>
		</main>
	);
}

export default SyncTelegramPage;
