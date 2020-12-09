using Elearning.G8.Exam.ApplicationCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elearning.G8.Exam.Testing.Interfaces
{
	public interface ITermService
	{
		Task<IEnumerable<Term>> Paging(string userID, int pageIndex, int pageSize, string keyword);
		Task<int> GetTotalRecords(string userID, string keyword);
	}
}
