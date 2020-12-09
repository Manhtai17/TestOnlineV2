using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.G8.Exam.ApplicationCore
{
	public class QuestionDescription
	{
		/// <summary>
		/// Câu hỏi 
		/// </summary>
		public string Question { get; set; }

		/// <summary>
		/// Loại câu hỏi
		/// </summary>
		public int Type { get; set; }

		/// <summary>
		/// Câu trả lời cho câu hỏi
		/// </summary>
		public string Answer { get; set; }
	}
}
