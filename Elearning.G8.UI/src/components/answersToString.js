export const answersToString = (questions, answers) => {
	let result = "";
	for (let i = 0; i < questions.length; i++) {
		if (answers[questions[i].QuestionTitle]) {
			result = result + "|" + answers[questions[i].QuestionTitle].join(",");
		} else {
			result += "|";
		}
	}
	return result;
};
