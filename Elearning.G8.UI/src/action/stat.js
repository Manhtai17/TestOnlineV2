export const fetchStat = (contestId, userId) => {
	return (dispatch) => {
		dispatch(fetchStatBegin());
		let url = `http://apig8.toedu.me/api/Contests/ScoreStatistics?contestID=${contestId}`;

		fetch(url, {
			headers: {
				userId: userId,
			},
		})
			.then((res) => res.json())
			.then((result) => {
				dispatch(fetchStatSuccess(result.data));
			})
			.catch((error) => {
				dispatch(fetchStatFailure(error.toString()));
			});
	};
};

export const fetchStatBegin = () => {
	return {
		type: "FETCH_STAT_BEGIN",
	};
};

export const fetchStatSuccess = (value) => {
	return {
		type: "FETCH_STAT_SUCCESS",
		payload: value,
	};
};

export const fetchStatFailure = (value) => {
	return {
		type: "FETCH_STAT_FAILURE",
		payload: value,
	};
};
