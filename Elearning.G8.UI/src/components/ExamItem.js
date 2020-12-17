import { CircularProgress } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import InfoOutlinedIcon from "@material-ui/icons/InfoOutlined";
import PlayCircleOutlineOutlinedIcon from "@material-ui/icons/PlayCircleOutlineOutlined";
import TimerOffOutlinedIcon from "@material-ui/icons/TimerOffOutlined";
import Start from "./modals/Start";
import { useHistory } from "react-router-dom";
import Result from "./modals/Result";

function ExamItem(props) {
	const [exam, setExam] = useState("");
	const [loading, setLoading] = useState(true);
	const [error, setError] = useState(null);
	const [modalShow, setModalShow] = useState("false");
	const userinfo = useSelector((state) => state.auth.userinfo);
	const dispatch = useDispatch();
	const history = useHistory();
	const timeError = useSelector((state) => state.time.timeError);

	useEffect(() => {
		let isMounted = true;
		let url = `http://apig8.toedu.me/api/Contests/${props.examId}`;

		fetch(url, {
			headers: {
				userId: userinfo.userID,
			},
		})
			.then((res) => res.json())
			.then((result) => {
				if (isMounted) {
					setLoading(false);
					setError(null);
					setExam(result.data.data);
				}
			})
			.catch((error) => {
				if (isMounted) {
					setLoading(false);
					setError(error.toString());
					setExam("");
				}
			});
		return () => {
			isMounted = false;
		};
	}, [dispatch, props.examId, userinfo.userID]);

	const closeModal = () => {
		setModalShow(false);
	};

	const openModal = (value) => {
		setModalShow(value);
	};

	return (
		<div className="ExamItem">
			{modalShow === "start" ? (
				<Start
					close={closeModal}
					exam={exam}
					contestId={props.examId}
					startTime={props.startTime}
					timeToDo={props.timeToDo}
					finishTime={props.finishTime}
				/>
			) : modalShow === "result" ? (
				<Result close={closeModal} exam={exam} />
			) : (
				""
			)}
			{error || timeError ? (
				<div className="error">{error || timeError}</div>
			) : loading || !exam ? (
				<CircularProgress className="loadingCircle" size={20} />
			) : (
				<React.Fragment>
					<div className="head">
						<div className="head__left">{props.examName}</div>
						<div className="head__right">
							{`${new Date(props.startTime).toLocaleDateString()} - ${new Date(
								props.startTime
							).toLocaleTimeString([], {
								timeStyle: "short",
							})}`}
						</div>
					</div>
					<div className="body">
						<div className="body__left">
							{`Thời gian làm bài: ${props.timeToDo} phút`}
						</div>
						<div className="body__right">
							{exam.continue === 0 ? (
								<button
									className="body__right-start"
									onClick={() => openModal("start")}
								>
									<PlayCircleOutlineOutlinedIcon />
									<span>Bắt đầu</span>
								</button>
							) : exam.continue === 1 ? (
								<button
									className="body__right-start"
									onClick={() => {
										history.push(`/exam/${props.examId}/${exam.examID}`);
									}}
								>
									<PlayCircleOutlineOutlinedIcon />
									<span>Làm tiếp</span>
								</button>
							) : exam.continue === 2 ? (
								<button className="body__right-timeout">
									<TimerOffOutlinedIcon />
									<span>Đã đóng</span>
								</button>
							) : (
								<button className="body__right-score">
									<InfoOutlinedIcon />
									<span>Chưa mở</span>
								</button>
							)}
						</div>
					</div>
				</React.Fragment>
			)}
		</div>
	);
}

export default ExamItem;
