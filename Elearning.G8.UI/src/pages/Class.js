import { LinearProgress } from "@material-ui/core";
import React, { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { useParams } from "react-router-dom";
import { fetchExams } from "../action/class";
import ExamItem from "../components/ExamItem";
import Footer from "../components/Footer";
import Header from "../components/Header";

function Class() {
	const loading = useSelector((state) => state.class.loading);
	const error = useSelector((state) => state.class.error);
	const exams = useSelector((state) => state.class.exams);
	const userinfo = useSelector((state) => state.auth.userinfo);
	const classId = useParams().classId;
	const dispatch = useDispatch();

	useEffect(() => {
		dispatch(fetchExams(classId, userinfo.userID));
	}, [dispatch, classId, userinfo.userID]);

	console.log(exams);

	if (error) return <div className="error">{error}</div>;
	else if (loading || !exams) return <LinearProgress className="loadingbar" />;
	else
		return (
			<div className="Class">
				<Header />
				<main className="container">
					<div className="title">Danh sách bài kiểm tra</div>
					<div className="content">
						{exams.map((value) => (
							sessionStorage.setItem(value.contestId, value.timeToDo),
							<ExamItem
								examId={value.contestId}
								key={value.contestId}
								examName={value.contestName}
								timeToDo={value.timeToDo}
								startTime={value.startTime}
								finishTime={value.finishTime}
							/>
						))}
					</div>
				</main>
				<Footer />
			</div>
		);
}

export default Class;
