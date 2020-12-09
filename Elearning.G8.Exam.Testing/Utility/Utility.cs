using Elearning.G8.Exam.ApplicationCore;
using System;
using System.Collections.Generic;

namespace Elearning.G8.Exam.Testing.Utility
{
	public static class Utility
	{
		public static IEnumerable<Question> ExamToObject(string exam)
		{
			var listQuestions = exam.Split("|");
			foreach (var listQuestion in listQuestions)
			{
				var questionArray = listQuestion.Split("%");
				var question = new Question();
				question.QuestionID = questionArray[0];
				question.Type = int.Parse(questionArray[1]);
				switch (question.Type)
				{
					//Dien dap an
					case 0:
						question.Answer = String.Empty;
						break;
					default:
						question.Answer = listQuestion[2].ToString();
						break;
				}
				yield return question;
			}


		}


	}
}
