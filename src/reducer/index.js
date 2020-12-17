import { combineReducers } from "redux";
import authReducer from "./auth";
import classReducer from "./class";
import examReducer from "./exam";
import homeReducer from "./home";
import statReducer from "./stat";
import timeReducer from "./time";

const rootReducer = combineReducers({
	exam: examReducer,
	auth: authReducer,
	home: homeReducer,
	class: classReducer,
	time: timeReducer,
	stat: statReducer,
});

export default rootReducer;
