import { Route, Routes } from "react-router-dom";
import CreateTripPage from "./pages/admin/CreateTripPage";
import Header from "./components/Header";
import LandingPage from "./pages/LandingPage";
import Footer from "./components/Footer";
import Login from "./pages/LoginPage";
import Dashboard from "./pages/admin/Dashboard";
import SyncTelegramPage from "./pages/admin/SyncTelegramPage";
import TripsSearchPage from "./pages/TripsSearchPage";

function App() {
	return (
		<>
			<Header />
			<Routes>
				<Route path="/" element={<LandingPage />} />
				<Route path="/trips" element={<TripsSearchPage />} />
				<Route path="/login" element={<Login />} />
				<Route path="/admin" element={<Dashboard />} />
				<Route path="/admin/telegram" element={<SyncTelegramPage />} />
				<Route path="/admin/trips/new" element={<CreateTripPage />} />
			</Routes>
			<Footer />
		</>
	);
}

export default App;
