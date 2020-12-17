const initialState = {
	statLoading: false,
	statError: null,
	stat: "",
};

const statReducer = (state = initialState, action) => {
	switch (action.type) {
		case "FETCH_STAT_BEGIN": {
			return {
				...state,
				statLoading: true,
				statError: null,
			};
		}
		case "FETCH_STAT_SUCCESS": {
			return {
				...state,
				statLoading: false,
				statError: null,
				stat: action.payload,
			};
		}
		case "FETCH_STAT_FAILURE": {
			return {
				...state,
				statLoading: false,
				statError: action.payload,
				stat: "",
			};
		}
		default: {
			return {
				...state,
			};
		}
	}
};

export default statReducer;
