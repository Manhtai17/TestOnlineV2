export const logIn = (user) => {
  return (dispatch) => {
    dispatch(loginUserBegin());
    fetch("http://apig8.toedu.me/api/Integrations/login",
			{
				method: "POST",
				headers: {
					"Content-Type": "application/json",
					Accept: "application/json",
				},
				body: JSON.stringify(user),
			}
		)
			.then((res) => res.json())
			.then((data) => {
				if (data.authorization) {
          localStorage.setItem("token", data.authorization);
          localStorage.setItem("user_id", data.userId);
					dispatch(loginUserSuccess(data.authorization, data.userId));
					window.location.reload();
				} else {
					dispatch(loginUserSuccess(""));
					alert("Thông tin đăng nhập không chính xác");
				}
			})
			.catch((error) => {
				dispatch(loginUserFailure(error.toString()));
			});
  };
};

export const authorize = () => {
  return (dispatch) => {
    const token = localStorage.getItem("token");
    const id = localStorage.getItem("user_id");
    if (token && id) {
      dispatch(getUserInfoBegin());
      fetch(`http://apig8.toedu.me/api/Users/${id}`)
				.then((res) => res.json())
				.then((result) => {
					dispatch(getUserInfoSuccess(result.data));
				})
				.catch((error) => {
					dispatch(getUserInfoFailure(error.toString()))
				});
    }
  };
};

export const logOut = () => {
  localStorage.removeItem("token");
  localStorage.removeItem("user_id");
  return {
    type: "LOG_OUT",
  };
};

export const loginUserBegin = () => {
  return {
    type: "LOGIN_USER_BEGIN",
  };
};

export const loginUserSuccess = (token, id) => {
  return {
    type: "LOGIN_USER_SUCCESS",
    token: token,
    id: id,
  };
};

export const loginUserFailure = (value) => {
  return {
    type: "LOGIN_USER_FAILURE",
    payload: value,
  };
};

export const getUserInfoSuccess = (value) => {
  return {
    type: "GET_USER_INFO_SUCCESS",
    payload: value,
  };
};

export const getUserInfoBegin = () => {
  return {
    type: "GET_USER_INFO_BEGIN",
  }
}

export const getUserInfoFailure = (value) => {
  return {
    type: "GET_USER_INFO_FAILURE",
    payload: value,
  }
}
