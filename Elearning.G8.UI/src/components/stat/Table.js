import React from "react";
import ReactHTMLTableToExcel from "react-html-table-to-excel";

function Table(props) {
	return (
		<div className="Table">
			<ReactHTMLTableToExcel
				id="test-table-xls-button"
				className="download-table-xls-button"
				table="table-to-xls"
				filename="Bảng điểm"
				sheet="tablexls"
				buttonText="Tải xuống file XLS"
			/>
			<table id="table-to-xls">
				<tr>
					<th>STT</th>
					<th>Họ và tên</th>
					<th>Lớp</th>
					<th>Điểm</th>
				</tr>
				{props.stat.map((value, key) => (
					<tr key={key}>
						<td>{key + 1}</td>
						<td>{value.name}</td>
						<td>{value.class}</td>
						<td>{value.point}</td>
					</tr>
				))}
			</table>
		</div>
	);
}

export default Table;
