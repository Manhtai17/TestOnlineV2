import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { setAnswers, submitExam } from "../action/exam";
import ExamSidebar from "./ExamSidebar";
import Question from "./Question";
import FlagIcon from "@material-ui/icons/Flag";
import FlagOutlinedIcon from "@material-ui/icons/FlagOutlined";
import PublishIcon from "@material-ui/icons/Publish";
import { answersToString } from "./answersToString";

function ExamOpen(props) {
	const exam = useSelector((state) => state.exam.exam);
	const questions = JSON.parse(exam.question);
	const corrects = exam.answer.substring(1).split("|");
	const answers = useSelector((state) => state.exam.answers);
	const [flags, setFlags] = useState([]);
	const [index, setIndex] = useState(0);
	const dispatch = useDispatch();

	useEffect(() => {
		if (exam.result) {
			let savedAnswers = exam.result.substring(1).split("|");
			let obj = {};
			for (let i = 0; i < questions.length; i++) {
				if (savedAnswers[i].localeCompare("") !== 0) {
					obj[questions[i].QuestionTitle] = savedAnswers[i].split(",");
				}
            }
			dispatch(setAnswers(obj));
		}
		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, [dispatch]);

	//auto submit
	useEffect(() => {
		if (exam && exam.status !== 1) {
			const timer = setInterval(() => {
				const result = answersToString(questions, answers);
				if (window.navigator.onLine) {
					dispatch(
						submitExam(
							props.examId,
							props.contestId,
							props.userId,
							0,
							1,
							result
						)
					);
				}
			}, 5000);
			return () => clearInterval(timer);
		}
		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, [dispatch, answers]);

	//check cheat
	useEffect(() => {
	  let timeOnScreen = 10;
	  const timer = setInterval(() => {
	    if (document.hasFocus()) {
	      timeOnScreen = 10;
	    } else {
	      if (timeOnScreen > 0) {
	        timeOnScreen--;
	      } else {
	        timeOnScreen = 0;
	      }
	    }
	    if (timeOnScreen === 0) {
	      handleSubmit();
	    }
	  }, 1000);
	  return () => {
	    clearInterval(timer);
	  };
	  // eslint-disable-next-line react-hooks/exhaustive-deps
	}, [answers]);

	const prevQues = () => {
		setIndex(index > 0 ? index - 1 : questions.length - 1);
	};

	const nextQues = () => {
		setIndex(index < questions.length - 1 ? index + 1 : 0);
	};

	const handleClickQuestion = (value) => {
		setIndex(value);
	};

	const handleAnswer = (value) => {
		dispatch(setAnswers(value));
	};

	const handleFlag = (value) => {
		if (flags.includes(value)) {
			setFlags(
				flags.filter((item) => {
					return item !== value;
				})
			);
		} else {
			setFlags(flags.concat(value));
		}
	};

	const confirmSubmit = () => {
		let result = window.confirm("Xác nhận nộp bài?");
		if (result) {
			handleSubmit();
		}
	};

	const handleSubmit = () => {
		props.setModalShow(true);
	};

	return (
		<React.Fragment>
			<button className="submitExam" onClick={() => confirmSubmit()}>
				<PublishIcon /> Nộp bài
			</button>
			<div className="left">
				<ExamSidebar
					time={props.timer}
					questions={questions}
					answers={answers}
					flags={flags}
					handleClickQuestion={handleClickQuestion}
					questionId={questions[index] ? questions[index].QuestionTitle : ""}
					handleSubmit={handleSubmit}
				/>
			</div>
			<div className="right">
				<div className="right__btn">
					<div className="right__btn-control">
						<button onClick={() => prevQues()}>
							<p>Câu hỏi trước</p>
						</button>
						<button onClick={() => nextQues()}>
							<p>Câu hỏi kế</p>
						</button>
					</div>
					<button
						className="right__btn-flag"
						onClick={() => {
							handleFlag(questions[index].QuestionTitle);
						}}
					>
						{questions[index] &&
						flags.includes(questions[index].QuestionTitle) ? (
							<FlagIcon />
						) : (
							<FlagOutlinedIcon />
						)}
					</button>
				</div>
				<div className="right__question">
					{questions[index] ? (
						<Question
							correct={corrects[index].split(",")}
							question={questions[index]}
							number={index + 1}
							answers={answers}
							handleAnswer={handleAnswer}
						/>
					) : (
						""
					)}
				</div>
			</div>
		</React.Fragment>
	);
}

export default ExamOpen;
