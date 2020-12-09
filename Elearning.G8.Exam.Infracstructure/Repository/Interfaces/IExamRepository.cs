using Elearning.G8.Exam.ApplicationCore;
using Elearning.G8.Exam.ApplicationCore.Entitty;
using System.Collections.Generic;

namespace Elearning.G8.Exam.Infrastructure.Repository.Interfaces
{
	public interface IExamRepository : IBaseRepository<Examination>
	{
		public IEnumerable<Examination> GetExamByContestID(string contestID);
	}
}
