using Elearning.G8.Exam.ApplicationCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elearning.G8.Exam.Testing.Interfaces
{
	public interface IContestService
	{
		Task<IEnumerable<Contest>> GetByTermID(string termID, int indexPage, int sizePage, string keyword);
		Task<int> GetTotalRecords(string userID, string keyword);
		Task<ActionServiceResult> CheckScreen(string userID, string contestID, string roleName);
		ActionServiceResult ThongKe(string userID, string contestID, string roleName);
	}
}
