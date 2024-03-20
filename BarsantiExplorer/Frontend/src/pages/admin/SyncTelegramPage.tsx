import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import React from "react";
import { IoMdRefresh } from "react-icons/io";

function SyncTelegramPage() {
	return (
		<main className="page mt-20">
			<h1 className="text-3xl font-bold text-center">Sync Telegram</h1>
			<p className="text-center mt-4">
				Copy the password on the telegram chatbot
				<br />
				to receive the messages about the trips comments
			</p>
			<div className="max-w-sm mx-auto">
				<Card className="py-4 px-12 mx-auto mt-8 font-medium text-xl tracking-widest">
					Paksdf;kasdkl32k4jlkds
				</Card>
				<Button className="mx-auto block mt-8 w-full flex gap-2">
					<IoMdRefresh className="text-xl" />
					<span>Refresh Token</span>
				</Button>
			</div>
		</main>
	);
}

export default SyncTelegramPage;
