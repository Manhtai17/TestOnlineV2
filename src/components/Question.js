import React from "react";
import { Checkbox } from "@material-ui/core";

function Question(props) {
	const question = props.question;
	const tags = "ABCDEFGHIJKLMNOPQKSTUVWXYZ";

	const handleClickAnswer = (value) => {
		if (question.type !== 3) {
			props.handleAnswer({
				...props.answers,
				[question.QuestionTitle]: [value],
			});
		} else {
			if (props.answers[question.QuestionTitle]) {
				let checked = props.answers[question.QuestionTitle];
				if (checked.includes(value)) {
					checked = checked.filter((item) => {
						return item !== value;
					});
				} else {
					checked.push(value);
				}
				if (checked.length === 0) {
					let newObj = { ...props.answers };
					delete newObj[question.QuestionTitle];
					props.handleAnswer(newObj);
				} else {
					props.handleAnswer({
						...props.answers,
						[question.QuestionTitle]: checked,
					});
				}
			} else {
				props.handleAnswer({
					...props.answers,
					[question.QuestionTitle]: [value],
				});
			}
		}
	};

	if (question.type === 1) {
		return (
			<div className="Question">
				<div className="title">
					{`Câu ${props.number}. `}
					<span>{question.QuestionTitle}</span>
				</div>
				<div className="answers">
					<div
						className={
							props.answers[question.QuestionTitle] &&
							props.answers[question.QuestionTitle].includes("true")
								? "answers__item answers__item--active"
								: "answers__item"
						}
						onClick={() => {
							handleClickAnswer("true");
						}}
					>
						<div className="answers__item-checkbox">
							<Checkbox
								size="small"
								checked={
									props.answers[question.QuestionTitle] &&
									props.answers[question.QuestionTitle].includes("true")
										? true
										: false
								}
							/>
						</div>
						<div className="answers__item-content">A. Đúng</div>
					</div>
					<div
						className={
							props.answers[question.QuestionTitle] &&
							props.answers[question.QuestionTitle].includes("false")
								? "answers__item answers__item--active"
								: "answers__item"
						}
						onClick={() => {
							handleClickAnswer("false");
						}}
					>
						<div className="answers__item-checkbox">
							<Checkbox
								size="small"
								checked={
									props.answers[question.QuestionTitle] &&
									props.answers[question.QuestionTitle].includes("false")
										? true
										: false
								}
							/>
						</div>
						<div className="answers__item-content">B. Sai</div>
					</div>
				</div>
			</div>
		);
	} else {
		return (
			<div className="Question">
				<div className="title">
					{`Câu ${props.number}. `}
					<span>{question.QuestionTitle}</span>
				</div>
				<div className="answers">
					{question.Answer
						? question.Answer.substring(1)
								.slice(0, -1)
								.split("|")
								.map((value, key) => (
									<div
										className={
											props.answers[question.QuestionTitle] &&
											props.answers[question.QuestionTitle].includes(value)
												? "answers__item answers__item--active"
												: "answers__item"
										}
										key={key}
										onClick={() => {
											handleClickAnswer(value);
										}}
									>
										<div className="answers__item-checkbox">
											<Checkbox
												size="small"
												checked={
													props.answers[question.QuestionTitle] &&
													props.answers[question.QuestionTitle].includes(value)
														? true
														: false
												}
											/>
										</div>
										<div className="answers__item-content">
											{tags[key]}. {value}
										</div>
									</div>
								))
						: ""}
				</div>
			</div>
		);
	}
}

export default Question;
