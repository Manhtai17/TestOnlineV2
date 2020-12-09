using System.Collections.Generic;

namespace Elearning.G8.Exam.ApplicationCore
{
	public class G10Questtion
	{
		public int Id { get; set; }
		public string Content { get; set; }

		public int Type { get; set; }

		public List<G10Answer> Answers { get; set; }
	}

	public class G10Answer
	{
		public int Id { get; set; }
		public string Content { get; set; }
		public bool IsCorrect { get; set; }
	}

	public class G10Res<T>
	{
		public T Data { get; set; }
		public bool Status { get; set; }
		public string ErrorCode { get; set; }
		public string Parameters { get; set; }

	}

	public class G10Contest
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string SubjectName { get; set; }
		public G10Questtion[] Questions { get; set; }
	}
}
