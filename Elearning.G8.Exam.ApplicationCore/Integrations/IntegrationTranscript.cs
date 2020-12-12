using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.G8.Exam.ApplicationCore.Integrations
{
	public class IntegrationTranscript: BaseEntity
	{
		public int? Point { get; set; }
		public string Name { get; set; }
		public string Class { get; set; }
		public string UserCode { get; set; }
	}
}
