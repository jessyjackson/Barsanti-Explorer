import { cn } from "@/lib/utils";
import React from "react";
import { FaRegStar, FaStar, FaStarHalfStroke } from "react-icons/fa6";

interface RatingProps {
	rating: number;
	className?: string;
	showNumber?: boolean;
	selectable?: boolean;
	onRatingChange?: (rating: number) => void;
}

function Rating({ rating, className, ...props }: RatingProps) {
	const buildStar = (val: number) => {
		if (val >= 1) {
			return <FaStar />;
		}
		if (val >= 0.5) {
			return <FaStarHalfStroke />;
		}

		return <FaRegStar />;
	};

	return (
		<div className={cn("flex items-center gap-1", className)}>
			{props.showNumber && <p>{rating.toFixed(1) ?? 0}</p>}
			<div className="flex items-center gap-1">
				{Array.from({ length: 5 }).map((_, index) => (
					<button
						className="focus:outline-none"
						key={index}
						onClick={() =>
							props.onRatingChange && props.onRatingChange(index + 1)
						}
					>
						{buildStar(rating - index)}
					</button>
				))}
			</div>
		</div>
	);
}

export default Rating;
