const initialState = {
	loading: false,
	error: null,
	token: "",
	id: "",
	userinfo: "",
	userinfoLoading: false,
	userinfoError: null,
};

const authReducer = (state = initialState, action) => {
	switch (action.type) {
		case "LOGIN_USER_BEGIN": {
			return {
				...state,
				loading: true,
				error: null,
			};
		}
		case "LOGIN_USER_SUCCESS": {
			return {
				...state,
				token: action.token,
				id: action.id,
				loading: false,
			};
		}
		case "LOGIN_USER_FAILURE": {
			return {
				...state,
				loading: false,
				error: action.payload,
				token: "",
			};
		}
		case "GET_USER_INFO_BEGIN": {
			return {
				...state,
				userinfoLoading: true,
				userinfoError: null,
			}
		}
		case "GET_USER_INFO_SUCCESS": {
			return {
				...state,
				userinfo: action.payload,
				userinfoLoading: false,
				userinfoError: null,
			};
		}
		case "GET_USER_INFO_FAILURE": {
			return {
				...state,
				userinfo: "",
				userinfoLoading: false,
				userinfoError: action.payload,
			}
		}
		case "LOG_OUT": {
			return {
				...state,
				token: "",
				id: "",
				userinfo: "",
			};
		}
		default: {
			return {
				...state,
			};
		}
	}
};

export default authReducer;
