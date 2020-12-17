import { LinearProgress } from "@material-ui/core";
import { SearchOutlined } from "@material-ui/icons";
import { Pagination } from "@material-ui/lab";
import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Link } from "react-router-dom";
import { fetchExams } from "../action/class";
import { fetchClasses } from "../action/home";
import Footer from "../components/Footer";
import Header from "../components/Header";
import ExamItem from "../components/ExamItem";

function Homepage() {
	const loading = useSelector((state) => state.home.loading);
	const error = useSelector((state) => state.home.error);
	const classes = useSelector((state) => state.home.classes);
	const numOfPages = useSelector((state) => state.home.numOfPages);
	const [search, setSearch] = useState("");
	const [page, setPage] = useState(1);
	const userinfo = useSelector((state) => state.auth.userinfo);
	const dispatch = useDispatch();

	useEffect(() => {
		dispatch(fetchClasses(userinfo.userID, search, page));
	}, [dispatch, userinfo.userID, search, page]);

	const handlePageChange = (value) => {
		setPage(value);
		window.scrollTo({
			top: 0,
			behavior: "smooth",
		});
	};

	return (
		<div className="Homepage">
			<Header />
			<main className="container">
				<div className="classes">
					<div className="menu">
						<div className="menu__left">Lớp học của bạn</div>
						<div className="menu__right">
							<input
								className="menu__right-input"
								placeholder="Tìm lớp học"
								value={search}
								onChange={(value) => {
									setSearch(value.target.value);
								}}
							/>
							<div className="menu__right-icon">
								<SearchOutlined />
							</div>
						</div>
					</div>
					<div className="content">
						{error ? (
							<div className="error">{error}</div>
						) : loading || !classes ? (
							<LinearProgress className="loadingbar" />
						) : classes.length === 0 ? (
							<div className="content__error">Không có kết quả tìm kiếm</div>
						) : (
							<div className="content__wrapper">
								<div className="content__list">
									{classes.map((value) => (
										<Link
											to={`/class/${value.termId}`}
											className="content__list-item"
											key={value.termId}
										>
											<div>
												<div
													className="content__list-item-image"
													style={{
														background: `url(https://images.unsplash.com/photo-1557683316-973673baf926?ixlib=rb-1.2.1&ixid=MXwxMjA3fDB8MHxleHBsb3JlLWZlZWR8Mnx8fGVufDB8fHw%3D&w=1000&q=80) no-repeat center`,
													}}
												></div>
												<div className="content__list-item-semester">
													{value.termCode}
												</div>
												<div className="content__list-item-name">
													{value.termName}
												</div>
											</div>
											<button>Truy cập</button>
										</Link>
									))}
								</div>
								{/* <Pagination
									className="pagination"
									count={parseInt(numOfPages)}
									color="primary"
									defaultPage={page}
									shape="rounded"
									onChange={(event, value) => handlePageChange(value)}
								/> */}
							</div>
						)}
					</div>
				</div>
			</main>
			<Footer />
		</div>
	);
}

export default Homepage;
