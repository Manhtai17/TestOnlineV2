export const fetchTime = () => {
	return (dispatch) => {
		dispatch(fetchTimeBegin());
		fetch(
			"https://script.googleusercontent.com/macros/echo?user_content_key=lcnhHyDfKaxlHE_kYFyaFoCVqBERo20uX-rO_QEz1j7wUtBjPM4xEBC8zIdZ-fC6ANWY45HamDD2ukkAJA1v7dN7LcBF4rKum5_BxDlH2jW0nuo2oDemN9CCS2h10ox_1xSncGQajx_ryfhECjZEnJ9GRkcRevgjTvo8Dc32iw_BLJPcPfRdVKhJT5HNzQuXEeN3QFwl2n0M6ZmO-h7C6bwVq0tbM60-uGhoxl1-0xbf9WzMOLFEoUrKxLVcJIaW&lib=MwxUjRcLr2qLlnVOLh12wSNkqcO1Ikdrk"
		)
			.then((res) => res.json())
			.then((res) => {
				dispatch(fetchTimeSuccess(res.fulldate));
			})
			.catch((error) => {
				dispatch(fetchTimeFailure(error.toString()));
			});
	};
};

export const fetchTimeBegin = () => {
	return {
		type: "FETCH_TIME_BEGIN",
	};
};

export const fetchTimeSuccess = (value) => {
	return {
		type: "FETCH_TIME_SUCCESS",
		payload: value,
	};
};

export const fetchTimeFailure = (value) => {
	return {
		type: "FETCH_TIME_FAILURE",
		payload: value,
	};
};
