import React from "react";
import { Offline } from "react-detect-offline";

function LostConnection() {
	return (
		<Offline>
			<div className="LostConnection">
				<p>Mất kết nối, vui lòng kiểm tra đường truyền internet của bạn!</p>
				<a href=".">Tải lại trang</a>
			</div>
		</Offline>
	);
}

export default LostConnection;
