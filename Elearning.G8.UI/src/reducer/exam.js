const initialState = {
	examLoading: false,
	examError: null,
	exam: "",
	submitLoading: false,
	submitError: null,
	result: "",
	resultLoading: false,
	resultError: null,
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
				count: action.count,
			};
		}
		case "SUBMIT_EXAM_FAILURE": {
			return {
				...state,
				submitLoading: false,
				submitError: action.payload,
			};
		}
		case "FETCH_RESULT_SUCCESS": {
			return {
				...state,
				resultLoading: false,
				resultError: null,
				result: action.result,
			};
		}
		case "FETCH_RESULT_BEGIN": {
			return {
				...state,
				resultLoading: true,
				resultError: null,
			};
		}
		case "FETCH_RESULT_FAILURE": {
			return {
				...state,
				resultLoading: false,
				result: "",
				resultError: action.payload,
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
