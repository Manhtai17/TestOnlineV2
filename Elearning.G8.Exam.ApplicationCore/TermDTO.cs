using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.G8.Exam.ApplicationCore
{
	public class TermDTO : Term
	{
		public Guid ContestId { get; set; }
		public string ContestName { get; set; }
		public DateTime StartTime { get; set; }
		public int TimeToDo { get; set; }
		public DateTime FinishTime { get; set; }
	}
}
