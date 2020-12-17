import React, { useEffect } from "react";
import "./sass/style.sass";
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import ScrollToTop from "./components/ScrollToTop";
import Exam from "./pages/Exam";
import Login from "./pages/Login";
import { useDispatch, useSelector } from "react-redux";
import { authorize } from "./action/auth";
import Homepage from "./pages/Homepage";
import Class from "./pages/Class";
import LostConnection from "./components/LostConnection";
import { LinearProgress } from "@material-ui/core";
import Stat from "./pages/Stat";

function App() {
	const dispatch = useDispatch();
	const userinfo = useSelector((state) => state.auth.userinfo);
	const userinfoLoading = useSelector((state) => state.auth.userinfoLoading);
	const userinfoError = useSelector((state) => state.auth.userinfoError);
	useEffect(() => {
		dispatch(authorize());
	}, [dispatch]);

	if (userinfoError) {
		return <div className="error">{userinfoError}</div>;
	} else if (userinfoLoading) {
		return <LinearProgress className="loadingbar" />;
	} else if (userinfo) {
		if (userinfo.userID === "08d89e0d-5cd4-47c0-8207-4a4aefb89615") {
			return (
				<Router>
					<LostConnection />
					<ScrollToTop />
					<div className="App">
						<Switch>
							<Route exact path="/">
								<Homepage />
							</Route>
							<Route exact path="/stat/:contestId">
								<Stat />
							</Route>
							<Route exact path="/class/:classId">
								<Class />
							</Route>
						</Switch>
					</div>
				</Router>
			);
		} else {
			return (
				<Router>
					<LostConnection />
					<ScrollToTop />
					<div className="App">
						<Switch>
							<Route exact path="/">
								<Homepage />
							</Route>
							<Route exact path="/exam/:contestId/:examId">
								<Exam />
							</Route>
							<Route exact path="/class/:classId">
								<Class />
							</Route>
						</Switch>
					</div>
				</Router>
			);
		}
	} else {
		return (
			<Router>
				<LostConnection />
				<Login />
			</Router>
		);
	}
}

export default App;
