import React from "react";
import Timer from "./Timer";

function ExamSidebar(props) {
	return (
		<div className="ExamSidebar">
			<div className="ExamSidebar__timer">
				<Timer time={props.time} handleSubmit={props.handleSubmit} />
			</div>
			<p className="ExamSidebar__count">
				Số câu đã làm: <span>{Object.keys(props.answers).length}</span>
			</p>
			<div className="ExamSidebar__questions">
				<p className="ExamSidebar__questions-titlelist">Danh sách câu hỏi</p>
				<div className="ExamSidebar__questions-list">
					{props.questions.map((value, key) => (
						<div
							className={
								props.flags.includes(value.QuestionTitle)
									? "ExamSidebar__questions-list-item ExamSidebar__questions-list-item--flag"
									: props.answers[value.QuestionTitle]
									? "ExamSidebar__questions-list-item ExamSidebar__questions-list-item--done"
									: "ExamSidebar__questions-list-item"
							}
							style={
								props.questionId === value.QuestionTitle
									? { background: "#4796f5", color: "white" }
									: {}
							}
							key={key}
							onClick={() => {
								props.handleClickQuestion(key);
							}}
						>
							{key + 1}
						</div>
					))}
				</div>
			</div>
		</div>
	);
}

export default ExamSidebar;
