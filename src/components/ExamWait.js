import React, { useState, useEffect } from "react";
import TimerIcon from "@material-ui/icons/Timer";
import { useDispatch, useSelector } from "react-redux";
import { fetchTime } from "../action/time";
import { LinearProgress } from "@material-ui/core";

function ExamWait(props) {
	const [time, setTime] = useState("");
	const timeLoading = useSelector((state) => state.time.timeLoading);
	const timeError = useSelector((state) => state.time.timeError);
	const now = useSelector((state) => new Date(state.time.time));
	const dispatch = useDispatch();

	useEffect(() => {
		dispatch(fetchTime());
	}, [dispatch]);

	useEffect(() => {
		const open = new Date(props.exam.StartTime);
		if (now < open) {
			setTime(Math.abs(open - now) / 1000);
		}
	}, [props.exam.StartTime, now]);

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
			window.location.reload();
		}
	};

	if (timeLoading) {
		return <LinearProgress className="loadingbar" />;
	} else if (timeError) {
		return <div className="error">{timeError}</div>;
	} else {
		return (
			<div className="container__alert">
				<div className="container__alert-icon container__alert-icon--done">
					<TimerIcon />
				</div>
				<p>Chưa đến giờ làm bài</p>
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
		);
	}
}

export default ExamWait;
