using System;

namespace Elearning.G8.Exam.ApplicationCore
{
	public class ContestDTO : Contest
	{
		/// <summary>
		/// Trang thai 0-nut bat dau, 1-tiep tuc lam bai, 2- thong ke
		/// </summary>
		public int Continue { get; set; }
		public Guid? ExamID { get; set; }

		/// <summary>
		/// Thời gian còn lại
		/// </summary>
		public long? TimeRemaining { get; set; }
	}
}
