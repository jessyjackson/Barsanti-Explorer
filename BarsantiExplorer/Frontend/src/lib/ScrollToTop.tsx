import { useLayoutEffect } from "react";
import { useLocation } from "react-router-dom";

interface ScrollToTopProps {
	children: React.ReactNode;
}

const ScrollToTop = (props: ScrollToTopProps) => {
	const location = useLocation();

	useLayoutEffect(() => {
		window.scrollTo(0, 0);
	}, [location.pathname]);

	return props.children;
};

export default ScrollToTop;
