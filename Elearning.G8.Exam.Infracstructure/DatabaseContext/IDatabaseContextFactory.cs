using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.G8.Exam.Infrastructure.DatabaseContext
{
	public interface IDatabaseContextFactory
	{
		IDatabaseContext Context();
	}
}
