import React, { useEffect, useState } from "react";

function LostConnection() {
  const [show, setShow] = useState(false);

  useEffect(() => {
    const timer = setInterval(() => {
      if (!window.navigator.onLine) {
        setShow(true);
      } else setShow(false);
    }, 1000);
    return () => {
      clearTimeout(timer);
    };
  }, []);

  if (show) {
    return (
      <div className="LostConnection">
        <p>Mất kết nối, vui lòng kiểm tra đường truyền internet của bạn!</p>
        <a href=".">Tải lại trang</a>
      </div>
    );
  } else return "";
}

export default LostConnection;