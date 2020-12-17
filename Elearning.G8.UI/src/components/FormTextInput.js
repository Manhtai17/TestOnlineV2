import React, { useState } from "react";
import VisibilityIcon from "@material-ui/icons/Visibility";
import VisibilityOffIcon from "@material-ui/icons/VisibilityOff";
import "../sass/components/formTextInput.sass";

function FormTextInput(props) {
	const [viewPassword, setViewPassword] = useState(false);

	const handleShowPassword = () => {
		setViewPassword(viewPassword ? false : true);
	};

	if (props.type === "password") {
		return (
			<div className="FormTextInput">
				<div className="FormTextInput__input">
					<input
						type={viewPassword ? "text" : "password"}
						placeholder={props.placeholder}
						name={props.name}
						value={props.value}
						onChange={(value) => {
							props.handleChange(value);
							if (props.handleAlert) {
								props.handleAlert(value);
							}
						}}
					/>
					<div>
						{props.alert ? (
							<small className="FormTextInput__alert">{props.alert}</small>
						) : (
							""
						)}
						<span onClick={() => handleShowPassword()}>
							{viewPassword ? <VisibilityIcon /> : <VisibilityOffIcon />}
						</span>
					</div>
				</div>
			</div>
		);
	} else {
		return (
			<div className="FormTextInput">
				<div className="FormTextInput__input">
					<input
						type={props.type}
						placeholder={props.placeholder}
						name={props.name}
						value={props.value}
						onChange={(value) => {
							props.handleChange(value);
							if (props.handleAlert) {
								props.handleAlert(value);
							}
						}}
					/>
					<div>
						{props.alert ? (
							<small className="FormTextInput__alert">{props.alert}</small>
						) : (
							""
						)}
					</div>
				</div>
			</div>
		);
	}
}

export default FormTextInput;
