const initialState = {
	loading: false,
	error: null,
	classes: "",
	numOfPages: 1,
};

const homeReducer = (state = initialState, action) => {
	switch (action.type) {
		case "FETCH_CLASSES_BEGIN": {
			return {
				...state,
				loading: true,
				error: null,
			};
		}
		case "FETCH_CLASSES_SUCCESS": {
			return {
				...state,
				loading: false,
				error: null,
				classes: action.classes,
				numOfPages: action.numOfPages,
			};
		}
		case "FETCH_CLASSES_FAILURE": {
			return {
				...state,
				loading: false,
				error: action.payload,
				classes: "",
				numOfPages: 1,
			};
		}
		default: {
			return {
				...state,
			};
		}
	}
};

export default homeReducer;
