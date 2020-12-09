using Elearning.G8.Exam.ApplicationCore;
using System.Collections.Generic;

namespace Elearning.G8.Exam.Infrastructure.Repository.Interfaces
{
	public interface IContestRepository
	{
		IEnumerable<Contest> GetByTermID(string userID, int indexPage, int sizePage, string keyword);
	}
}
