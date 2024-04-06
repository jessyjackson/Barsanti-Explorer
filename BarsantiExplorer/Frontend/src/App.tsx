import { Route, Routes } from "react-router-dom";
import CreateTripPage from "./pages/admin/CreateTripPage";
import TripTypesPage from "./pages/admin/TripTypesPage";
import Header from "./components/Header";
import LandingPage from "./pages/LandingPage";
import Footer from "./components/Footer";
import Login from "./pages/LoginPage";
import Dashboard from "./pages/admin/Dashboard";
import SyncTelegramPage from "./pages/admin/SyncTelegramPage";
import TripsSearchPage from "./pages/TripsSearchPage";
import TripDetailsPage from "./pages/TripDetailsPage";
import EditTripPage from "./pages/admin/EditTripPage";
import { ThemeProvider } from "./components/ThemeProvider";
import { useEffect } from "react";
import { useAuthStore } from "./store/authStore";
import { Toaster } from "./components/ui/toaster";
import AdminLayout from "./layout/AdminLayout";
import ScrollToTop from "./lib/ScrollToTop";

function App() {
	const auth = useAuthStore();

	useEffect(() => {
		auth.fetchUser();
	}, []);

	return (
		<ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
			<ScrollToTop>
				<Header />
				<Routes>
					<Route path="/" element={<LandingPage />} />
					<Route path="/trips" element={<TripsSearchPage />} />
					<Route path="/trips/:id" element={<TripDetailsPage />} />
					<Route path="/login" element={<Login />} />
					<Route path="/admin" element={<AdminLayout />}>
						<Route path="/admin" element={<Dashboard />} />
						<Route path="/admin/telegram" element={<SyncTelegramPage />} />
						<Route path="/admin/trips/new" element={<CreateTripPage />} />
						<Route path="/admin/trips-type" element={<TripTypesPage />} />
						<Route path="/admin/trips/:id" element={<EditTripPage />} />
					</Route>
				</Routes>
				<Footer />
				<Toaster />
			</ScrollToTop>
		</ThemeProvider>
	);
}

export default App;
