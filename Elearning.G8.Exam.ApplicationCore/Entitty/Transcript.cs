using System;

namespace Elearning.G8.Exam.ApplicationCore
{
	/// <summary>
	///Bảng đ
	///
	/// </summary>
	public class Transcript : ExamDTO
	{
		public string Name { get; set; }
		public string Class { get; set; }
		public string UserCode { get; set; }

		public DateTime DateOfBirth { get; set; }
	}
}
