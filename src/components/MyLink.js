import React from "react";
import { Link, useLocation } from "react-router-dom";

function MyLink(props) {
	const location = useLocation().pathname;

	return (
		<Link to={props.to} className={location === props.to ? "active" : ""}>
			{props.children}
		</Link>
	);
}

export default MyLink;
