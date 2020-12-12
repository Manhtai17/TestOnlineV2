using Elearning.G8.Exam.ApplicationCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elearning.G8.Exam.Testing.Interfaces
{
	public interface ITermService
	{
		Task<ActionServiceResult> Paging(string userID, int pageIndex = 1, int pageSize = 99, string keyword = null);

		Task<ActionServiceResult> GetByTermID(string termID);
		Task<int> GetTotalRecords(string userID, string keyword);
	}
}
