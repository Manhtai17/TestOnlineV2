const initialState = {
	timeLoading: false,
	timeError: null,
	time: "",
};

const timeReducer = (state = initialState, action) => {
	switch (action.type) {
		case "FETCH_TIME_BEGIN": {
			return {
				...state,
				timeLoading: true,
				timeError: null,
			};
		}
		case "FETCH_TIME_SUCCESS": {
			return {
				...state,
				timeLoading: false,
				timeError: null,
				time: action.payload,
			};
		}
		case "FETCH_TIME_FAILURE": {
			return {
				...state,
				timeLoading: false,
				timeError: action.payload,
				time: "",
			};
		}
		default: {
			return {
				...state,
			};
		}
	}
};

export default timeReducer;
