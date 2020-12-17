export const fetchExams = (classId , userId) => {
    return (dispatch) => {
        dispatch(fetchExamsBegin())
        let url = `http://apig8.toedu.me/api/Contests?termID=${classId}`;

        fetch(url, {
            headers: { 
                userId: userId,
            }
        })
            .then((res) => res.json())
            .then((res) => {
                dispatch(fetchExmasSuccess(res.data))
            })
            .catch((error) => {
                dispatch(fetchExmasFailure(error.toString()))
            })
    }
}

export const fetchExamsBegin = () => {
	return {
		type: "FETCH_EXAMS_BEGIN",
	};
};

export const fetchExmasSuccess = (value) => {
	return {
		type: "FETCH_EXAMS_SUCCESS",
		payload: value,
	};
};

export const fetchExmasFailure = (value) => {
	return {
		type: "FETCH_EXAMS_FAILURE",
		payload: value,
	};
};
