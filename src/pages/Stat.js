import React, { useEffect, useState } from "react";
import Footer from "../components/Footer";
import Header from "../components/Header";
import TableChartIcon from "@material-ui/icons/TableChart";
import InsertChartIcon from "@material-ui/icons/InsertChart";
import Table from "../components/stat/Table";
import Scorechart from "../components/stat/Chart";
import { useDispatch, useSelector } from "react-redux";
import { LinearProgress } from "@material-ui/core";
import { useParams } from "react-router-dom";
import { fetchStat } from "../action/stat";

function Stat() {
	const contestId = useParams().contestId;
	const [tab, setTab] = useState(localStorage.getItem("statSite") || "table");
	const statLoading = useSelector((state) => state.stat.statLoading);
	const statError = useSelector((state) => state.stat.statError);
	const userinfo = useSelector((state) => state.auth.userinfo);
	const stat = useSelector((state) => state.stat.stat);
	const dispatch = useDispatch();

	useEffect(() => {
		dispatch(fetchStat(contestId, userinfo.userID));
	}, [dispatch, contestId, userinfo.userID]);

	if (statError) {
		return <div className="error">{statError}</div>;
	} else if (statLoading) {
		return <LinearProgress className="loadingbar" />;
	} else {
		return (
			<div className="Stat">
				<Header>
					<div className="tabs">
						<div
							className={
								tab === "table" ? "tabs__item tabs__item--active" : "tabs__item"
							}
							onClick={() => {
								setTab("table");
								localStorage.setItem("statSite", "table");
							}}
						>
							<TableChartIcon />
							<p>Bảng thống kê</p>
						</div>
						<div
							className={
								tab === "chart" ? "tabs__item tabs__item--active" : "tabs__item"
							}
							onClick={() => {
								setTab("chart");
								localStorage.setItem("statSite", "chart");
							}}
						>
							<InsertChartIcon />
							<p>Biểu đồ</p>
						</div>
					</div>
				</Header>
				<main className="container">
					{stat.length === 0 ? (
						<div className="no-data">Không có dữ liệu hiển thị</div>
					) : tab === "table" ? (
						<Table stat={stat} />
					) : (
						<Scorechart stat={stat} />
					)}
				</main>
				<Footer />
			</div>
		);
	}
}

export default Stat;
