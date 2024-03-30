import { AiOutlineLoading3Quarters } from "react-icons/ai";

interface RefetchingIndicatorProps {
	isShown: boolean;
}

function RefetchingIndicator(props: RefetchingIndicatorProps) {
	if (props.isShown) {
		return (
			<div className="fixed bottom-10 left-16 z-50 flex items-center border-[1px] bg-primary px-3 py-2 text-primary-foreground rounded-md">
				<AiOutlineLoading3Quarters className="animate-spin text-xl mr-2" />
				<span>Caricamento...</span>
			</div>
		);
	}

	return null;
}

export default RefetchingIndicator;
