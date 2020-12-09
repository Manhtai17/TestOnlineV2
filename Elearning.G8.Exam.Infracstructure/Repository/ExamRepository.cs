using Elearning.G8.Exam.ApplicationCore;
using Elearning.G8.Exam.ApplicationCore.Entitty;
using Elearning.G8.Exam.Infrastructure.Repository.Interfaces;
using System.Collections.Generic;

namespace Elearning.G8.Exam.Infrastructure.Repository
{
	public class ExamRepository : BaseRepository<Examination>, IExamRepository
	{
		public IEnumerable<Examination> GetExamByContestID(string contestID)
		{
			var result = GetEntities("Proc_GetExamByUserID", new object[] { contestID });
			return result;
		}
	}
}
