using System;

namespace Elearning.G8.Exam.ApplicationCore
{
	public class ExamDTO : BaseEntity
	{
		public Guid ExamId { get; set; }
		public Guid? ContestId { get; set; }
		public Guid? UserId { get; set; }
		public int? Point { get; set; }
		public ulong? Status { get; set; }
		public ulong? IsDoing { get; set; }
		public DateTime? CreatedDate { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public object Question { get; set; }
		public string Result { get; set; }
	}
}
