namespace Elearning.G8.Exam.ApplicationCore
{
	public class Question : BaseEntity
	{
		public string QuestionID { get; set; }
		public string QuestionTitle { get; set; }
		public int Type { get; set; }
		public string Answer { get; set; }
	}
}

