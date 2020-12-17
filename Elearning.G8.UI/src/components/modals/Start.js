import { CircularProgress } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useHistory } from "react-router-dom";
import { fetchTime } from "../../action/time";
import Modal from "../Modal";

function Start(props) {
	const [time, setTime] = useState("");
	const timeLoading = useSelector((state) => state.time.timeLoading);
	const timeError = useSelector((state) => state.time.timeError);
	const now = useSelector((state) => new Date(state.time.time));
	const dispatch = useDispatch();
	const history = useHistory();

	useEffect(() => {
		dispatch(fetchTime());
	}, [dispatch]);

	useEffect(() => {
		if (now) {
			const open = new Date(props.exam.timeOpen);
			if (now < open) {
				setTime(Math.abs(open - now) / 1000);
			}
		}
	}, [props.exam.timeOpen, now]);

	useEffect(() => {
		const timer = setInterval(() => {
			setTime((prevTime) => (prevTime <= 0 ? 0 : prevTime - 1));
		}, 1000);
		return () => {
			clearInterval(timer);
		};
	}, []);

	const handleClick = () => {
		if (time <= 0) {
			history.push(`/exam/${props.contestId}/${props.exam.examID}`);
		}
	};

	return (
		<Modal close={props.close} title="Làm bài thi">
			<div className="startModal">
				{timeLoading ? (
					<CircularProgress className="loadingCircle" />
				) : timeError ? (
					<div className="error">{timeError}</div>
				) : (
					<React.Fragment>
						<div className="startModal__content">
							<p>
								Thời gian bắt đầu:{" "}
								{new Date(props.startTime).toLocaleTimeString([], {
									timeStyle: "short",
								})}{" "}
								- {new Date(props.startTime).toLocaleDateString()}
							</p>
							<p>
								Thời gian đóng:{" "}
								{new Date(props.finishTime).toLocaleTimeString([], {
									timeStyle: "short",
								})}{" "}
								- {new Date(props.finishTime).toLocaleDateString()}
							</p>
							<p>Thời gian làm bài: {props.timeToDo} phút</p>
						</div>
						<div className="startModal__btn">
							<button onClick={() => handleClick()}>
								{time <= 0
									? "Làm bài"
									: time >= 60
									? time % 60 === 0
										? `Vui lòng đợi: ${Math.floor(time / 60)}:00`
										: `Vui lòng đợi: ${Math.floor(time / 60)}:${(
												"0" + Math.floor(time % 60)
										  ).slice(-2)}`
									: `Vui lòng đợi: ${Math.floor(time)}`}
							</button>
						</div>
					</React.Fragment>
				)}
			</div>
		</Modal>
	);
}

export default Start;
