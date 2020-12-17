import React from "react";
import { useDispatch, useSelector } from "react-redux";
import { Link } from "react-router-dom";
import { logOut } from "../action/auth";
import MyLink from "./MyLink";

function Header() {
	const userinfo = useSelector((state) => state.auth.userinfo);
	const dispatch = useDispatch();

	const handleSignOut = () => {
		dispatch(logOut());
	};

	return (
		<header>
			<nav className="navbar">
				<div className="navbar__left">
					<Link to="/">
						<img
							className="navbar__left-logo"
							src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTsq0N58c2GvZpCKF5qaIAYk5uEgX1XJtA47A&usqp=CAU"
							alt="uetlogo"
						/>
					</Link>
					<div className="navbar__left-links">
						<MyLink to="/">Trang chủ</MyLink>
					</div>
				</div>
				<div className="navbar__right">
					<div to="#" className="navbar__right-profile">
						<div className="navbar__right-profile-button">
							<img
								src="https://media.istockphoto.com/vectors/avatar-icon-design-for-man-vector-id648229986?b=1&k=6&m=648229986&s=612x612&w=0&h=q63d9btUl0vzNubqWExOzp_7pKM6zC9aaQ0R157KPmw="
								alt="avatar"
							/>
							<Link to="#">{userinfo.userName}</Link>
						</div>
						<div className="navbar__right-profile-list">
							<Link to="#" onClick={() => handleSignOut()}>
								Đăng xuất
							</Link>
						</div>
					</div>
				</div>
			</nav>
		</header>
	);
}

export default Header;
