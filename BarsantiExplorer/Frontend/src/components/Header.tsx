import React from "react";
import { Link } from "react-router-dom";
import { Button } from "../components/ui/button";
import logoImg from "../assets/logo.png";
import { LuMapPin, LuPlane, LuUser } from "react-icons/lu";

function Header() {
	return (
		<header className="py-8 px-12 flex items-center">
			<Link to="/">
				<img src={logoImg} alt="logo" className="w-48" />
			</Link>
			<div className="flex items-center mx-auto gap-12">
				<Link to="/">
					<Button variant="ghost" className="p-6">
						<LuMapPin className="text-2xl mr-2" />
						<p className="text-xl">Map</p>
					</Button>
				</Link>
				<Link to="/trips">
					<Button variant="ghost" className="p-6">
						<LuPlane className="text-2xl mr-2" />
						<p className="text-xl">Trips</p>
					</Button>
				</Link>
			</div>
			<div className="w-48 flex justify-end">
				<Link to="/login">
					<Button variant="ghost" className="p-6">
						<LuUser className="text-2xl mr-2" />
						<p className="text-xl">Login</p>
					</Button>
				</Link>
			</div>
		</header>
	);
}

export default Header;
