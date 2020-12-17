const initialState = {
	examLoading: false,
	examError: null,
	exam: "",
	submitLoading: false,
	submitError: null,
	score: "",
	answers: {},
};

const examReducer = (state = initialState, action) => {
	switch (action.type) {
		case "FETCH_EXAM_BEGIN": {
			return {
				...state,
				examLoading: true,
				examError: null,
			};
		}
		case "FETCH_EXAM_SUCCESS": {
			return {
				...state,
				examLoading: false,
				examError: null,
				exam: action.exam,
			};
		}
		case "FETCH_EXAM_FAILURE": {
			return {
				...state,
				examLoading: false,
				exam: "",
				examError: action.payload,
			};
		}
		case "SUBMIT_EXAM_BEGIN": {
			return {
				...state,
				submitLoading: true,
				submitError: null,
			};
		}
		case "SUBMIT_EXAM_SUCCESS": {
			return {
				...state,
				submitLoading: false,
				score: action.score,
				submitError: null,
			};
		}
		case "SUBMIT_EXAM_FAILURE": {
			return {
				...state,
				submitLoading: false,
				score: "",
				submitError: action.payload,
			};
		}
		case "SET_ANSWERS": {
			return {
				...state,
				answers: action.payload,
			};
		}
		default: {
			return {
				...state,
			};
		}
	}
};

export default examReducer;
