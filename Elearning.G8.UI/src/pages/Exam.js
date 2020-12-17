import { LinearProgress } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchExam } from "../action/exam";
import Footer from "../components/Footer";
import Header from "../components/Header";
import { useHistory, useParams } from "react-router-dom";
import Examsubmit from "../components/Examsubmit";
import TimerOffOutlinedIcon from "@material-ui/icons/TimerOffOutlined";
import CheckCircleOutlineOutlinedIcon from "@material-ui/icons/CheckCircleOutlineOutlined";
import ExamOpen from "../components/ExamOpen";
import ExamWait from "../components/ExamWait";

function Exam() {
	const examId = useParams().examId;
	const contestId = useParams().contestId;
	const timeToDo = sessionStorage.getItem(useParams().contestId);
	const examLoading = useSelector((state) => state.exam.examLoading);
	const examError = useSelector((state) => state.exam.examError);
	const exam = useSelector((state) => state.exam.exam);
	const userinfo = useSelector((state) => state.auth.userinfo);
	const answers = useSelector((state) => state.exam.answers);
	const [timer, setTimer] = useState("");
	const [modalShow, setModalShow] = useState("");
	const dispatch = useDispatch();
	const history = useHistory();

	useEffect(() => {
		if (userinfo.userID) {
			dispatch(fetchExam(examId, userinfo.userID));
		}
	}, [dispatch, examId, userinfo.userID]);

	useEffect(() => {
		if (exam) {
			const created = new Date(exam.createdDate);
			const modified = new Date(exam.modifiedDate.replace("+00:00", ""));
			const timeSpent = Math.abs(modified - created) / 1000;
			console.log(timeSpent);
			setTimer(
				timeSpent >= timeToDo * 60
					? "timeout"
					: Math.abs(timeToDo * 60 - timeSpent)
			);
		}
	}, [exam, timeToDo]);

	if (examError) {
		return <div className="error">{examError}</div>;
	} else if (examLoading || !timer) {
		return <LinearProgress className="loadingbar" />;
	} else {
		return (
			<div className="Exam">
				<Header />
				<main className="container">
					<React.Fragment>
						{modalShow ? (
							<Examsubmit
								examId={examId}
								contestId={contestId}
								userId={userinfo.userID}
								answers={answers}
								questions={JSON.parse(exam.question)}
							/>
						) : (
							""
						)}
						{!exam.status || modalShow ? (
							<div className="container__alert">
								<div className="container__alert-icon container__alert-icon--done">
									<CheckCircleOutlineOutlinedIcon />
								</div>
								<p>Bài thi đã được nộp</p>
								<button onClick={() => history.goBack()}>Quay lại</button>
							</div>
						) : timer === "timeout" ? (
							<div className="container__alert">
								<div className="container__alert-icon container__alert-icon--timeout">
									<TimerOffOutlinedIcon />
								</div>
								<p>Quá hạn làm bài</p>
								<button onClick={() => history.goBack()}>Quay lại</button>
							</div>
						) : exam.createdDate && timer ? (
							<ExamOpen
								examId={examId}
								contestId={contestId}
								userId={userinfo.userID}
								timer={timer}
								setModalShow={setModalShow}
							/>
						) : (
							<ExamWait exam={exam} />
						)}
					</React.Fragment>
				</main>
				<Footer />
			</div>
		);
	}
}

export default Exam;
