using System;
using System.Collections.Generic;

namespace Elearning.G8.Exam.ApplicationCore
{
	public partial class Term : BaseEntity
	{
		public Guid TermId { get; set; }
		public string TermName { get; set; }
		public int? Classes { get; set; }

		public string TermCode { get; set; }

	}
}
