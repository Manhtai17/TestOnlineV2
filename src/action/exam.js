export const fetchExam = (examId, userId) => {
	return (dispatch) => {
		dispatch(fetchExamBegin());
		let url = `http://apig8.toedu.me/api/Exams/${examId}`;

		fetch(url, {
			headers: {
				userId: userId,
			},
		})
			.then((res) => res.json())
			.then((results) => {
				if (results.code === 604) {
					alert("Bài kiểm tra hiện đang được làm, vui lòng quay lại sau");
					window.history.back();
				} else {
					dispatch(fetchExamSuccess(results.data));
				}
			})
			.catch((error) => {
				dispatch(fetchExamFailure(error.toString()));
			});
	};
};

export const submitExam = (
	examId,
	contestId,
	userId,
	status,
	isDoing,
	result
) => {
	return (dispatch) => {
		dispatch(submitExamBegin());
		let url = `http://apig8.toedu.me/api/Exams/submit`;

		fetch(url, {
			method: "POST",
			headers: {
				"Content-Type": "application/json",
				userId: userId,
			},
			body: JSON.stringify({
				examId: examId,
				contestId: contestId,
				userId: userId,
				status: status,
				isDoing: isDoing,
				result: result,
			}),
		})
			.then((res) => res.json())
			.then((results) => {
				dispatch(submitExamSuccess(results.data));
			})
			.catch((error) => {
				dispatch(submitExamFailure(error.toString()));
			});
	};
};

export const fetchExamBegin = () => {
	return {
		type: "FETCH_EXAM_BEGIN",
	};
};

export const fetchExamSuccess = (exam) => {
	return {
		type: "FETCH_EXAM_SUCCESS",
		exam: exam,
	};
};

export const fetchExamFailure = (value) => {
	return {
		type: "FETCH_EXAM_FAILURE",
		payload: value,
	};
};

export const submitExamBegin = () => {
	return {
		type: "SUBMIT_EXAM_BEGIN",
	};
};

export const submitExamSuccess = (score) => {
	return {
		type: "SUBMIT_EXAM_SUCCESS",
		score: score,
	};
};

export const submitExamFailure = (value) => {
	return {
		type: "SUBMIT_EXAM_FAILURE",
		payload: value,
	};
};

export const setAnswers = (value) => {
	return {
		type: "SET_ANSWERS",
		payload: value,
	};
};
