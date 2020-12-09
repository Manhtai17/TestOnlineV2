using System;

namespace Elearning.G8.Exam.ApplicationCore
{
	public partial class Contest : BaseEntity
	{
		public Guid ContestId { get; set; }
		public string ContestName { get; set; }
		public Guid? TermId { get; set; }
		public DateTime StartTime { get; set; }
		public int TimeToDo { get; set; }
		public DateTime FinishTime { get; set; }

	}
}
