import { LinearProgress } from "@material-ui/core";
import React, { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import Modal from "../Modal";

function Result(props) {
  const userinfo = useSelector((state) => state.auth.userinfo);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [result, setResult] = useState("");

  const tags = "ABCDEFGHIJKLMNOPQKSTUVWXYZ";

  useEffect(() => {
    setLoading(true);
    fetch(
      `http://localhost:3001/result?examId=${props.exam.id}&userId=${userinfo.id}`
    )
      .then((res) => res.json())
      .then((res) => {
        setLoading(false);
        setResult(res);
        setError(null);
      })
      .catch((error) => {
        setLoading(false);
        setResult("");
        setError(error.toString());
      });
  }, [props.exam.id, userinfo.id]);

  return (
    <Modal
      close={props.close}
      title={`Kết quả bài kiểm tra số ${props.exam.id}`}
    >
      <div className="resultModal">
        {error ? (
          <div className="error">{error}</div>
        ) : loading || !result ? (
          <LinearProgress className="loadingbar" />
        ) : (
          <React.Fragment>
            <div className="resultModal__head">
              <div className="resultModal__head-score">
                {`Điểm: ${result[0].score}`}
              </div>
              <div className="resultModal__head-score">
                {`Thời điểm nộp bài: ${result[0].timeSubmit}`}
              </div>
            </div>
            <div className="resultModal__body">
              <div className="resultModal__body-title">
                Danh sách câu trả lời
              </div>
              <div className="questionsList">
                {props.exam.questions.map((quesValue, key) => (
                  <div className="questionsList__item" key={quesValue.id}>
                    <div className="questionsList__item-title">{`Câu ${
                      key + 1
                    }. ${quesValue.title}`}</div>
                    <div className="questionsList__item-answers">
                      {quesValue.answer.map((answerValue) => (
                        <div
                          className={
                            result[0].answers[quesValue.id] &&
                            result[0].answers[quesValue.id].includes(answerValue.id)
                              ? "questionsList__item-answers-item questionsList__item-answers-item--active"
                              : "questionsList__item-answers-item"
                          }
                          key={answerValue.id}
                        >
                          {`${tags[answerValue.id - 1]}. ${
                            answerValue.content
                          }`}
                        </div>
                      ))}
                    </div>
                  </div>
                ))}
              </div>
            </div>
          </React.Fragment>
        )}
      </div>
    </Modal>
  );
}

export default Result;
