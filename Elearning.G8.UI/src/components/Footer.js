import React from "react";
import FacebookIcon from "@material-ui/icons/Facebook";
import TwitterIcon from "@material-ui/icons/Twitter";
import YouTubeIcon from "@material-ui/icons/YouTube";

function Footer() {
	return (
		<footer>
			<div className="socials">
				<a href="https://www.facebook.com/">
					<FacebookIcon />
				</a>
				<a href="https://twitter.com/">
					<TwitterIcon />
				</a>
				<a href="https://www.youtube.com/">
					<YouTubeIcon />
				</a>
			</div>
		</footer>
	);
}

export default Footer;
