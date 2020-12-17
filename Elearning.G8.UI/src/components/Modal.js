import React from "react";
import CloseIcon from "@material-ui/icons/Close";

function Modal(props) {
	return (
		<div className="modalWrapper">
			<div className="modal">
				<div className="modal__head">
					<div className="modal__head-left">{props.title}</div>
					<CloseIcon
						className="modal__head-right"
						onClick={() => props.close()}
					/>
				</div>
				<div className="modal__body">{props.children}</div>
			</div>
		</div>
	);
}

export default Modal;
