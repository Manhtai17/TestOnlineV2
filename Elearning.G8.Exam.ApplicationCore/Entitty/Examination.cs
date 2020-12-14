using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.G8.Exam.ApplicationCore
{
	public partial class Examination : BaseEntity
	{
		public Guid ExamId { get; set; }
		public Guid? ContestId { get; set; }
		public Guid? UserId { get; set; }
		public int? Point { get; set; }
		public ulong? Status { get; set; }
		public ulong? IsDoing { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string Question { get; set; }
		public string Answer { get; set; }
		public string Result { get; set; }
		/// <summary>
		/// Thời gian đã làm bài
		/// </summary>
		public double TimeUsing { get; set; } = 0;
		public DateTime? StartAgain { get; set; }


	}
}
