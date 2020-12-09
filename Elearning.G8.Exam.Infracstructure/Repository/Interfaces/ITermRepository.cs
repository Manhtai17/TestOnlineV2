using Elearning.G8.Exam.ApplicationCore;
using System.Collections.Generic;

namespace Elearning.G8.Exam.Infrastructure.Repository.Interfaces
{
	public interface ITermRepository
	{
		IEnumerable<Term> Paging(string userID, int pageIndex, int pageSize, string keyword);
	}
}
