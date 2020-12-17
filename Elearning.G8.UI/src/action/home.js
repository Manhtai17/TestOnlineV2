export const fetchClasses = (userId, search, page) => {
	return (dispatch) => {
		dispatch(fetchClassesBegin());
		let url = `http://apig8.toedu.me/api/Terms?index=${page}&size=4`;

		if (search) {
			url += `&keyword=${search}`;
		}

		let urlForPages = url.replace(`index=${page}&size=4`, "");

		Promise.all([
			fetch(url, {
				headers: {
					userId: userId,
				},
			}).then((res) => res.json()),
			fetch(urlForPages, {
				headers: {
					userId: userId,
				},
			}).then((res) => res.json()),
		])
			.then((allResults) => {
				dispatch(
					fetchClassesSuccess(
						allResults[0].data,
						Math.ceil(allResults[1].data.length / 4)
					)
				);
			})
			.catch((error) => {
				dispatch(fetchClassesFailure(error.toString()));
			});
	};
};

export const fetchClassesBegin = () => {
	return {
		type: "FETCH_CLASSES_BEGIN",
	};
};

export const fetchClassesSuccess = (classes, numOfPages) => {
	return {
		type: "FETCH_CLASSES_SUCCESS",
		classes: classes,
		numOfPages: numOfPages,
	};
};

export const fetchClassesFailure = (value) => {
	return {
		type: "FETCH_CLASSES_FAILURE",
		payload: value,
	};
};
