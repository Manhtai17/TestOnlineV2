using Elearning.G8.Exam.ApplicationCore;
using Elearning.G8.Exam.Infrastructure.Repository.Interfaces;
using System.Collections.Generic;

namespace Elearning.G8.Exam.Infrastructure.Repository
{
	public class TermRepository : BaseRepository<Term>, ITermRepository
	{
		public IEnumerable<Term> Paging(string userID, int pageIndex, int pageSize, string keyword)
		{
			var result = GetEntitites("Proc_GetTermsPaging", new object[] { userID, pageIndex, pageSize, keyword }).Result;
			return result;
		}
	}
}
