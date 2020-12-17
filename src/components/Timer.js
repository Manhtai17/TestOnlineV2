import React, { useEffect, useState } from "react";
import PropTypes from "prop-types";
import CircularProgress from "@material-ui/core/CircularProgress";
import Typography from "@material-ui/core/Typography";
import Box from "@material-ui/core/Box";

function CircularProgressWithLabel(props) {
	return (
		<Box position="relative" display="inline-flex">
			<CircularProgress
				variant="static"
				value={props.value}
				className={
					props.time <= 10
						? "timeCircle timeCircle--danger"
						: props.time / props.fullTime <= 0.333
						? "timeCircle timeCircle--normal"
						: "timeCircle"
				}
				size={170}
				thickness={2}
			/>
			<Box
				top={0}
				left={0}
				bottom={0}
				right={0}
				position="absolute"
				display="flex"
				alignItems="center"
				justifyContent="center"
			>
				<Typography
					variant="caption"
					component="div"
					color="textSecondary"
					className="timeText"
				>
					<p>Thời gian còn lại</p>
					<p>
						{props.time >= 60
							? props.time % 60 === 0
								? `${Math.floor(props.time / 60)}:00`
								: `${Math.floor(props.time / 60)}:${(
										"0" + Math.floor(props.time % 60)
								  ).slice(-2)}`
							: `${Math.floor(props.time)}`}
					</p>
				</Typography>
			</Box>
		</Box>
	);
}

CircularProgressWithLabel.propTypes = {
	value: PropTypes.number.isRequired,
};

export default function Timer(props) {
	const [progress, setProgress] = useState(0);
	const [time, setTime] = useState(props.time);

	const timeStep = props.time * 10;

	useEffect(() => {
		const timer = setInterval(() => {
			setTime((prevTime) => (prevTime <= 0 ? 0 : prevTime - 1));
		}, 1000);
		const progressCal = setInterval(() => {
			setProgress((prevProgress) =>
				prevProgress >= 100 ? 100 : prevProgress + 1
			);
		}, timeStep);
		return () => {
			clearInterval(timer);
			clearInterval(progressCal);
		};
		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, []);

	useEffect(() => {
		if (time === 0) {
			props.handleSubmit()
		}
		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, [time])

	return (
		<CircularProgressWithLabel
			value={progress}
			time={time}
			fullTime={props.time}
		/>
	);
}
