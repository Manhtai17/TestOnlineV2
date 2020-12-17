import { CircularProgress, LinearProgress } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import Chart from "react-google-charts";

function Scorechart(props) {
	const [data, setData] = useState("");

	useEffect(() => {
		let data = [["Điểm", "Điểm"]];
		for (let i = 0; i < props.stat.length; i++) {
			let item = [];
			item.push(props.stat[i].name);
			item.push(props.stat[i].point);
			data.push(item);
		}
		setData(data);
		// eslint-disable-next-line react-hooks/exhaustive-deps
	}, []);

	if (data) {
		return (
			<div className="Chart">
				<Chart
					width={900}
					height={500}
					chartType="ColumnChart"
					loader={<CircularProgress className="loadingCircle" />}
					data={data}
					options={{
						title: "Biểu đồ cột phổ điểm",
						chartArea: { width: "60%" },
						hAxis: {
							title: "Sinh viên",
							minValue: 0,
						},
						vAxis: {
							title: "Điểm",
						},
					}}
					legendToggle
				/>
			</div>
		);
	} else {
		return <LinearProgress className="loadingbar" />;
	}
}

export default Scorechart;
