const initialState = {
	loading: false,
	error: null,
	exams: "",
};

const classReducer = (state = initialState, action) => {
	switch (action.type) {
		case "FETCH_EXAMS_BEGIN": {
			return {
				...state,
				loading: true,
				error: null,
			};
		}
		case "FETCH_EXAMS_SUCCESS": {
			return {
				...state,
				loading: false,
				error: null,
				exams: action.payload,
			};
		}
		case "FETCH_EXAMS_FAILURE": {
			return {
				...state,
				loading: false,
				error: action.payload,
				exams: "",
			};
		}
		default: {
			return {
				...state,
			};
		}
	}
};

export default classReducer;
