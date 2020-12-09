using Elearning.G8.Exam.ApplicationCore;
using System.Threading.Tasks;

namespace Elearning.G8.Exam.Testing.Interfaces
{
	public interface IExamService
	{
		//public Task<ActionServiceResult> GetByUserID(string contestID);
		//public Task<ExamDTO> GetByUserID(string userID, string contestID);
		Task<ActionServiceResult> GetByUserID(string userID, string contestID, string roleName);
		Task<Examination> CreateExam(string contestID, string userID);
	}
}
