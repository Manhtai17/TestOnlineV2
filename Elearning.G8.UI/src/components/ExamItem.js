import { CircularProgress } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import InfoOutlinedIcon from "@material-ui/icons/InfoOutlined";
import PlayCircleOutlineOutlinedIcon from "@material-ui/icons/PlayCircleOutlineOutlined";
import TimerOffOutlinedIcon from "@material-ui/icons/TimerOffOutlined";
import Start from "./modals/Start";
import { useHistory } from "react-router-dom";
import Result from "./modals/Result";
import { fetchTime } from "../action/time";

function ExamItem(props) {
	const [exam, setExam] = useState("");
	const [examDetail, setExamDetail] = useState("");
	const [loading, setLoading] = useState(true);
	const [error, setError] = useState(null);
	const [timeEnd, setTimeEnd] = useState("");
	const [modalShow, setModalShow] = useState("false");
	const userinfo = useSelector((state) => state.auth.userinfo);
	const dispatch = useDispatch();
	const history = useHistory();
	const timeError = useSelector((state) => state.time.timeError);
	const now = useSelector((state) => new Date(state.time.time));

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
					setExam(result.data.data);
				}
			})
			.catch((error) => {
				if (isMounted) {
					setError(error.toString());
				}
			});
		return () => {
			isMounted = false;
		};
	}, [dispatch, props.examId, userinfo.userID]);

	useEffect(() => {
		if (exam) {
			let isMounted = true;
			let url = `http://apig8.toedu.me/api/Exams/${exam.examID}`;

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
						setExamDetail(result.data);
					}
				})
				.catch((error) => {
					if (isMounted) {
						setLoading(false);
						setExamDetail("");
						setError(error.toString());
					}
				});
			return () => {
				isMounted = false;
			};
		}
	}, [exam, userinfo.userID]);

	useEffect(() => {
		dispatch(fetchTime());
	}, [dispatch]);

	useEffect(() => {
		if (examDetail && exam) {
			const created = new Date(examDetail.createdDate);
			const modified = new Date(examDetail.modifiedDate.replace("+00:00", ""));
			const timeSpent = Math.abs(modified - created) / 1000;
			setTimeEnd(timeSpent >= exam.timeToDo * 60 ? false : true);
		}
	}, [examDetail, exam]);

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
			) : loading || !exam || !examDetail || !now ? (
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
							{!examDetail.status && examDetail.point ? (
								<button className="body__right-score">
									<InfoOutlinedIcon />
									<span>{`Điểm: ${examDetail.point}`}</span>
								</button>
							) : !timeEnd || exam.finishTime <= now ? (
								examDetail.point ? (
									<button
										className="body__right-score"
										onClick={() => openModal("result")}
									>
										<InfoOutlinedIcon />
										<span>{`Điểm: ${examDetail.point}/10`}</span>
									</button>
								) : (
									<button className="body__right-timeout">
										<TimerOffOutlinedIcon />
										<span>Quá hạn làm</span>
									</button>
								)
							) : examDetail.point ? (
								<button
									className="body__right-start"
									onClick={() => {
										history.push(`/exam/${props.examId}/${exam.examID}`);
									}}
								>
									<PlayCircleOutlineOutlinedIcon />
									<span>Làm tiếp</span>
								</button>
							) : (
								<button
									className="body__right-start"
									onClick={() => openModal("start")}
								>
									<PlayCircleOutlineOutlinedIcon />
									<span>Bắt đầu</span>
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
