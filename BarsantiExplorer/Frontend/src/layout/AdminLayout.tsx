import { useAuthStore } from "@/store/authStore";
import { useEffect } from "react";
import { AiOutlineLoading3Quarters } from "react-icons/ai";
import { Outlet, useNavigate } from "react-router-dom";

function AdminLayout() {
	const auth = useAuthStore();
	const navigate = useNavigate();

	useEffect(() => {
		if (!auth.isLogging && !auth.isUserLoading && !auth.user) {
			navigate("/login");
		}
	}, [auth, navigate]);

	if (auth.isLogging || auth.isUserLoading) {
		return (
			<div className="flex items-center justify-center mt-32">
				<AiOutlineLoading3Quarters className="animate-spin text-4xl mr-4" />
				<span>Loading...</span>
			</div>
		);
	}

	return <Outlet />;
}

export default AdminLayout;
