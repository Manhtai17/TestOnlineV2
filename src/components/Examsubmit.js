import { CircularProgress } from "@material-ui/core";
import React, { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import CheckCircleOutlinedIcon from "@material-ui/icons/CheckCircleOutlined";
import SentimentDissatisfiedOutlinedIcon from "@material-ui/icons/SentimentDissatisfiedOutlined";
import { submitExam } from "../action/exam";
import { Link } from "react-router-dom";
import { answersToString } from "./answersToString";

function Examsubmit(props) {
	const submitLoading = useSelector((state) => state.exam.submitLoading);
	const submitError = useSelector((state) => state.exam.submitError);
	const score = useSelector((state) => state.exam.score);
	const result = answersToString(props.questions, props.answers);
	const dispatch = useDispatch();

	useEffect(() => {
		dispatch(
			submitExam(props.examId, props.contestId, props.userId, 1, 0, result)
		);
	}, [dispatch, props.examId, props.contestId, props.userId, result]);

	return (
		<div className="Examsubmit">
			<div className="wrapper">
				{submitError ? (
					<div className="content content--error">
						<SentimentDissatisfiedOutlinedIcon size={100} />
						<div className="content--error-text">{submitError}</div>
						<button
							className="content--error-btn"
							onClick={() => {
								dispatch(
									submitExam(
										props.examId,
										props.contestId,
										props.userId,
										1,
										0,
										result
									)
								);
							}}
						>
							Thử lại
						</button>
					</div>
				) : submitLoading || !score ? (
					<div className="content content--loading">
						<CircularProgress size={100} />
						<div className="content--loading-text">Đang nộp bài</div>
					</div>
				) : (
					<div className="content">
						<div className="content__text">
							<CheckCircleOutlinedIcon />
							<p>Nộp bài hoàn tất!</p>
						</div>
						<div className="content__score">
							<p>{score.point}</p>
						</div>
						<Link to="/" className="content__back">
							Về trang chủ
						</Link>
					</div>
				)}
			</div>
		</div>
	);
}

export default Examsubmit;
