import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { useToast } from "@/components/ui/use-toast";
import { useAuthStore } from "@/store/authStore";
import { IoSaveOutline } from "react-icons/io5";

function SyncTelegramPage() {
	const { user } = useAuthStore();
	const { toast } = useToast();

	const saveTelegramId = () => {
		// Save the telegram id
		toast({
			title: "Telegram Id saved",
			description: "Your telegram id has been saved successfully",
			duration: 1500,
			variant: "success",
		});
	};

	return (
		<main className="page mt-20">
			<h1 className="text-3xl font-bold text-center">Sync Telegram</h1>
			<p className="text-center mt-4">
				Enter "<b>get id</b>" in the chat bot to get your telegram Id
			</p>
			<div className="max-w-sm mx-auto">
				<Input
					className="p-6 text-base mt-4"
					defaultValue={user?.telegramId ?? ""}
					placeholder="Enter your telegram Id"
				/>
				<Button
					className="mx-auto mt-8 w-full flex gap-2"
					onClick={saveTelegramId}
				>
					<IoSaveOutline className="text-xl" />
					<span>Save Id</span>
				</Button>
			</div>
		</main>
	);
}

export default SyncTelegramPage;
