using Elearning.G8.Exam.ApplicationCore;
using Elearning.G8.Exam.Infrastructure.Repository.Interfaces;
using System.Collections.Generic;

namespace Elearning.G8.Exam.Infrastructure.Repository
{
	public class ContestRepository : BaseRepository<Contest>, IContestRepository
	{
		public IEnumerable<Contest> GetByTermID(string termID, int indexPage, int sizePage, string keyword)
		{
			var result = (GetEntitites("Proc_GetContestsByTermID", new object[] { termID, indexPage, sizePage, keyword })).Result;
			return result;
		}
	}
}
